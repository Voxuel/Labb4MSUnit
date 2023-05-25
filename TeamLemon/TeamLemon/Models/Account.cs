using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace TeamLemon.Models
{
    public class Account
    {
        public string AccountName { get; set; }
        public decimal Balance { get; set; }
        public string AccountID { get; set; }
        public decimal interest = 1.2m;
        public CultureInfo Culture { get; set; }

        public static Dictionary<int, List<Account>> AllAccounts { get; set; } = new Dictionary<int, List<Account>>();
        public static Dictionary<int, List<Account>> AllSavings { get; set; } = new Dictionary<int, List<Account>>();

        
        public override string ToString()
        {
            CultureInfo sek = new CultureInfo("sv-SE");
            sek = (CultureInfo)sek.Clone();
            return AccountName + " " + Balance.ToString("C",Culture);
        }
    }
}
