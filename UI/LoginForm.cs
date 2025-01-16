using DataPassingBLL;
using System;
using System.Data;
using System.Linq;

namespace UI
{
    class LoginForm
    {
        public static bool Login(DataSet ds)
        {
            Console.Clear();
            Console.WriteLine("********** Login **********");
            Console.Write("Enter Username: ");
            string username = Console.ReadLine();
            Console.Write("Enter Password: ");
            string password = Console.ReadLine();
            bool isAuthenticated = false;
            foreach (DataRow row in ds.Tables["Users"].Rows)
            {
                if (row["Username"].ToString() == username && row["Password"].ToString() == password)
                {
                    Console.Clear();
                    Console.WriteLine($"Login successful! Welcome, {username}.\n");
                    Session.UserName = username;
                    isAuthenticated = true;
                }
            }
            if (!isAuthenticated)
            {
                Console.WriteLine("Invalid username or password. Please try again.");
                return false;
            }
            return true;
        }
        public static void Register(DataSet ds, DataBLL bll)
        {
            Console.Clear();
            Console.WriteLine("********** Register **********");
            Console.Write("Enter Full Name: ");
            string fullName = Console.ReadLine();
            Console.Write("Enter Username: ");
            string username = Console.ReadLine();
            DataRow existingUser = ds.Tables["users"].AsEnumerable()
                                                .FirstOrDefault(row => row.Field<string>("Username") == username);
            if (existingUser != null)
            {
                Console.WriteLine("User already exists. Please try a different username.");
                Console.WriteLine("\nPress any key to return to the Login Page...");
                Console.ReadKey();
                return; 
            }
            //DataRow[] existingUsers = ds.Tables["users"].Select($"Username = '{username}'");

            //if (existingUsers.Length > 0)
            //{
            //    Console.WriteLine("User already exists. Please try a different username.");
            //}
            Console.Write("Enter Password: ");
            string password = Console.ReadLine();
            Console.Write("Enter Mobile Number: ");
            string phone = Console.ReadLine();
            DataRow newUser = ds.Tables["Users"].NewRow();
            newUser["FullName"] = fullName;
            newUser["Username"] = username;
            newUser["Password"] = password;
            newUser["MobileNumber"] = phone;
            ds.Tables["Users"].Rows.Add(newUser);
            bll.RegisterUser(ds);
            Console.WriteLine("Registration successful!");
            Console.WriteLine("\nPress any key to return to the Login Page...");
            Console.ReadKey();
        }
    }
}
