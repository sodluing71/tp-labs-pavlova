using System.Diagnostics;
using System.Text;

const int N = 30_000;

var swTotal = Stopwatch.StartNew();

// --- 1. Генерация данных ---
var rnd = new Random(42);                       // фиксированное зерно: результат воспроизводим
var buyers = new List<string>();
var amounts = new List<int>();
for (int i = 0; i < N; i++)
{
    buyers.Add("user" + rnd.Next(0, 5000));
    amounts.Add(rnd.Next(1, 10_000));
}


// --- 2. Список уникальных покупателей ---
var sw = Stopwatch.StartNew();
var unique = new HashSet<string>();     // УЗКОЕ МЕСТО №2 (Поиск внутри списка на коллекцию)
for (int i = 0; i < buyers.Count; i++)
{           
        unique.Add(buyers[i]);
}
Console.WriteLine($"Уникальных покупателей: {unique.Count}  ({sw.ElapsedMilliseconds} мс)");


// --- 3. Текстовый отчёт ---
sw.Restart();
var str = new StringBuilder();
for (int i = 0; i < buyers.Count; i++)
{
    str.Append(buyers[i] + ";" + amounts[i] + Environment.NewLine);  // УЗКОЕ МЕСТО №1 (Конкатенация строк +=)
}
string report = str.ToString();
Console.WriteLine($"Отчёт: {report.Length} символов  ({sw.ElapsedMilliseconds} мс)");


// --- 4. Сортировка сумм ---
sw.Restart();
var sorted = new List<int>(amounts);
sorted.Sort();      // УЗКОЕ МЕСТО №3 (самодельная сортировка "пузырек" взамен готовой)
Console.WriteLine($"Медианная сумма: {sorted[sorted.Count / 2]}  ({sw.ElapsedMilliseconds} мс)");


// --- 5. Сумма покупок каждого покупателя ---
sw.Restart();
var totals = new Dictionary<string, long>();
for (int i =0; i < buyers.Count; i++)
{
    long current;           // УЗКОЕ МЕСТО №4 (1 прогон без линего цикличного перебора)
    totals.TryGetValue(buyers[i], out current);
    totals[buyers[i]] = current + amounts[i];

}
var best = totals.MaxBy(t => t.Value);
Console.WriteLine($"Максимум потратил: {best.Key}  ({sw.ElapsedMilliseconds} мс)");

Console.WriteLine($"ИТОГО: {swTotal.ElapsedMilliseconds} мс");
