using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Calculator.Models
{
    /// <summary>
    /// Gets or Sets Operation for Calculation
    /// Add, Sub, Mul, Div, Rem, Sqrt
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Operations
    {
        /// <summary>
        /// Enum Add for "Addition"
        /// </summary>
        [EnumMember(Value = "Add")]
        Add,
        /// <summary>
        /// Enum Sub for "Subtraction"
        /// </summary>
        [EnumMember(Value = "Sub")]
        Sub,
        /// <summary>
        /// Enum Mul for "Multiplication"
        /// </summary>
        [EnumMember(Value = "Mul")]
        Mul,
        /// <summary>
        /// Enum Div for "Division"
        /// </summary>
        [EnumMember(Value = "Div")]
        Div,
        /// <summary>
        /// Enum Rem for "Remainder"
        /// </summary>
        [EnumMember(Value = "Rem")]
        Rem,
        /// <summary>
        /// Enum Sqrt for "Square root"
        /// </summary>
        [EnumMember(Value = "Sqrt")]
        Sqrt
    }

    public class Calculation
    {
        public int Id { get; set; }

        [Required]
        public double FirstValue { get; set; }

        public double SecondValue { get; set; }

        [Required]
        [JsonConverter(typeof(StringEnumConverter))]
        public Operations Operation { get; set;}

        public double Result { get; set; }
    }
}
