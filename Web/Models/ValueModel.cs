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

        public int Add()
        {
            return this.randValueFirst + this.randValueSecond;
        }
        
        public int Sub()
        {
            return this.randValueFirst - this.randValueSecond;
        }
        
        public int Mult()
        {
            return this.randValueFirst * this.randValueSecond;
        }

        public float Divide()
        {
            float div;
            try
            {
                div = (float)this.randValueFirst / this.randValueSecond;
            }
            catch (DivideByZeroException e)
            {
                div = float.MaxValue;
            }
            return div;   
        }

    }
}

