using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velichina.Core
{   // умеет хранить коэффициенты для разных единиц (длина, масса, скорость, температура), конвертировать их и добавлять новые
    public class EdinicConverter
    {
        public const string TemperatureCategory = "Температура";
        public const string Celsius = "°C";
        public const string Fahrenheit = "°F";


        private readonly IEdinicsHranit hranit; // "ключ" к файлу/памяти

        // словарь словарей <ключ: категория, значение: словарь <ключ: наименование, значение: коэфф>>
        private readonly Dictionary<string, Dictionary<string, double>> znachs;

        // маленький словарь-подсказка с <ключ: для катеории, значение: базовый принятый нами кэофф>
        private readonly Dictionary<string, string> baseZnachs;


        public EdinicConverter(IEdinicsHranit storage)
        {
            hranit = storage;
            // инициализация словарей с заполнением базовых значений
            znachs = new Dictionary<string, Dictionary<string, double>>(StringComparer.OrdinalIgnoreCase);
            baseZnachs = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            znachs["Длина"] = new Dictionary<string, double>(StringComparer.OrdinalIgnoreCase)
        {
            { "мм", 0.001 },
            { "см", 0.01 },
            { "м", 1 },
            { "км", 1000 },
            { "дюйм", 0.0254 },
            { "фут", 0.3048 },
        };
            baseZnachs["Длина"] = "м";

            znachs["Масса"] = new Dictionary<string, double>(StringComparer.OrdinalIgnoreCase)
        {
            { "мг", 0.000001 },
            { "г", 0.001 },
            { "кг", 1 },
            { "т", 1000 },
            { "фунт", 0.45359237 },
        };
            baseZnachs["Масса"] = "кг";

            znachs["Скорость"] = new Dictionary<string, double>(StringComparer.OrdinalIgnoreCase)
        {
            { "м/с", 1 },
            { "км/ч", 1000.0 / 3600.0 },
        };
            baseZnachs["Скорость"] = "м/с";

            // подгружаем ранее сохранённые свои единицы
            foreach (SvojaEdinica edinica in hranit.Load())
            {
                try
                {
                    RegisterEdinic(edinica.Category, edinica.Name, edinica.ZnachKoeff);
                }
                catch (ArgumentException)
                {
                    // испорченную запись пропускаем
                }
            }
        }

        // добавляем в список категорию температуру
        public List<string> GetCategories()
        {
            var result = new List<string>(znachs.Keys);
            result.Add(TemperatureCategory);
            return result;
        }
        // подгружаем меры C и F
        public List<string> GetNames(string category)
        {
            if (IsTemperature(category))
            {
                return new List<string> {Celsius, Fahrenheit};
            }

            return new List<string>(GetTable(category).Keys);
        }

        public Dictionary<string, double> GetZnachs(string category)
        {
            return new Dictionary<string, double>(GetTable(category));
        }

        public string GetBaseEdinic(string category)
        {
            GetTable(category);   // проверяем, что категория подходит
            return baseZnachs[category];
        }


        public double Convert(string category, string fromEdinic, string toEdinic, double value)
        {
            if (IsTemperature(category))
            {
                return ConvertTemperature(fromEdinic, toEdinic, value);
            }

            Dictionary<string, double> table = GetTable(category);
            //проверка что единицы есть в таблице
            if (!table.ContainsKey(fromEdinic))
            {
                throw new ArgumentException("Неизвестная единица: " + fromEdinic);
            }
            if (!table.ContainsKey(toEdinic))
            {
                throw new ArgumentException("Неизвестная единица: " + toEdinic);
            }

            return value * table[fromEdinic] / table[toEdinic];
        }
        //добавить свою
        public void AddEdinic(string category, string name, double znach)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Название единицы не может быть пустым.");
            }

            string cleanName = name.Trim();
            RegisterEdinic (category, cleanName, znach);
            hranit.Add(new SvojaEdinica(category, cleanName, znach));
        }

        private void RegisterEdinic (string category, string name, double znach)
        {
            Dictionary<string, double> table = GetTable(category);
            if (string.IsNullOrWhiteSpace(name) || name.Contains(';'))
            {
                throw new ArgumentException("Название единицы пустое или содержит символ ';'.");
            }
            if (znach <= 0 || double.IsNaN(znach) || double.IsInfinity(znach))
            {
                throw new ArgumentException("Коэффициент должен быть положительным числом.");
            }
            if (table.ContainsKey(name))
            {
                throw new ArgumentException("Единица уже существует: " + name);
            }

            table[name] = znach;

        }
        // что не температура
        private Dictionary<string, double> GetTable(string category)
        {
            if (IsTemperature(category))
            {
                throw new ArgumentException("Для температуры используются формулы, коэффициентов нет.");
            }
            if (!znachs.ContainsKey(category))
            {
                throw new ArgumentException("Неизвестная категория: " + category);
            }

            return znachs[category];
        }
        // что температура
        private static bool IsTemperature(string category)
        {
            return string.Equals(category, TemperatureCategory, StringComparison.OrdinalIgnoreCase);
        }

        private static double ConvertTemperature(string fromEdinic, string toEdinic, double value)
        {
            // переводим в Цельсия
            double celsius;
            if (fromEdinic == Celsius)
            {
                celsius = value;
            }
            else if (fromEdinic == Fahrenheit)
            {
                celsius = (value - 32) * 5 / 9;
            }
            else
            {
                throw new ArgumentException("Неизвестная единица температуры: " + fromEdinic);
            }

            if (celsius < -273.15 - 1e-9)
            {
                throw new ArgumentException("Температура ниже абсолютного нуля");
            }

            // переводим из Цельсия в нужную
            if (toEdinic == Celsius)
            {
                return celsius;
            }
            if (toEdinic == Fahrenheit)
            {
                return celsius * 9 / 5 + 32;
            }

            throw new ArgumentException("Неизвестная единица температуры: " + toEdinic);
        }


    }
}
