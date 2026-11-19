using System;

namespace Web.Models
{
    public class ValueModel
    {
        public readonly int randValueFirst, randValueSecond;

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

        // float division by zero gives Infinity instead of throwing, so check first
        public float Divide() =>
            randValueSecond == 0 ? float.MaxValue : (float)randValueFirst / randValueSecond;

    }
}

