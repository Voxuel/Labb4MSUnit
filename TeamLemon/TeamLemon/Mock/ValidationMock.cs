using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using TeamLemon.Controls;
using TeamLemon.Models;

namespace TeamLemon.Mock
{
    public class ValidationMock
    {
        public ValidationMock()
        {
            if (User.AllUsers.Count == 0)
            {
                User.initUsers();
            }
        }

        public int ValidateAccountNumber(string inputAccNumber)
        {
            var foundID = 0;
            foreach (var item in Account.AllAccounts)
            {
                foreach (Account account in item.Value.Where(x => x.AccountID == inputAccNumber))
                {
                    foundID = item.Key;
                }
            }

            return foundID;
        }

        public bool ValidateAmount(User currentUser, decimal amountToTransfer, int fromAccount)
        {
            if (amountToTransfer > Account.AllAccounts[currentUser.ID][fromAccount].Balance)
            {
                return false;
            }

            return true;
        }

        public bool LoginValidation(string username, string password)
        {
            bool LogIn = false;
            bool UserFound = false;
            var currentUser = new User();
            var currentAdmin = new Admin();

            foreach (var user in User.AllUsers)
            {
                if (user.Name == username && user.Password == password && user.LockedUser == false)
                {
                    UserFound = true;
                    currentUser = user;
                    LogIn = true;
                    user.LogInAttempt = 3;
                    user.LockedUser = false;
                    break;
                }
                else if (user.Name != username ^ user.Password != password)
                {
                    currentUser = user;
                    user.LogInAttempt--;
                    Console.WriteLine("Wrong username or password");
                }
            }

            foreach (var admin in Admin.AllAdmins)
            {
                if (admin.Name == username && admin.Password == password)
                {
                    UserFound = true;
                    currentAdmin = admin;
                    LogIn = true;
                    break;
                }
                else if (admin.Name != username ^ admin.Password != password)
                {
                    Console.WriteLine("Wrong username or password");
                }
            }

            if (currentUser.LogInAttempt <= 0 && currentAdmin.IsAdmin != true && currentUser.Name != null
                && currentAdmin.Name != null)
            {
                Console.WriteLine("The user is locked");
                currentUser.LockedUser = true;
                LogIn = false;
            }

            return LogIn;
        }

        public User CreateNewUser(string username, string password, int userChoice)
        {
            int _id = 0;
            bool pass = false;
            // Get unique username
            do
            {
                if (String.IsNullOrEmpty(CheckUnique(username)))
                {
                    Console.WriteLine("Not unique username");
                    return new User();
                }

                pass = true;
            } while (!pass);


            // Get id 
            _id = 1001 + User.AllUsers.Count;


            // Generates account ID for the new users account using Guid-struct.
            var accID = Guid.NewGuid().ToString();
            var result = accID.Substring(0, 6);

            // Choose culture info aka currency

            CultureInfo sek = new CultureInfo("sv-SE");
            CultureInfo usd = new CultureInfo("en-US");
            CultureInfo culture = sek;

            do
            {
                if (userChoice > 0 && userChoice < 3)
                {
                    if (userChoice == 1)
                    {
                        culture = sek;
                    }

                    if (userChoice == 2)
                    {
                        culture = usd;
                    }

                    break;
                }
                else
                {
                    Console.Write("Wrong input, select 1 or 2");
                    return new User();
                }
            } while (true);

            // Create a new user
            User newUser = new User()
            {
                Name = username,
                Password = password,
                ID = _id,
                IsAdmin = false,
                LogInAttempt = 3,
                LockedUser = false,
                Accounts = new List<Account>()
                {
                    new Account() { AccountName = "Salary", Balance = 0, AccountID = result, Culture = culture }
                },
                SavingsAccounts = new List<Account>()
            };
            Account.AllAccounts.Add(newUser.ID, newUser.Accounts);
            Account.AllSavings.Add(newUser.ID, newUser.SavingsAccounts);
            // Append to AllUsers
            User.AllUsers.Add(newUser);
            return newUser;
        }


        public string CheckUnique(string input)
        {
            // Loop through all users names to see if this username is unique

            var result = User.AllUsers.Where(u =>
                String.Equals(u.Name, input, StringComparison.CurrentCulture));
            var isUnique = !result.Any();
            

            // If the username is unique then _username = the new username
            // If it is not unique then ask for another username
            if (!isUnique)
            {
                Console.WriteLine("Your username is not unique.");
                return "";
            }
            return input;
        }
    }
}