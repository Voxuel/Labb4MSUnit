using System;
using System.Collections.Generic;
using System.Text;
using TeamLemon.Models;
using System.Linq;

namespace TeamLemon.Controls
{
    public class LoanMangement
    {
        /// <summary>
        /// Handles taking loan by user from the bank.
        /// </summary>
        /// <param name="currentUser"></param>
        public static void TakeALoan(User currentUser)
        {
            LoanMangement manager = new LoanMangement();
            var takingLoan = true;
            var amountToLoan = 0.0m;
            Console.Clear();
            if (MenuClass.GotoMenu("take a loan") != 1)
            {
                var go = new MenuClass();
                go.UserMenu(currentUser);
            }
            while (takingLoan)
            {
                Console.Clear();
                Console.WriteLine("Take a loan");
                var found = manager.UserLoanValidation(currentUser);
                if (found != null)
                {
                    Console.WriteLine("You already have a loan at the bank" +
                        $", You current loan is at: {found.Amount}");
                }
                else if(currentUser.AmountLeftToLoan == 0)
                {
                    Console.WriteLine("You already have reached your loan celling");
                    takingLoan = false;
                }
                var resultRounded = Math.Round(manager.CalculateLoanCelling(currentUser),2,MidpointRounding.ToEven);
                Console.WriteLine("Enter the amount you wish to loan" +
                    $"\nYour current loancelling is at: {resultRounded}");
                decimal.TryParse(Console.ReadLine(), out amountToLoan);
                if (!manager.ValidateLoanAmount(currentUser, amountToLoan))
                {
                    continue;
                }
                Console.WriteLine("To what account to do wish the money be transfered to?\n");
                AccountManagement.MonitorAccounts(currentUser);
                int.TryParse(Console.ReadLine(), out int result);
                result--;
                if (!AccountManagement.ValidateToAccount(currentUser,result))
                {
                    continue;
                }
                Console.WriteLine("Please enter your password to sign your loan: ");
                var password = Console.ReadLine();
                if (!AccountManagement.ValidatePassword(currentUser, password))
                {
                    Console.WriteLine("Wrong password, Press enter to try again");
                    Console.ReadKey();
                    continue;
                }
                var amountToLoanWithInterest = manager.CalculateInterest(amountToLoan);
                manager.MakeLoanTransfer(currentUser,result,amountToLoan);
                Console.Clear();
                Console.WriteLine($"The transfer was successful, You new account balance is now:");
                AccountManagement.MonitorAccounts(currentUser);
                Console.WriteLine($"Your current loan at 10% interest is at {amountToLoanWithInterest}");

                takingLoan = false;
            }
        }
        /// <summary>
        /// Checks if the current user already have a loan in the bank.
        /// </summary>
        /// <param name="currentUser"></param>
        /// <returns>The loan which is conected to the user in question</returns>
        private Loan UserLoanValidation(User currentUser)
        {
            var user = Loan.AllLoans.Values.FirstOrDefault(x => x.User == currentUser);
            if(user != null)
            {
                return user;
            }
            return null;
        }
        /// <summary>
        /// Calculates the loan celling for the user in question.
        /// </summary>
        /// <param name="currentUser"></param>
        /// <returns>Decimal of the current users loancelling</returns>
        public decimal CalculateLoanCelling(User currentUser)
        {
            Account.AllAccounts.TryGetValue(currentUser.ID, out List<Account> accounts);
            decimal loanCelling = 0;
            Loan.AllLoans.TryGetValue(currentUser.ID, out Loan currentLoan);
            if(currentLoan != null)
            {
                return currentUser.AmountLeftToLoan;
            }
            foreach (Account account in accounts)
            {
                if(account.Culture.Name == "en-US")
                {
                    loanCelling += account.Balance / Admin.usdValue;
                }
                else
                {
                    loanCelling += account.Balance;
                }            
            }
            loanCelling *= 5;
            return loanCelling;
        }
        /// <summary>
        /// Checks if the amount is valid to loan
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="amount"></param>
        /// <returns>True if the current user does not precede the loan celling</returns>
        private bool ValidateLoanAmount(User currentUser, decimal amount)
        {
            if(currentUser.AmountLeftToLoan < amount)
            {
                Console.WriteLine("You can't take a loan over your loan celling" +
                    ", Press enter to try again");
                Console.ReadKey();
                return false;
            }
            else if(amount <= 0)
            {
                Console.WriteLine("Wrong input, can't be less than or equal to 0" +
                    ", Press enter to try again");
                Console.ReadKey();
                return false;
            }
            return true;
        }
        /// <summary>
        /// Makes transfer from the bank to the current user
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="accountIndex"></param>
        /// <param name="amountToLoan"></param>
        private void MakeLoanTransfer(User currentUser,int accountIndex ,decimal amountToLoan)
        {
            if(Account.AllAccounts[currentUser.ID][accountIndex].Culture.Name == "en-US")
            {
                amountToLoan *= Admin.usdValue;
            }

            Account.AllAccounts[currentUser.ID][accountIndex].Balance += amountToLoan;
            if (Loan.AllLoans.ContainsKey(currentUser.ID))
            {
                Loan.AllLoans[currentUser.ID].Amount += amountToLoan;
            }
            else
            {
                Loan.AllLoans.Add(currentUser.ID, new Loan { User = currentUser, Amount = amountToLoan });
            }
            User.AllUsers[User.AllUsers.IndexOf(currentUser)].AmountLeftToLoan -= amountToLoan;
            ChangelogManagement.AppendToChangelog("Took a loan of '" + amountToLoan + "' to account '" 
                + Account.AllAccounts[currentUser.ID][accountIndex].AccountID + "'", currentUser.Changelog);
        }
        /// <summary>
        /// Adds interest to the amount the user wishes to loan
        /// </summary>
        /// <param name="amount"></param>
        /// <returns>Decimal of the new value when interest is added</returns>
        private decimal CalculateInterest(decimal amount)
        {
            decimal interest = 1.10m;

            amount *= interest;

            return amount;
        }
    }
}
