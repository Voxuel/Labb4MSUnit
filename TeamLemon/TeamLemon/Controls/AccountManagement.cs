using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TeamLemon.Models;

namespace TeamLemon.Controls
{
    public class AccountManagement
    {

        public static void CreateNewAcc(User currentUser)
        {
            Console.Clear();
            Console.WriteLine("Welcome to creating a new account");
            if (MenuClass.GotoMenu("to create a new account") != 1)
            {
                var go = new MenuClass();
                go.UserMenu(currentUser);
            }
            Console.WriteLine("You are creating a new account\nWhat type of account would you like to make?\n1: Normal account\n2: Savings account");
            int accType;

            do
            {
                Int32.TryParse(Console.ReadLine(), out accType);
                if (accType <= 0 || accType > 2)
                {
                    Console.WriteLine("Invalid input, Please enter a number between 1-2!");
                }

            } while (accType > 2 || accType <= 0);


            Console.Write("What would you like to name your account?: ");
            string accName = "";
            do
            {
                accName = Console.ReadLine();
                if (String.IsNullOrEmpty(accName)) Console.WriteLine("Please enter a name!");
            } while (String.IsNullOrEmpty(accName));

            // Generates a new unique ID for the specified account using Guid-struct.
            var id = Guid.NewGuid().ToString();
            var result = id.Substring(0, 6);

            // Choose culture info aka currency
            Console.WriteLine("What currency would you like to use on this account?");
            Console.WriteLine("1. SEK");
            Console.WriteLine("2. USD");


            CultureInfo sek = new CultureInfo("sv-SE");
            CultureInfo usd = new CultureInfo("en-US");
            CultureInfo culture = sek;

            do
            {
                if (int.TryParse(Console.ReadLine(), out int userChoice) && userChoice > 0 && userChoice < 3)
                {
                    if (userChoice == 1) { culture = sek; }
                    else if (userChoice == 2) { culture = usd; }

                    break;
                }
                else Console.WriteLine("Please enter a number between 1-2!");
            }
            while (true);

            if (accType == 1)
            {

                Account tempAcc = new Account() { AccountName = accName, Balance = 0, AccountID = result, Culture = culture };

                foreach (var item in Account.AllAccounts)
                {
                    if (item.Key == currentUser.ID)
                    {
                        item.Value.Add(tempAcc);
                        break;
                    }
                }
            }

            else if (accType == 2)
            {
                Account tempAcc = new Account() { AccountName = accName, Balance = 0, AccountID = result, Culture = culture };

                foreach (var item in Account.AllSavings)
                {
                    if (item.Key == currentUser.ID)
                    {
                        item.Value.Add(tempAcc);
                        break;
                    }
                }
            }

            ChangelogManagement.AppendToChangelog("Created a new account '" + accName + "' with id '" + result + "'", currentUser.Changelog);

        }
        public static void InternalChoice(User currentUser)
        {
            if (MenuClass.GotoMenu("to internal transfer") != 1)
            {
                var go = new MenuClass();
                go.UserMenu(currentUser);
            }
            bool loop = true;
            while (loop)
            {
                Console.Clear();
                Console.WriteLine("Make internal transfers or deposit to savings account\n1: Internal Transfers\n2: Savings Deposit\n3: Return to menu");
                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    switch (choice)
                    {
                        case 1:
                            AccountManagement.InternalTransfer(currentUser);
                            break;
                        case 2:
                            AccountManagement.SavingsDeposit(currentUser);
                            break;
                        case 3:
                            loop = false;
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Not a valid choice ");
                }
            }
        }




        public static void SavingsDeposit(User currentUser)
        {
            List<Account> accounts = new List<Account>();
            Account.AllSavings.TryGetValue(currentUser.ID, out accounts);

            if (accounts.Count != 0)
            {
                Console.Clear();
                Console.WriteLine("Make deposit to your savings account");
                MonitorAccounts(currentUser);

                int fromAccount = 0;
                int toAccount = 0;
                decimal amountToTransfer = 0;
                bool IsTransfer = true;
                while (IsTransfer)
                {
                    Console.WriteLine("Choose account to transfer from");
                    int.TryParse(Console.ReadLine(), out fromAccount);

                    if (ValidateFromAccount(currentUser, fromAccount, toAccount))
                    {
                        IsTransfer = false;
                    }
                }
                fromAccount--;
                while (!IsTransfer)
                {
                    Console.WriteLine("Choose account to deposit your savings to");
                    int.TryParse(Console.ReadLine(), out toAccount);
                    toAccount--;
                    if (ValidateToAccount(currentUser, toAccount, fromAccount))
                    {
                        IsTransfer = true;
                    }
                }
                while (IsTransfer)
                {
                    Console.Write("Choose amount to deposit: ");
                    decimal.TryParse(Console.ReadLine(), out amountToTransfer);
                    if (ValidateAmount(currentUser, amountToTransfer, fromAccount))
                    {

                        IsTransfer = false;
                    }
                }

                Account.AllAccounts[currentUser.ID][fromAccount].Balance -= amountToTransfer;

                // Enchange rate on "en-US" aka American Dollar
                if (Account.AllSavings[currentUser.ID][toAccount].Culture.Name == "en-US")
                {
                    amountToTransfer = amountToTransfer * Admin.usdValue;
                }

                if (Account.AllAccounts[currentUser.ID][fromAccount].Culture.Name == "en-US")
                {
                    amountToTransfer = amountToTransfer / Admin.usdValue;
                }

                Account.AllSavings[currentUser.ID][toAccount].Balance += amountToTransfer;


            }
            else
            {
                Console.WriteLine("You have no saving accounts ");
            }


        }

