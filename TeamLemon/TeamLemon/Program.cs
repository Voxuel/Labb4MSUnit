using System;
using TeamLemon.Models;
using TeamLemon.Controls;

namespace TeamLemon
{
    class Program
    {
        static void Main(string[] args)
        {

            ASCIIArt.IntroArt();

            User.initUsers();
            Admin.initAdmins();
            ChangelogManagement.Init();
            ChangelogManagement.InitAllUserChangelogs();

            LoginClass.LoginValidation(User.AllUsers,Admin.AllAdmins);

            System.Threading.Tasks.Task task = ChangelogManagement.WriteChangelogAsync();

        }
    }
}
