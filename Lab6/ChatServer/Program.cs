using System.Net;
using System.Net.Sockets;

int port = 5555;
if (args.Length > 0)
{
    int parsed;
    if (int.TryParse(args[0], out parsed) && parsed >= 1 && parsed <= 65535)
    {
        port = parsed;
    }
    else
    {
        Console.WriteLine("Неверный порт, используется 5555.");
    }
}


var users = new Dictionary<string, StreamWriter>(StringComparer.OrdinalIgnoreCase);
var lockObj = new object(); //список клиентов
var logLock = new object(); // журнал


var listener = new TcpListener(IPAddress.Any, port);
listener.Start();
Console.WriteLine("Журнал пишется в файл: " + Path.GetFullPath("chat.log"));
Log("Сервер запущен на порту " + port);

while (true)
{
    TcpClient client = await listener.AcceptTcpClientAsync();
    Task task = HandleClientAsync(client);// каждого клиента - в свою задачу
}


async Task HandleClientAsync(TcpClient client)
{
    var endpoint = client.Client.RemoteEndPoint;
    var stream = client.GetStream();
    var reader = new StreamReader(stream);
    var writer = new StreamWriter(stream) { AutoFlush = true };

    string nick = "";
    try
    {
        while (nick == "")
        {
            string? candidate = await reader.ReadLineAsync();
            if (candidate == null)
            {
                return;   // не представился:(
            }

            candidate = candidate.Trim();
            if (candidate == "" || candidate.Contains(' '))
            {
                await writer.WriteLineAsync("BAD");
                continue;
            }

            bool added;
            lock (lockObj)
            {
                added = users.TryAdd(candidate, writer);   // проверка и добавление
            }

            if (added)
            {
                nick = candidate;
                await writer.WriteLineAsync("OK");
            }
            else
            {
                await writer.WriteLineAsync("BUSY");
            }
        }

        Log($"{nick} подключился ({endpoint})");
        await BroadcastAsync($"* {nick} вошёл в чат *");


        while (true)
        {
            string? line = await reader.ReadLineAsync();
            if (line == null || line == "/exit")
            {
                break;   // соединение закрыто или клиент вышел
            }

            if (line == "/list")
            {
                string names;
                lock (lockObj)
                {
                    names = string.Join(", ", users.Keys);
                }
                await writer.WriteLineAsync("Онлайн: " + names);
            }
            else if (line == "/w" || line.StartsWith("/w "))
            {
                string[] parts = line.Split(' ', 3, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length < 3)
                {
                    await writer.WriteLineAsync("Формат: /w ник текст");
                }
                else
                {
                    string target = parts[1];
                    string text = parts[2].Trim();

                    StreamWriter? targetWriter;
                    lock (lockObj)
                    {
                        users.TryGetValue(target, out targetWriter);
                    }

                    if (targetWriter == null)
                    {
                        await writer.WriteLineAsync($"Пользователь {target} не найден.");
                    }
                    else
                    {
                        string time = DateTime.Now.ToString("HH:mm:ss");
                        try
                        {
                            await targetWriter.WriteLineAsync($"[{time}] (лично от {nick}): {text}");
                            await writer.WriteLineAsync($"[{time}] (лично для {target}): {text}");
                            Log($"{nick} -> {target} (лично): {text}");
                        }
                        catch (IOException)
                        {
                            await writer.WriteLineAsync($"Не удалось доставить сообщение {target}.");
                        }
                        catch (ObjectDisposedException)
                        {
                            await writer.WriteLineAsync($"Не удалось доставить сообщение {target}.");
                        }
                    }
                }
            }
            else if (line.StartsWith("/"))
            {
                await writer.WriteLineAsync("Неизвестная команда. Доступно: /list, /w ник текст, /exit");
            }
            else
            {
                Log($"{nick}: {line}");
                await BroadcastAsync($"[{DateTime.Now:HH:mm:ss}] {nick}: {line}");
            }
        }
    }
    catch (IOException)
    {
        // клиент оборвал соединение
    }
    finally
    {
        if (nick != "")
        {
            lock (lockObj)
            {
                users.Remove(nick);
            }
        }
        client.Close();
        if (nick != "")
        {
            Log($"{nick} отключился");
            await BroadcastAsync($"* {nick} покинул чат *");
        }
    }
}


async Task BroadcastAsync(string message)
{
    List<StreamWriter> snapshot;
    lock (lockObj)
    {
        snapshot = users.Values.ToList();
    }

    foreach (var w in snapshot)
    {
        try
        {
            await w.WriteLineAsync(message);
        }
        catch (IOException)
        {
            // клиент отвалился
        }
        catch (ObjectDisposedException)
        {
            // соединение закрыто
        }

    }
}


void Log(string text)
{
    string line = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {text}";
    Console.WriteLine(line);

    lock (logLock)
    {
        try
        {
            File.AppendAllText("chat.log", line + Environment.NewLine);
        }
        catch (IOException)
        {
            Console.WriteLine("Не удалось записать в chat.log");
        }
    }
}
