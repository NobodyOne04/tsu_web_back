using Web.Services;
using Web.Models;
using System.Collections.Generic;

namespace Web.Services
{
    public class OperationService: Services.IService
    {
        private Models.ValueModel model;

        public OperationService()
        {
            this.model = new ValueModel();
        }

        public int Add()
        {
            return this.model.randValueFirst + this.model.randValueSecond;
        }

        public int Sub()
        {
            return this.model.randValueFirst - this.model.randValueSecond;
        }

        public int Mult()
        {
            return this.model.randValueFirst * this.model.randValueSecond;
        }

        public float Div()
        {
            return this.model.Divide();
        }

        public int GetFirst()
        {
            return this.model.randValueFirst;
        }
        
        public int GetSecond()
        {
            return this.model.randValueSecond;
        }
        
    }

}