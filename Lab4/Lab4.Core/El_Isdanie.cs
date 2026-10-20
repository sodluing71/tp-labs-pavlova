using System;

namespace Lab4.Core
{
    public class El_Isdanie : Publication
    {
        private string format;

        public string Format
        {
            get { return format; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Формат не может быть пустым");
                format = value;
            }
        }

        public El_Isdanie(string title, string author, int year, string format)
            : base(title, author, year)
        {
            Format = format;
        }

        public override string GetInfo()
        {
            return $"Электронное издание: {base.ToString()}, формат: {Format}";
        }
    }
}
