using System.Net.Sockets;

Console.Write("Адрес сервера (Enter = localhost): ");
string host = Console.ReadLine() is { Length: > 0 } h ? h : "localhost";

int port = 5555;
Console.Write("Порт сервера: ");
string portText = Console.ReadLine() ?? "";
if (portText != "")
{
    int parsed;
    if (int.TryParse(portText, out parsed))
    {
        port = parsed;
    }
    else
    {
        Console.WriteLine("Неверный порт, используется 5555.");
    }
}

using var client = new TcpClient();
try
{
    await client.ConnectAsync(host, port);
}
catch (SocketException)
{
    Console.WriteLine($"Не удалось подключиться к {host}:{port}. Сервер недоступен.");
    return;
}

var stream = client.GetStream();
var reader = new StreamReader(stream);
var writer = new StreamWriter(stream) { AutoFlush = true };

bool connectionLost = false;

try
{
    bool entered = false; // выбор ника
    while (!entered)
    {
        Console.Write("Ваш ник (без пробелов): ");
        string nick = Console.ReadLine() ?? "";
        await writer.WriteLineAsync(nick);

        string? answer = await reader.ReadLineAsync();
        while (answer != null && answer != "OK" && answer != "BUSY" && answer != "BAD")
        {
            Console.WriteLine(answer);
            answer = await reader.ReadLineAsync();
        }

        if (answer == null)
        {
            Console.WriteLine("Соединение с сервером потеряно.");
            return;
        }

        if (answer == "OK")
        {
            entered = true;
        }
        else if (answer == "BUSY")
        {
            Console.WriteLine("Этот ник уже занят, введите другой.");
        }
        else
        {
            Console.WriteLine("Ник не должен быть пустым или содержать пробелы.");
        }
    }


    _ = Task.Run(async () =>
    {
        try
        {
            string? line;
            while ((line = await reader.ReadLineAsync()) != null)
                Console.WriteLine(line);
        }
        catch (IOException) { }
        connectionLost = true;
        Console.WriteLine("Соединение с сервером потеряно. Нажмите Enter для выхода.");
    });

    Console.WriteLine("Подключено. Команды: /list, /w ник текст, /exit");

    // отправка сообщений
    while (!connectionLost)
    {
        string? msg = Console.ReadLine();
        if (msg == null || connectionLost)
        {
            break;
        }
        await writer.WriteLineAsync(msg);
        if (msg == "/exit")
        {
            break;
        }
    }
}
catch (IOException)
{
    Console.WriteLine("Соединение с сервером потеряно.");
}