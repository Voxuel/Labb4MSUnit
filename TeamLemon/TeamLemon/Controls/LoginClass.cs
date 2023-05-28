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
    public class LoginClass
    {
        public LoginClass()
        {
            if (User.AllUsers.Count() == 0)
            {
                User.initUsers();
            }
        }

        public static void Login()
        {
            LoginClass validat = new LoginClass();
            MenuClass menus = new MenuClass();
            Console.Clear();
            Console.WriteLine("\nWelcome to Lemon Bank\n");
            Console.Write("Username: ");
            var username = Console.ReadLine();
            Console.Write("\nPassword: ");
            var password = Console.ReadLine();
            validat.LoginValidation(username, password, out User currentU, out Admin currentA);
            if (currentA.IsAdmin == true)
            {
                menus.AdminMenu(currentA);
            }
            else
            {
                menus.UserMenu(currentU);
            }

        }

        /// <summary>
        /// Method to check if the user exists
        /// </summary>
        /// <param name="allUsers"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>Returns either the current user if it exists, else returns null</returns>
        public bool LoginValidation(string username, string password, out User currentU, out Admin currentA)
        {
            bool LogIn = false;
            bool UserFound = false;
            var currentUser = new User();
            var currentAdmin = new Admin();
            var menus = new MenuClass();
            currentU = new User();
            currentA = new Admin();
            foreach (var user in User.AllUsers)
            {
                if (user.Name == username && user.Password == password && user.LockedUser == false)
                {
                    UserFound = true;
                    currentUser = user;
                    LogIn = true;
                    user.LogInAttempt = 3;
                    user.LockedUser = false;
                    currentU = currentUser;
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
                    currentA = currentAdmin;
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
                return false;
            }

            else if (!LogIn)
            {
                if (currentAdmin.IsAdmin)
                {
                }

                Console.WriteLine("Error, Wrong input or there is no user with that name");
                return false;
            }
            
            return true;
        }
    }
}

