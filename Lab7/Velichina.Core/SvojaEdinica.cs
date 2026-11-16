using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velichina.Core
{
    public class SvojaEdinica
    {
        public string Category { get; }
        public string Name { get; }
        public double ZnachKoeff { get; }

        public SvojaEdinica(string category, string name, double znach)
        {
            Category = category;
            Name = name;
            ZnachKoeff = znach;
        }

    }
}
