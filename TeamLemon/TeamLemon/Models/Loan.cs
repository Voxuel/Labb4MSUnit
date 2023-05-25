using System;
using System.Collections.Generic;
using System.Text;

namespace TeamLemon.Models
{
    public class Loan
    {
        public static Dictionary<int, Loan> AllLoans { get; set; } = new Dictionary<int, Loan>();
        public decimal Amount { get; set; }
        public User User { get; set; }

        public override string ToString()
        {
            return User.ToString() + $"\nCurrent loan amount: {Amount}";
        }
    }
}
