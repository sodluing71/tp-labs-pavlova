using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Velichina.Core;

namespace Velichina.Tests
{
    public class EdinicConverterTests
    {
        // создаём конвертер с хранилищем в памяти
        private static EdinicConverter CreateConverter()
        {
            return new EdinicConverter(new MemoryEdinicsHranit());
        }


        [Fact]
        public void Convert_KilometersToMeters_5000()
        {
            var converter = CreateConverter();

            double result = converter.Convert("Длина", "км", "м", 5);

            Assert.Equal(5000, result, 1e-9);
        }

        [Fact]
        public void Convert_KilogramsToPounds_GivesKnownValue()
        {
            var converter = CreateConverter();

            double result = converter.Convert("Масса", "кг", "фунт", 1);

            Assert.Equal(2.2046226218, result, 1e-9);
        }
        
        [Fact]
        public void Convert_KmhToMs_10()
        {
            var converter = CreateConverter();

            double result = converter.Convert("Скорость", "км/ч", "м/с", 36);

            Assert.Equal(10, result, 1e-9);
        }


        [Fact]
        public void Convert_CelsiusToFahrenheit_212()
        {
            var converter = CreateConverter();

            double result = converter.Convert(EdinicConverter.TemperatureCategory,
                EdinicConverter.Celsius, EdinicConverter.Fahrenheit, 100);

            Assert.Equal(212, result, 1e-9);
        }

        //неизвестная единица
        [Fact]
        public void Convert_UnknownEdinic_Throws()
        {
            var converter = CreateConverter();

            Assert.Throws<ArgumentException>(() =>
                converter.Convert("Длина", "парсек", "м", 1));
        }

        //добавление единиц
        [Fact]
        public void AddEdinic_NewEdinic_UsedInConversion()
        {
            var converter = CreateConverter();

            converter.AddEdinic("Длина", "верста", 1066.8);
            double result = converter.Convert("Длина", "верста", "км", 1);

            Assert.Equal(1.0668, result, 1e-9);
        }

        [Fact]
        public void AddEdinic_DuplicateName_Throws()
        {
            var converter = CreateConverter();

            Assert.Throws<ArgumentException>(() =>
                converter.AddEdinic("Длина", "КМ", 5));
        }

        [Fact]
        public void AddEdinic_ForTemperature_Throws()
        {
            var converter = CreateConverter();

            Assert.Throws<ArgumentException>(() =>
                converter.AddEdinic(EdinicConverter.TemperatureCategory, "Реомюр", 1.25));
        }

        // хранилище

        [Fact]
        public void AddEdinic_SavesUnitToStorage()
        {
            var storage = new MemoryEdinicsHranit();
            var converter = new EdinicConverter(storage);

            converter.AddEdinic("Масса", "пуд", 16.38);

            Assert.Single(storage.Load());
        }


    }
}
