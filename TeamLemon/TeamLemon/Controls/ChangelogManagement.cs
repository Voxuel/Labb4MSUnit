using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TeamLemon.Models;

namespace TeamLemon.Controls
{
    internal class ChangelogManagement
    {
        public static List<string> Lines = new List<string>();

        // Write to admin changelog
        public static async Task WriteChangelogAsync()
        {
            // Get the path to write the changelog
            // Using DateTime to get the currrent Month-Day-Year_Hour-Minute-Second
            // To add in the filename for easier readability
            string path = Path.Combine(@"../../../Changelogs/Admins\log-" + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss") + ".txt");
            await File.WriteAllLinesAsync(path, Lines);

            foreach (User user in User.AllUsers)
            {
                string userPath = Path.Combine(@"../../../Changelogs/" + user.ID + "/Changelog.txt");
                await File.WriteAllLinesAsync(userPath, user.Changelog);
            }
        }

        // Add to personal changelog
        public static void AppendToChangelog(string StringToAppend, List<string> Changelog)
        {
            Changelog.Add(DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss") + " : " + StringToAppend);
            Lines.Add(DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss") + " : " + StringToAppend);
        }

        // Main admin changelog
        public static void AppendToChangelog(string StringToAppend)
        {
            Lines.Add(DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss") + " : " + StringToAppend);
        }

        // Create folder for new users
        public static void CreateFolder(int userID)
        {
            Directory.CreateDirectory(@"../../../Changelogs/" + userID);
            if (!File.Exists(@"../../../Changelogs/" + userID + "/Changelog.txt"))
            {
                File.WriteAllText(@"../../../Changelogs/" + userID + "/Changelog.txt", "Changelog of user: " + userID);
            }
        }

        // Read the changelog for a specific user and write it to console
        public static void ReadCurrentLog(User currentUser)
        {
            //string path = Path.Combine(@"../../../Changelogs/" + userID + "/Changelog.txt");
            //foreach(string line in File.ReadLines(path))
            //{
            //    Console.WriteLine(line);
            //}

            foreach (var item in User.AllUsers[User.AllUsers.IndexOf(currentUser)].Changelog)
            {
                Console.WriteLine(item);
            }
        }

        // Write from Changelog.txt to all users changelog list
        public static void InitAllUserChangelogs()
        {
            foreach (User user in User.AllUsers) {
                string path = Path.Combine(@"../../../Changelogs/" + user.ID + "/Changelog.txt");
                foreach (string line in File.ReadLines(path))
                {
                    user.Changelog.Add(line);
                }
            }
        }

        // Init the ChangelogManagement
        public static void Init()
        {
            foreach (User user in User.AllUsers)
            {
                CreateFolder(user.ID);
            }
        }
    }
}
