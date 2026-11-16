using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velichina.Core
{ //«виртуальное хранилище»: пока программа работает, все добавленные единицы лежат в этом списке в оперативной памяти
    public class MemoryEdinicsHranit : IEdinicsHranit
    {
        private List<SvojaEdinica> edinics = new List<SvojaEdinica>(); //создаем список, в котором будут храниться наши единицы

        public List<SvojaEdinica> Load()
        {
            return new List<SvojaEdinica>(edinics);
        }

        public void Add(SvojaEdinica edinica)
        {
            edinics.Add(edinica);
        }

    }
}
