using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using TeamLemon.Controls;

namespace TeamLemon.Models
{
    public class User : Person
    {
        public static List<User> AllUsers { get; set; } = new List<User>();
        public List<string> Changelog { get; set; } = new List<string>();
        public decimal AmountLeftToLoan { get; set; }


        public static void initUsers()
        {
            User sebastian = new User()
            {
                Name = "Sebastian",
                Password = "LandFall",
                IsAdmin = false,
                ID = 1001,
                LogInAttempt = 3,
                LockedUser = false,
                Accounts = new List<Account>()
                {

                    new Account { AccountName = "Savings: ", Balance = 1432.34m, AccountID = "100101", Culture = new CultureInfo("sv-SE") },
                    new Account { AccountName = "Salery: ", Balance = 10000.00m, AccountID = "100102", Culture = new CultureInfo("sv-SE") }
                },
                SavingsAccounts = new List<Account>(),
                Changelog = new List<string>()
            };
            User patrik = new User()
            {
                Name = "Patrik",
                Password = "Horror",
                IsAdmin = false,
                ID = 1002,
                LogInAttempt = 3,
                LockedUser = false,
                Accounts = new List<Account>()
                {
                    new Account{AccountName = "Savings: ", Balance = 1337.00m, AccountID = "100201", Culture = new CultureInfo("sv-SE")},
                    new Account{AccountName = "Salery: ", Balance = 420.00m, AccountID = "100202", Culture = new CultureInfo("sv-SE")}
                },
                Changelog = new List<string>(),
                SavingsAccounts = new List<Account>()
            };
            User leo = new User()
            {
                Name = "Leo",
                Password = "MTG",
                IsAdmin = false,
                ID = 1003,
                LogInAttempt = 3,
                LockedUser = false,
                Accounts = new List<Account>()
                {

                    new Account{AccountName = "Savings: ", Balance = 1664.00m, AccountID = "100301", Culture = new CultureInfo("sv-SE")},
                    new Account{AccountName = "Salery: ", Balance = 18887.00m, AccountID = "100302", Culture = new CultureInfo("sv-SE")}
                },
                Changelog = new List<string>(),
                SavingsAccounts = new List<Account>()
            };
            User theo = new User()
            {
                Name = "Theo",
                Password = "CarLover",
                IsAdmin = false,
                ID = 1004,
                LogInAttempt = 3,
                LockedUser = false,
                Accounts = new List<Account>()
                {
                    new Account{AccountName = "Savings: ", Balance = 740.00m, AccountID = "100401", Culture = new CultureInfo("sv-SE")},
                    new Account{AccountName = "Salery: ", Balance = 6400.00m, AccountID = "100402", Culture = new CultureInfo("en-US")}
                },
                Changelog = new List<string>(),
                SavingsAccounts = new List<Account>()
            };
            AllUsers.Add(sebastian);
            AllUsers.Add(patrik);
            AllUsers.Add(leo);
            AllUsers.Add(theo);
            LoanMangement manager = new LoanMangement();
            int i = 0;
            foreach (User user in AllUsers)
            {
                Account.AllAccounts.Add(user.ID, user.Accounts);
                Account.AllSavings.Add(user.ID, user.SavingsAccounts);
            }
            foreach (User user in AllUsers)
            {
                AllUsers[i].AmountLeftToLoan = manager.CalculateLoanCelling(user);
                i++;
            }
        }
    }
}

