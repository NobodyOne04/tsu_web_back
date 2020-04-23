using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Models
{
    public class ValueModel
    {
        public readonly int randValueFirst, randValueSecond;
        public ValueModel()
        {
            Random rnd = new Random();
            randValueFirst = rnd.Next(-10, 10);
            randValueSecond = rnd.Next(-10, 10);
        }
    }
}
