
while (true)
{
    Console.WriteLine("Выберите номер задния:");
    Console.WriteLine("0. Выход");
    Console.WriteLine("1. Факториал числа n");
    Console.WriteLine("2. Последовательность чисел Фибоначчи от 0 до n");
    Console.WriteLine("3. Вычислить значение функции");
    Console.WriteLine("4. Вычислить сумму ряда Тейлора");
    Console.Write("Ваш выбор: ");

    if (!int.TryParse(Console.ReadLine(), out int comand) || comand < 0 || comand > 4)
    {
        Console.WriteLine("Ошибка: нужно целое число от 0 до 4");
        Console.WriteLine();
        continue;
    }

    if (comand == 0)
    {
        break;
    }

    switch(comand)
    {
        case 1: Metod_Factorial(); break;
        case 2: Metod_Fibonachi(); break;
        case 3: Metod_Function(); break;
        case 4: Console.WriteLine("Задание 4 - Metod_Teilora()"); break;
    }
}

// Метод факториала
static void Metod_Factorial()
{
    Console.Write("Введите n (0..20): ");

    if (!int.TryParse(Console.ReadLine(), out int n) || n < 0 || n > 20)
    {
        Console.WriteLine("Ошибка: нужно целое число от 0 до 20.");
        return;
    }

    Console.WriteLine($"{n}! = {Factorial(n)}");
    Console.WriteLine();

}

static long Factorial(int n)
{
    long result = 1;
    for (int i = 2; i <= n; i++)
        result *= i;
    return result;
}

// Метод Фибоначчи
static void Metod_Fibonachi()
{
    Console.Write("Введите n (от 1): ");

    if (!int.TryParse(Console.ReadLine(), out int n) || n < 1)
    {
        Console.WriteLine("Ошибка: нужно целое число от 1");
        Console.WriteLine();
        return;
    }

    Console.WriteLine("Последовательность чисел Фибоначчи от 0 до " + n + ":");
    long a = 0;
    long b = 1;
    for (int i = 0; i <= n; i++)
    {
        Console.Write(a);
        if (i < n) Console.Write(", ");
        long c = a + b;
        a = b;
        b = c;
    }
    Console.WriteLine();
    Console.WriteLine();
}

// Метод Функуции
static void Metod_Function()
{
    Console.Write("Введите x (от 54): ");
    if (!double.TryParse(Console.ReadLine(), out double x))
    {
        Console.WriteLine("Ошибка: нужно ввести число!");
        Console.WriteLine();
        return;
    }
    if (x<54)
    {
        Console.WriteLine("Ошибка в значение х!");
        Console.WriteLine("Под коренным выражением должно быть неотрицательным");
        Console.WriteLine();
        return;
    }
    double result = Math.Sqrt(x - 54) + Math.Cos(x / 2) / Math.Sin(x * x) - Math.Log(x);
    Console.WriteLine($"A = {result}");
    Console.WriteLine();
    Console.WriteLine();
}