using System;

namespace Lab4.Core
{
    public class Jurnal : Publication
    {
        private int numberVipusk;

        public int NumberVipusk
        {
            get { return numberVipusk; }
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException(nameof(value), "Номер выпуска должен быть больше 0");
                numberVipusk = value;
            }
        }

        public Jurnal(string title, string author, int year, int numberVipusk)
            : base(title, author, year)
        {
            NumberVipusk = numberVipusk;
        }

        public override string GetInfo()
        {
            return $"Журнал: {base.ToString()}, выпуск №{NumberVipusk}";
        }
    }
}
