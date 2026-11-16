using System;
using System.Globalization;
using System.Text;
using ConsoleTables;
using Velichina.Core;

Console.OutputEncoding = Encoding.UTF8;
Console.InputEncoding = Encoding.UTF8;

// файл со своими единицами лежит рядом с программой
string hranitPath = Path.Combine(AppContext.BaseDirectory, "svoi_edinics.txt");

// хранилище "вставляем" в конвертер через конструктор
var converter = new EdinicConverter(new FileEdinicsHranit(hranitPath));

Console.WriteLine("Конвертер величин");
Console.WriteLine("Свои единицы хранятся в файле: " + hranitPath);

while (true)
{
    Console.WriteLine();
    Console.WriteLine("1 - Конвертировать");
    Console.WriteLine("2 - Таблица коэффициентов (кроме температуры)");
    Console.WriteLine("3 - Добавить свою единицу");
    Console.WriteLine("0 - Выход");
    Console.Write("Ваш выбор: ");

    switch (Console.ReadLine())
    {
        case "1":
            ConvertMenu();
            break;
        case "2":
            TableMenu();
            break;
        case "3":
            AddUnitMenu();
            break;
        case "0":
            return;
        default:
            Console.WriteLine("Нет такого пункта меню");
            break;
    }
}

// конвертация
void ConvertMenu()
{
    string category = ChooseFromList("Выберите величину:", converter.GetCategories());
    if (category == "")
    {
        return;
    }

    List<string> edinics = converter.GetNames(category);

    string fromEdinic = ChooseFromList("Из какой единицы:", edinics);
    if (fromEdinic == "")
    {
        return;
    }

    string toEdinic = ChooseFromList("В какую единицу:", edinics);
    if (toEdinic == "")
    {
        return;
    }

    double value;
    if (!TryReadNumber("Значение: ", out value))
    {
        return;
    }

    try
    {
        double result = converter.Convert(category, fromEdinic, toEdinic, value);
        Console.WriteLine($"{value} {fromEdinic} = {result.ToString("G10")} {toEdinic}");
    }
    catch (ArgumentException ex)
    {
        Console.WriteLine("Ошибка: " + ex.Message);
    }
}

// таблица коэффициентов. возвращает список категорий,
// исключая температуру (потому что для температуры нет коэффициентов, там формулы)
void TableMenu()
{
    string category = ChooseFromList("Таблица какой величины:", GetLinearCategories());
    if (category == "")
    {
        return;
    }

    Console.WriteLine($"Базовая единица: {converter.GetBaseEdinic(category)} " +
                      "В колонке справа - сколько базовых единиц в одной единице");

    var table = new ConsoleTable("Единица", "Коэффициент");
    foreach (var pair in converter.GetZnachs(category))
    {
        table.AddRow(pair.Key, pair.Value.ToString("0.########"));
    }
    table.Write();
}

// добавление своей единицы
void AddUnitMenu()
{
    string category = ChooseFromList("В какую величину добавить единицу:", GetLinearCategories());
    if (category == "")
    {
        return;
    }

    Console.Write("Название новой единицы (например, верста): ");
    string? name = Console.ReadLine();
    if (name == null || name.Trim() == "")
    {
        Console.WriteLine("Отмена.");
        return;
    }

    Console.WriteLine($"Сколько ({converter.GetBaseEdinic(category)}) в одной единице '{name.Trim()}'?");
    double znach;
    if (!TryReadNumber("Коэффициент: ", out znach))
    {
        Console.WriteLine("Отмена.");
        return;
    }

    try
    {
        converter.AddEdinic(category, name, znach);
        Console.WriteLine("Единица добавлена и сохранена.");
    }
    catch (ArgumentException ex)
    {
        Console.WriteLine("Ошибка: " + ex.Message);
    }
}

// вспомогательные методы

// категории, у которых есть таблица коэффициентов (без температуры)
//берем все категории и добавляем в список только те, которые не равны "Температуре".
List<string> GetLinearCategories()
{
    var result = new List<string>();
    foreach (string c in converter.GetCategories())
    {
        if (c != EdinicConverter.TemperatureCategory)
        {
            result.Add(c);
        }
    }
    return result;
}

// показывает пронумерованный список, возвращает выбранный элемент ("" = отмена)
// пример: 1 - Длина, 2 - Масса, 0 - Отмена
string ChooseFromList(string title, List<string> items)
{
    while (true)
    {
        Console.WriteLine(title);
        for (int i = 0; i < items.Count; i++)
        {
            Console.WriteLine($"  {i + 1} - {items[i]}");
        }
        Console.WriteLine("  0 - Отмена");
        Console.Write("Номер: ");

        string? text = Console.ReadLine();
        if (text == null)
        {
            return "";
        }

        int number;
        if (int.TryParse(text, out number))
        {
            if (number == 0)
            {
                return "";
            }
            if (number >= 1 && number <= items.Count)
            {
                return items[number - 1];
            }
        }

        Console.WriteLine("Введите номер из списка.");
    }
}

// читает число (можно и с точкой, и с запятой); false = пользователь отменил
bool TryReadNumber(string prompt, out double number)
{
    while (true)
    {
        Console.Write(prompt);
        string? text = Console.ReadLine();

        if (text == null || text.Trim() == "")
        {
            number = 0;
            return false;
        }

        if (double.TryParse(text.Trim(), out number))
        {
            return true;
        }

        Console.WriteLine("Ошибка! Это не число. Введите числовое значение (например: 12,5)");
    }
}