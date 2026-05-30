using System;
using BankSystem.Services;

namespace BankSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            var bankService = new BankService();
            string? currentUsername = null;

            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Banking System ===");

                if (currentUsername == null)
                {
                    Console.WriteLine("1. Register\n2. Login\n0. Exit");
                    Console.Write("Choice: ");
                    var choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            HandleRegister(bankService);
                            break;
                        case "2":
                            currentUsername = HandleLogin(bankService);
                            break;
                        case "0":
                            return;
                    }
                }
                else
                {
                    DisplayUserInfo(bankService, currentUsername);
                    Console.WriteLine("1. Transfer\n2. Deposit\n3. Withdraw\n4. Logout");
                    Console.Write("Choice: ");
                    var choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            HandleTransfer(bankService, currentUsername);
                            break;
                        case "2":
                            HandleDeposit(bankService, currentUsername);
                            break;
                        case "3":
                            HandleWithdraw(bankService, currentUsername);
                            break;
                        case "4":
                            currentUsername = null;
                            break;
                    }
                }

                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
        }

        static void HandleRegister(BankService service)
        {
            Console.Write("Username: ");
            string user = Console.ReadLine();
            Console.Write("Password: ");
            string pass = Console.ReadLine();

            var result = service.Register(user, pass);
            Console.WriteLine(result.Message);
        }

        static string? HandleLogin(BankService service)
        {
            Console.Write("Username: ");
            string user = Console.ReadLine();
            Console.Write("Password: ");
            string pass = Console.ReadLine();

            var result = service.Login(user, pass);
            Console.WriteLine(result.Message);

            if (result.IsSuccess && result.Data != null)
            {
                return result.Data.Username;
            }
            return null;
        }

        static void DisplayUserInfo(BankService service, string username)
        {
            string id = service.GetId(username);
            decimal balance = service.GetBalance(username);
            Console.WriteLine($"\nUser: {username} | ID: {id} | Balance: {balance}");
        }

        static void HandleTransfer(BankService service, string username)
        {
            Console.Write("Recipient ID: ");
            string targetId = Console.ReadLine();
            Console.Write("Amount: ");

            if (decimal.TryParse(Console.ReadLine(), out decimal amount))
            {
                var result = service.Transfer(username, targetId, amount);
                Console.WriteLine(result.Message);
            }
            else
            {
                Console.WriteLine("Invalid amount.");
            }
        }

        static void HandleDeposit(BankService service, string username)
        {
            Console.Write("Amount: ");
            if (decimal.TryParse(Console.ReadLine(), out decimal amount))
            {
                var result = service.Deposit(username, amount);
                Console.WriteLine(result.Message);
            }
            else
            {
                Console.WriteLine("Invalid amount.");
            }
        }

        static void HandleWithdraw(BankService service, string username)
        {
            Console.Write("Amount: ");
            if (decimal.TryParse(Console.ReadLine(), out decimal amount))
            {
                var result = service.Withdraw(username, amount);
                Console.WriteLine(result.Message);
            }
            else
            {
                Console.WriteLine("Invalid amount.");
            }
        }
    }
}