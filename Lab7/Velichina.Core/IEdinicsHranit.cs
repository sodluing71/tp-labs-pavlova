using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velichina.Core
{
    public  interface IEdinicsHranit
    {
        List<SvojaEdinica> Load();
        void Add(SvojaEdinica edinica);

    }
}
