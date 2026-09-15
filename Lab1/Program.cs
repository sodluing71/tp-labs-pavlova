
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
        case 3: Console.WriteLine("Задание 3 - Metod_Function()"); break;
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
    Console.ReadLine();
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

    if (!int.TryParse(Console.ReadLine(), out int n) || n < 0)
    {
        Console.WriteLine("Ошибка: нужно целое число от 1");
        return;
    }

    if (n == 0)
    {
        Console.WriteLine("Последовательность чисел Фибоначчи:" + n);
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