        public static void MonitorAccounts(User currentUser)
        {
            Account.AllAccounts.TryGetValue(currentUser.ID, out List<Account> currentAccount);
            int i = 1;
            foreach (Account account in currentAccount)
            {
                Console.WriteLine(i + ": " + account.ToString());
                i++;
            }
            List<Account> accounts = new List<Account>();
            Account.AllSavings.TryGetValue(currentUser.ID, out accounts);

            i = 1;
            if (accounts.Count != 0)
            {
                Console.WriteLine("Savings Account(s)");
                foreach (Account item in accounts)
                {
                    Console.WriteLine(i + ": " + item.ToString());
                    i++;
                }

            }

        }

        public static void InternalTransfer(User currentUser)
        {
            Console.Clear();
            Console.WriteLine("Internal Transfer");
            MonitorAccounts(currentUser);

            int fromAccount = 0;
            int toAccount = 0;
            decimal amountToTransfer = 0;
            bool IsTransfer = true;

            while (IsTransfer)
            {
                Console.WriteLine("Choose account to transfer from.");
                int.TryParse(Console.ReadLine(), out fromAccount);

                if (ValidateFromAccount(currentUser, fromAccount, toAccount))
                {
                    IsTransfer = false;
                }
            }
            fromAccount--;
            while (!IsTransfer)
            {
                Console.WriteLine("Choose account to transfer to.");
                int.TryParse(Console.ReadLine(), out toAccount);
                toAccount--;
                if (ValidateToAccount(currentUser, toAccount, fromAccount))
                {
                    IsTransfer = true;
                }
            }
            while (IsTransfer)
            {
                Console.WriteLine("How much do you want to transfer?");

                decimal.TryParse(Console.ReadLine(), out amountToTransfer);
                if (Account.AllAccounts[currentUser.ID][fromAccount].Balance < amountToTransfer)
                {
                    Console.WriteLine("The amount is to high or to low for what exists on the account");
                }
                else if (amountToTransfer <= -1)
                {
                    Console.WriteLine("Please enter a sum greater than 0");
                }
                else
                {
                    IsTransfer = false;
                }
            }

            Account.AllAccounts[currentUser.ID][fromAccount].Balance -= amountToTransfer;

            // Enchange rate on "en-US" aka American Dollar
            if (Account.AllAccounts[currentUser.ID][toAccount].Culture.Name == "en-US")
            {
                amountToTransfer = amountToTransfer * Admin.usdValue;
            }

            if (Account.AllAccounts[currentUser.ID][fromAccount].Culture.Name == "en-US")
            {
                amountToTransfer = amountToTransfer / Admin.usdValue;
            }

            Account.AllAccounts[currentUser.ID][toAccount].Balance += amountToTransfer;
            ChangelogManagement.AppendToChangelog("Transfered " + amountToTransfer + " to account '"
                + Account.AllAccounts[currentUser.ID][toAccount].AccountID
                + "' from account '" + Account.AllAccounts[currentUser.ID][fromAccount].AccountID + "'", currentUser.Changelog);
        }

        public static void ExternalTransfer(User currentUser)
        {
            if (MenuClass.GotoMenu("to external transfer") != 1)
            {
                var go = new MenuClass();
                go.UserMenu(currentUser);
            }
            int fromAccount = 0;
            int toAccount = 10;
            decimal amountToTransfer = 0;
            var foundAccount = new Account();

            var isTransfering = true;
            while (isTransfering)
            {
                Console.Clear();
                MonitorAccounts(currentUser);
                Console.WriteLine("External transfer");
                Console.WriteLine("From what account do you wish to transfer from?");
                int.TryParse(Console.ReadLine(), out fromAccount);

                if (!ValidateFromAccount(currentUser, fromAccount, toAccount))
                {
                    continue;
                }
                fromAccount--;
                Console.WriteLine("Enter the account number below you wish to transfer the money to");
                var inputAccNumber = Console.ReadLine();
                var ID = ValidateAccountNumber(inputAccNumber);
                if (ID == 0)
                {
                    Console.WriteLine("There is no account with that number" +
                        ", Press enter to try again");
                    Console.ReadKey();
                    continue;
                }
                Console.WriteLine("Enter the amount to transfer to the account");
                if (decimal.TryParse(Console.ReadLine(), out amountToTransfer) && amountToTransfer >= 0
                    && ValidateAmount(currentUser, amountToTransfer, fromAccount))
                {
                    Console.Write("\nPassword:");
                    var password = Console.ReadLine();
                    if (!ValidatePassword(currentUser, password))
                    {
                        Console.WriteLine("Wrong password, Press enter to try again");
                        Console.ReadKey();
                        continue;
                    }
                    MakeExternalTransfer(currentUser, ID, amountToTransfer, fromAccount, inputAccNumber);
                    ChangelogManagement.AppendToChangelog("Transfered " + amountToTransfer + " to account '" + inputAccNumber 
                        + "' from account '" + Account.AllAccounts[currentUser.ID][fromAccount].AccountID + "'", currentUser.Changelog);
                    isTransfering = false;
                }
                else
                {
                    Console.WriteLine("Invalid amount to transfer" +
                        ", Press enter to continue");
                    Console.ReadKey();
                }

            }

            MenuClass.ContinueToMenu();
        }


