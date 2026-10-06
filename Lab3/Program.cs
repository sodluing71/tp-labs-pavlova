using System;
using System.Text;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        FileCopier f = new FileCopier();

        Console.Write("Введите путь к искомому файлу: ");
        string filePath = Console.ReadLine();
        if(!f.FilePath(filePath))
        {
            Console.WriteLine("Ошибка: файл не найден");
            return;
        }

        Console.Write("Введите путь к искоой папке: ");
        string directoryPath = Console.ReadLine();
        if (!f.DirectoryPath(directoryPath))
        {
            Console.WriteLine("Ошибка: папка не существует");
            return;
        }

        Console.Write("Введите новое имя для файла: ");
        string newName = Console.ReadLine();
        bool overwrite = false;
        if (f.Proverka(directoryPath, newName))
        {
            Console.WriteLine("Внимание: такой файл уже есть в папке!");
            Console.WriteLine("Хотите перезаписать?");
            Console.WriteLine(" - Да (введите цифру 1)");
            Console.WriteLine(" - Нет (введите цифру 2)");
            int otvet;
            if (!int.TryParse(Console.ReadLine(), out otvet))
            {
                Console.WriteLine("Некорректный ввод, операция отменена");
                return;
            }
            else if (otvet == 2)
            {
                Console.WriteLine("Перезапись отменена");
                return;
            }
            else if (otvet == 1)
            {
                overwrite = true;
            }
            else
            {
                Console.WriteLine("Некорректный ввод, операция отменена");
                return;
            }
        }

        try
        {
            f.CopyFile(filePath, directoryPath, newName, overwrite);
            Console.WriteLine("Файл успешно скопирован");
        }
        catch (IOException ex)
        {
            Console.WriteLine($"Ошибка ввода-вывода: {ex.Message}");
        }
        Console.ReadLine();
    }
}