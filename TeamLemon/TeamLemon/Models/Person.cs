using System;
using System.Collections.Generic;
using System.Text;
using TeamLemon.Models;

namespace TeamLemon
{
    public class Person
    {
        private string _name;
        private string _password;
        private bool _admin;
        private int _id;
        private int _logInAttempt;
        private bool _lockedUser;
        private List<Account> _accounts;
        private List<Account> _savingsAccounts;
        public string Name { get => _name; set { _name = value; } }
        public string Password { get => _password; set { _password = value; } }
        public bool IsAdmin { get => _admin; set { _admin = value; } }
        public int ID { get => _id; set { _id = value; } }
        public int LogInAttempt { get => _logInAttempt; set { _logInAttempt = value; } }
        public bool LockedUser { get => _lockedUser; set { _lockedUser = value; } }
        public List<Account> Accounts { get => _accounts;set { _accounts = value; } }
        public List<Account> SavingsAccounts { get => _savingsAccounts; set { _savingsAccounts = value; } }
    }
}
