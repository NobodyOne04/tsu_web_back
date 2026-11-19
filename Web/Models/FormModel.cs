using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    public class FormModel
    {
        public FormModel(int first , int second, string operation)
        {
            this.First = first;
            this.Second = second;
            this.Operation = operation;
            this.Result = 0;
        }
        public FormModel() {}
        [Required]
        public int First { get; set; }
        [Required]
        public int Second { get; set; }
        [Required]
        public string Operation { get; set; }

        public float Result { get; set; }

        public static float Calculate(int first, int second , string operation)
        {
            float Result;
            switch (operation)
            {
                case "+":
                    Result = first + second;
                    break;
                case "-":
                    Result = first - second;
                    break;
                case "*":
                    Result = first * second;
                    break;
                case "/":
                    // float division by zero gives Infinity instead of throwing, so check first
                    Result = second == 0 ? float.MaxValue : (float)first / second;
                    break;
                default:
                    Result = 0;
                    break;
            }
            return Result;
        }
    }
}