using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using TeamLemon.Models;
using TeamLemon.Controls;

namespace TeamLemon.Models
{
    public class Admin : Person
    {
        public Admin()
        {
            if (User.AllUsers.Count() == 0)
            {
                User.initUsers();
            }
        }

        public static List<Admin> AllAdmins { get; set; } = new List<Admin>();
        public static decimal usdValue = 0.097m;

        public static decimal ExchangeRate()
        {
            Console.WriteLine(
                $"Select new exchange rate between USD and SEK\nCurrently it's {Admin.usdValue} $ per SEK\n");
            Console.Write("New exchange rate: ");
            if (decimal.TryParse(Console.ReadLine(), out decimal NewExchange))
            {
                Admin.usdValue = NewExchange;
            }
            else
            {
                Console.WriteLine("Not a valid choice of exchange rate");
            }

            return Admin.usdValue;
        }

        public static void initAdmins()
        {
            Admin anas = new Admin()
            {
                Name = "Anas",
                Password = "coolshirt",
                ID = 1005,
                IsAdmin = true,
                LockedUser = false,
                LogInAttempt = 3
            };
            AllAdmins.Add(anas);
        }

        public void CreateNewUser()
        {
            string _username = "";
            string _password = "";
            int userChoice = 0;
            // Get unique username
            do
            {
                Console.WriteLine("Creating new user...");
                Console.Write("Enter a new username : ");
                string input = Console.ReadLine();
                CheckUnique(input);
            } while (String.IsNullOrEmpty(_username));

            // Get the password
            do
            {
                Console.Write("Enter a new password : ");
                string input = Console.ReadLine();

                if (input != null)
                {
                    _password = input;
                }
            } while (String.IsNullOrEmpty(_password));

            CreateUser(_username, _password, userChoice);
        }

        public User CreateUser(string _username, string _password, int userChoice)
        {
            int _id = 0;
            bool pass = false;
            // Get unique username
            do
            {
                if (String.IsNullOrEmpty(CheckUnique(_username)))
                {
                    Console.WriteLine("Not unique username, Start from the beginning");
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
            }
            else
            {
                Console.Write("Wrong input, select 1 or 2");
                return new User();
            }

            // Create a new user
                User newUser = new User()
                {
                    Name = _username,
                    Password = _password,
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
                string username = "";
                // Loop through all users names to see if this username is unique


                var matches = User.AllUsers.Where(u =>
                    String.Equals(u.Name, input, StringComparison.CurrentCulture));
                bool isNotUnique = matches.Any();

                // If the username is unique then _username = the new username
                // If it is not unique then ask for another username
                if (isNotUnique)
                {
                    return "";
                }
                else
                {
                    username = input;
                    return username;
                }
            }
        }
    }