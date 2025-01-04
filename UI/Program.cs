using DataPassingBLL;
using System;
using System.Data;

namespace UI
{
    class Program
    {
        static void Main(string[] args)
        {
            DataBLL bll = new DataBLL();
            DataSet ds = bll.GetDataSet();
            while (true)
            {
                Console.Clear();
                Console.WriteLine("********************************");
                Console.WriteLine("Welcome to the Login Page!");
                Console.WriteLine("********************************");
                Console.WriteLine("1. Login");
                Console.WriteLine("2. Register");
                Console.WriteLine("3. Exit");
                Console.Write("Enter your choice: ");
                string choice = Console.ReadLine();
                if (choice == "1")
                {
                    if (LoginForm.Login(ds))
                    {
                        HomePage.Menu(ds,bll);
                    }
                }
                else if (choice == "2")
                {
                    LoginForm.Register(ds, bll);
                }
                else if (choice == "3")
                {
                    Console.WriteLine("\nThank You for visiting our store..!");
                    Console.ReadKey();
                    return;
                }
                else
                {
                    Console.WriteLine($"\nPlease Enter a Valid Choice!! Press any Key To Try Again.");
                    Console.ReadKey();
                    continue;
                }
            }
        }
    }
}
