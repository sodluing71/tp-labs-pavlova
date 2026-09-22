using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2
{
    public class Library
    {
        private List<Publication> publications;

        public Library()
        {
            publications = new List<Publication>();
        }

        public void AddPublication(Publication publication)
        {
            if (publication == null)
                throw new ArgumentNullException(nameof(publication), "Издание не может быть пустым");
            publications.Add(publication);
        }

        public List<Publication> FindByAuthor(string author)
        {
            if (string.IsNullOrWhiteSpace(author))
                throw new ArgumentException("Автор не может быть пустым");

            List<Publication> result = new List<Publication>();
            foreach (Publication p in publications)
            {
                if (p.Author.Equals(author, StringComparison.OrdinalIgnoreCase))
                {
                    result.Add(p);
                }
            }
            return result;
        }
        public List<Publication> GetAll()
        {
            return publications;
        }
    }
}