        /// <summary>
        /// Checks if the account from exists.
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="fromAccount"></param>
        /// <param name="toAcccount"></param>
        /// <returns>if the account exists returns true</returns>
        private static bool ValidateFromAccount(User currentUser, int fromAccount, int toAcccount)
        {
            if (fromAccount <= Account.AllAccounts[currentUser.ID].Count && fromAccount != toAcccount &&
                fromAccount >= 0)
            {
                return true;
            }
            else
            {
                Console.WriteLine("Wrong account input, select between the accounts you have" +
                    ", Press enter to try again");
                Console.ReadKey();
                return false;
            }
        }
        /// <summary>
        /// Checks if the account to exists
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="fromAccount"></param>
        /// <param name="toAccount"></param>
        /// <returns>if the account exists returns true</returns>
        public static bool ValidateToAccount(User currentUser, int toAccount, int? fromAccount = null)
        {
            if (toAccount <= Account.AllAccounts[currentUser.ID].Count - 1 && toAccount != fromAccount &&
                toAccount >= 0)
            {
                return true;
            }
            else if (toAccount <= Account.AllSavings[currentUser.ID].Count - 1 && toAccount != fromAccount)
            {
                return true;
            }
            else
            {
                Console.WriteLine("Wrong account input, select between the accounts you have" +
                    ", Press enter to try again");
                Console.ReadKey();
                return false;
            }
        }
        /// <summary>
        /// checks if the account with the specified account number exists
        /// </summary>
        /// <param name="inputAccNumber"></param>
        /// <returns>If the account exists returns the accountnumber</returns>
        private static int ValidateAccountNumber(string inputAccNumber)
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
        /// <summary>
        /// Checks if there is enough money on the account which you want to transfer from.
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="amountToTransfer"></param>
        /// <param name="fromAccount"></param>
        /// <returns>If the amount is valid returns true</returns>
        private static bool ValidateAmount(User currentUser, decimal amountToTransfer, int fromAccount)
        {
            if (amountToTransfer > Account.AllAccounts[currentUser.ID][fromAccount].Balance)
            {
                Console.WriteLine("The amount is greater than what exists on the selected account" +
                    ", Press enter to try again");
                Console.ReadKey();
                return false;
            }
            return true;
        }
        /// <summary>
        /// Validates the password before transfer to external account.
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="password"></param>
        /// <returns>Returns true if the password matches the current user</returns>
        public static bool ValidatePassword(User currentUser, string password)
        {
            var result = currentUser.Password == password ? true : false;
            return result;
        }


        /// <summary>
        /// Makes the transfer after all checks have been passed
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="toAccountKey"></param>
        /// <param name="amount"></param>
        /// <param name="fromAccount"></param>
        /// <param name="inputAccNumber"></param>
        private static void MakeExternalTransfer(User currentUser, int toAccountKey, decimal amount
            , int fromAccount, string inputAccNumber)
        {

            Account.AllAccounts[currentUser.ID][fromAccount].Balance -= amount;

            // Enchange rate on "en-US" aka American Dollar
            if (Account.AllAccounts[currentUser.ID][fromAccount].Culture.Name == "sv-SE" &&
                Account.AllAccounts[toAccountKey].Where(x => x.AccountID == inputAccNumber).Any(x => x.Culture.Name == "en-US"))
            {
                amount = amount * Admin.usdValue;
            }
            if (Account.AllAccounts[currentUser.ID][fromAccount].Culture.Name == "en-US" &&
                Account.AllAccounts[toAccountKey].Where(x => x.AccountID == inputAccNumber).Any(x => x.Culture.Name == "sv-SE"))
            {
                amount = amount / Admin.usdValue;
            }


            Account.AllAccounts[toAccountKey].Where(x => x.AccountID == inputAccNumber).ToList()
                .ForEach(y => y.Balance += amount);
        }
    }
}
