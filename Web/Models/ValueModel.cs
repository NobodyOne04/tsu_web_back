using System;
using System.Collections.Generic;

namespace Web.Models
{
    public class ValueModel
    {
        public readonly int randValueFirst, randValueSecond;
        
        private Dictionary<string, string> operation;
        public ValueModel()
        {
            Random rnd = new Random();
            randValueFirst = rnd.Next(0, 10);
            randValueSecond = rnd.Next(0, 10);
        }

        public float Divide()
        {
            float div;
            try
            {
                div = this.randValueFirst / this.randValueSecond;
            }
            catch (DivideByZeroException e)
            {
                div = float.MaxValue;
            }
            return div;   
        }

    }
}

