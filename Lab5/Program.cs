using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab5;

class Program
{
    static void Main(string[] args)
    {
        string cipherText = "НЦКЁХЦ, ЖЛФЩЛТОЛРГ ТУСЁУГППЛУСЕГРЛЗ ХЗШРСОСЁЛМ! ТЦ ТЦ ТЦ ТЦ";
        string knownWord = "КУЗГТУ, ДИСЦИПЛИНА ПРОГРАММИРОВАНИЕ ТЕХНОЛОГИЙ! ПУ ПУ ПУ ПУ";
        int maxSdvig = 33;

        Console.WriteLine($"Результат шифра: {cipherText}");
        Console.WriteLine($"Зашифрованное слово: {knownWord}\n");

        // Последовательно
        var sw = Stopwatch.StartNew();
        int seqShift = -1;

        for (int i = 1; i <= maxSdvig; i++)
        {
            string poisk = Class.Schifr(cipherText, i);
            if (poisk.Contains(knownWord))
            {
                seqShift = i;
                break;
            }
        }
        sw.Stop();
        long seqTime = sw.ElapsedMilliseconds;
        Console.WriteLine($"Последовательно: сдвиг = {seqShift}; время = {sw.ElapsedMilliseconds} мс");

        // Параллельно
        sw.Restart();
        int parShift = -1;

        Parallel.For(1, maxSdvig + 1, (i, state) =>
        {
            string poisk = Class.Schifr(cipherText, i);
            if (poisk.Contains(knownWord))
            {
                parShift = i;
                state.Stop();
            }
        });
        sw.Stop();
        long parTime = sw.ElapsedMilliseconds;
        Console.WriteLine($"Параллельно: сдвиг = {parShift}; время = {sw.ElapsedMilliseconds} мс");
        double speedup = parTime > 0 ? (double)seqTime / parTime : 0;
        Console.WriteLine($"Ускорение (T_посл / T_пар): {speedup:F2}x");
        Console.ReadLine();
    }
}