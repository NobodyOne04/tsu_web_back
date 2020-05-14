using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    public class Operations
    {
        [Required]
        [StringLength(5, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 1)]
        public string First { get; set; }
        [Required]
        [StringLength(5, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 1)]
        public string Second { get; set; }
        public string Operand { get; set; }
        public string CorrectAnswer { get; private set; }
        public string YourAnswer { get; set; }

        public Operations()
        {
            Random random = new Random();
            double First = random.Next(11);
            double Second = random.Next(11);
            string[] Operands = { "+", "-", "*", "/" };
            this.Operand = Operands[random.Next(4)];
            this.First = "" + First;
            this.Second = "" + Second;
        }
        public void Solution()
        {
            StringToOperands(Operand, Convert.ToDouble(this.First), Convert.ToDouble(this.Second));
        }

        private void  StringToOperands(string oper, double First, double Second)
        {
            switch (oper)
            {
                case "+":
                    CorrectAnswer = "" + (First + Second);
                    break;
                case "-":
                    CorrectAnswer = "" + (First - Second);
                    break;
                case "*":
                    CorrectAnswer = "" + (First * Second);
                    break;
                case "/":
                    CorrectAnswer = "" + (First / Second);
                    break;
            }
        }
        public bool Check()
        {
            double AnswerDouble = Math.Abs(Convert.ToDouble(YourAnswer) - Convert.ToDouble(CorrectAnswer));
            if (Operand == "/" & AnswerDouble < 0.1)
            {
                return true;
            }
            if (YourAnswer == CorrectAnswer)
                return true;
            return false;
        }
    }

    public class CorrectAnswers
    {
        private CorrectAnswers()
        {
            Number = 0;
            Correct = 0;
            Total = 0;
            Answers = new List<Operations>();
        }
        
        public List<Operations> Answers;
        public int Correct { get; set; }
        public int Total { get; set; }
        public int Number { get; set; }

        public static CorrectAnswers Instance { get; } = new CorrectAnswers();

    }
}
