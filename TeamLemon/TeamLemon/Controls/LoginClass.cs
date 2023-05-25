using System;
using System.Collections.Generic;
using System.Text;
using TeamLemon.Models;
using System.Linq;

namespace TeamLemon.Controls
{
    /// <summary>
    /// Class to handle login control and logic.
    /// </summary>
    class LoginClass
    {
        /// <summary>
        /// Method to check if the user exists
        /// </summary>
        /// <param name="allUsers"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>Returns either the current user if it exists, else returns null</returns>
        public static void LoginValidation(List<User> allUsers,List<Admin> allAdmins)
        {
            var menus = new MenuClass();
            bool LogIn = false;
            bool UserFound = false;
            var currentUser = new User();
            var currentAdmin = new Admin();
            do
            {
                Console.Clear();
                Console.WriteLine("\nWelcome to Lemon Bank\n");
                Console.Write("Username: ");
                var username = Console.ReadLine();
                Console.Write("\nPassword: ");
                var password = Console.ReadLine();

                foreach (var user in allUsers)
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
                foreach (var admin in allAdmins)
                {
                    if (admin.Name == username && admin.Password == password)
                    {
                        UserFound = true;
                        currentAdmin = admin;
                        LogIn = true;
                        break;
                    }
                    else if(admin.Name != username ^ admin.Password != password)
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
                    continue;
                }
                else if (!LogIn)
                {
                    if (currentAdmin.IsAdmin)
                    {
                        continue;
                    }
                    Console.WriteLine("Error, Wrong input or there is no user with that name");
                }
            } while (LogIn == false);
            if (UserFound == true)
            {
                if (currentAdmin.IsAdmin == true)
                {
                    menus.AdminMenu(currentAdmin);
                }
                else
                {
                    menus.UserMenu(currentUser);
                }
            }
        
        }
    }
}

