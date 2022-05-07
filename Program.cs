using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.ActiveDirectory;


namespace Compare_GroupMembers
{
    internal class Program
    {
        static List<string> Group1MembersList = new List<string>();
        static List<string> Group2MembersList = new List<string>();
        static string group1name = "Helpdesk Admins";
        static string group2name = "Server Administrators";

        static void Main(string[] args)
        {

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\r\n   ________________________________ ");
            Console.WriteLine("  |  _____________________________  |");
            Console.WriteLine("  | |    Compare-Group Members    | |");
            Console.WriteLine("  | |_____________________________| |");
            Console.WriteLine("  |_________________________________|");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("                     by @ScarredMonk\r\n");
            Console.ForegroundColor = ConsoleColor.Gray;

            try
            {
                Domain.GetCurrentDomain().ToString();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message + "\n\nPlease run it inside the domain joined machine \n\n");
                Console.ForegroundColor = ConsoleColor.Gray;
                return;
            }

            //Saving members of group 1 into the list
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("[+] Members of Group " + group1name);
            Console.ForegroundColor = ConsoleColor.Gray;
            SaveGroup1Members(group1name);

            //Saving members of group 2 into the list
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n[+] Members of Group " + group2name);
            Console.ForegroundColor = ConsoleColor.Gray;
            SaveGroup2Members(group2name);

            //Checking for new machine account addition into the security groups
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n[+] Members of " + group1name + " in " + group2name +" Group");
            Console.ForegroundColor = ConsoleColor.Gray;            
            CompareGroupMembers();
    }

        static void SaveGroup1Members(string groupname)
        {
            PrincipalContext context = new PrincipalContext(ContextType.Domain, Domain.GetCurrentDomain().ToString());
            GroupPrincipal group = GroupPrincipal.FindByIdentity(context, IdentityType.Name, groupname);
            if (group != null)
            {

                foreach (Principal p in group.GetMembers(true))
                {
                    Group1MembersList.Add(p.Name);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(p.Name);
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
                group.Dispose();
            }
        }

        static void SaveGroup2Members(string groupname)
        {
            PrincipalContext context = new PrincipalContext(ContextType.Domain, Domain.GetCurrentDomain().ToString());
            GroupPrincipal group = GroupPrincipal.FindByIdentity(context, IdentityType.Name, groupname);
            if (group != null)
            {

                foreach (Principal p in group.GetMembers(true))
                {
                    Group2MembersList.Add(p.Name);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(p.Name);
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
                group.Dispose();
            }
        }


        static void CompareGroupMembers()
        {
            List<string> duplicates = Group1MembersList.Intersect(Group2MembersList).ToList();
            foreach (string dup in duplicates)
            {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine(dup);
                    Console.ForegroundColor = ConsoleColor.Gray;
            }
        }
    }
}
