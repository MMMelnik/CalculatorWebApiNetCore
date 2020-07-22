using System.ComponentModel.DataAnnotations;

namespace Calculator.Models
{
    public enum Operations
    {
        Add,
        Sub,
        Mul,
        Div
    }

    public class Calculation
    {
        public int Id { get; set; }

        [Required]
        public double FirstValue { get; set; }

        [Required]
        public double SecondValue { get; set; }

        [Required]
        public Operations Operation { get; set;}

        public double Result { get; set; }
    }
}
