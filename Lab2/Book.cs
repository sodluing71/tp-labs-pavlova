using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2
{
    public class Book : Publication
    {
        private string djanor;

        public string Djanor
        {
            get { return djanor; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Жанр не может быть пустым");
                djanor = value;
            }
        }
        public Book(string title, string author, int year, string djanor) : base(title, author, year)
        {
            Djanor = djanor;
        }
        public override string GetInfo()
        {
            return $"Книга: {base.ToString()}, жанр: {Djanor}";
        }

        public override string ToString()
        {
            return $"Книга \"{Title}\" ({Djanor})";
        }
    }
}
