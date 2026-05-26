using System;
using System.Collections.Generic;
using System.Text;

namespace BankUh
{
    public class BankLogin
    {
        public void RegisterUser(AccountSystem db)
        {
            Console.Write("\nПридумайте логин: ");
            string user = Console.ReadLine();

            // Простая проверка на пустоту
            if (string.IsNullOrWhiteSpace(user))
            {
                Console.WriteLine("Логин не может быть пустым!");
                Console.ReadKey();
                return;
            }

            Console.Write("Придумайте пароль: ");
            string pass = Console.ReadLine();

            if (db.Register(user, pass, 0))
            {
                Console.WriteLine("\nУспешная регистрация! Теперь войдите.");
            }
            Console.WriteLine("Нажмите любую клавишу...");
            Console.ReadKey();
        }

        public string? LoginUser(AccountSystem db)
        {
            Console.Write("\nЛогин: ");
            string user = Console.ReadLine();
            Console.Write("Пароль: ");
            string pass = Console.ReadLine();

            var account = db.Login(user, pass);
            if (account != null)
            {
                Console.WriteLine($"\nВход выполнен.");
                Console.ReadKey();
                return user;
            }
            else
            {
                Console.WriteLine("\nЛОГИН или ПАРОЛЬ не ВЕРНЫЕ!!!");
                Console.ReadKey();
                return null;
            }
        }

        public void MakeTransfer(AccountSystem db, string senderUsername)
        {
            Console.Write("\nВведите ID счета получателя: ");
            string targetId = Console.ReadLine();

            Console.Write("Сумма перевода: ");
            if (decimal.TryParse(Console.ReadLine(), out decimal amount))
            {
                if (amount <= 0)
                {
                    Console.WriteLine("Сумма должна быть положительной!");
                }
                else
                {
                    db.Transfer(senderUsername, targetId, amount);
                }
            }
            else
            {
                Console.WriteLine("Некорректная сумма.");
            }

            Console.WriteLine("Нажмите любую клавишу...");
            Console.ReadKey();
        }

        public void MakeAddKromer(AccountSystem db, string username)
        {

            Console.Write("Сумма пополения: ");
            if (decimal.TryParse(Console.ReadLine(), out decimal amount))
            {
                if (amount <= 0)
                {
                    Console.WriteLine("Сумма должна быть положительной!");
                }
                else
                {
                    db.AddKromer(username, amount);
                }
            }
            else
            {
                Console.WriteLine("Некорректная сумма.");
            }

            Console.WriteLine("Нажмите любую клавишу...");
            Console.ReadKey();
        }
        public void MakeNoKromer(AccountSystem db, string username)
        {

            Console.Write("Сколько кромеров вы хотите вывести: ");
            if (decimal.TryParse(Console.ReadLine(), out decimal amount))
            {
                if (amount <= 0)
                {
                    Console.WriteLine("Сумма должна быть положительной!");
                }
                else
                {
                    db.NoKromer(username, amount);
                }
            }
            else
            {
                Console.WriteLine("Некорректная сумма.");
            }

            Console.WriteLine("Нажмите любую клавишу...");
            Console.ReadKey();
        }
    }
}
