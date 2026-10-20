using System.Numerics;
using System;

namespace Lab4.Core;

public static class MathAlgorithms
{
    public static long Factorial(int n)
    {
        if (n < 0) throw new ArgumentOutOfRangeException(nameof(n));
        if (n > 20) throw new ArgumentOutOfRangeException(nameof(n), "переполнение long");

        long result = 1;
        for (int i = 2; i <= n; i++) 
            result *= i;
        return result;
    }

    public static int[] Metod_Fibonachi(int N)
    {
        if (N < 1) throw new ArgumentOutOfRangeException(nameof(N));
        long a = 0;
        long b = 1;
        int[] m = new int[N+1];
        for (int i = 0; i <= N; i++)
        {
            m[i] = (int)a;
            long c = a + b;
            a = b;
            b = c;

        }
        return m;
    }

    public static double Metod_Teilora(double x)
    {
        if (x <= -1) throw new ArgumentOutOfRangeException(nameof(x), "значение x выходит за пределы вычислений");
        if (x > 1) throw new ArgumentOutOfRangeException(nameof(x), "значение x выходит за пределы вычислений");

        double eps = 1e-6;
        double sum = 0.0;
        int n = 1;

        while (true)
        {
            // (-1)^(n+1) * x^n / n
            double result = Math.Pow(x, n) / n;
            if (n % 2 == 0)
            {
                result = -result;
            }
            if (Math.Abs(result) < eps)
            {
                break;
            }
            sum += result;
            n++;

        }
        return sum;
    }

    public static double Function(double X)
    {
        if (X < 54) throw new ArgumentOutOfRangeException(nameof(X), " под коренным выражением должно быть неотрицательным!");

        double result = Math.Sqrt(X - 54) + Math.Cos(X / 2) / Math.Sin(X * X) - Math.Log(X);
        return result;
    }
}
