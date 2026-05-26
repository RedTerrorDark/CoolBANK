using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.IO;

namespace BankUh
{
    public class Account
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public decimal Balance { get; set; }

        public Account(string username, string password, decimal balance)
        {
            Username = username;
            Password = password;
            Balance = balance;
            Id = Guid.NewGuid().ToString("N")[..8];
        }
    }


    public sealed class AccountSystem
    {
        private static readonly Lazy<AccountSystem> _instance =
            new(() => new AccountSystem());

        public static AccountSystem Instance => _instance.Value;

        public List<Account> Accounts { get; private set; }
        private readonly string _filePath = "accounts.json";

        private AccountSystem()
        {
            Accounts = LoadAccounts();
        }

        private List<Account> LoadAccounts()
        {
            if (!File.Exists(_filePath)) return new List<Account>();

            try
            {
                var json = File.ReadAllText(_filePath);
                return JsonSerializer.Deserialize<List<Account>>(json) ?? new List<Account>();
            }
            catch
            {
                return new List<Account>();
            }
        }

        public void Save()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            File.WriteAllText(_filePath, JsonSerializer.Serialize(Accounts, options));
        }

        public bool Register(string username, string password, decimal startBalance = 0)
        {
            if (Accounts.Any(a => a.Username == username))
            {
                Console.WriteLine("Такой ЛОГИН уже есть.");
                return false;
            }

            var newAccount = new Account(username, password, startBalance);

            Accounts.Add(newAccount);
            Save();

            Console.WriteLine($"Аккаунт создан!\n\n Ваш ID счета: {newAccount.Id}");

            return true;
        }

        public Account? Login(string username, string password)
        {
            return Accounts.FirstOrDefault(a => a.Username == username && a.Password == password);
        }

        public bool Transfer(string fromUsername, string toAccountId, decimal amount)
        {
            var from = Accounts.FirstOrDefault(a => a.Username == fromUsername);

            var to = Accounts.FirstOrDefault(a => a.Id == toAccountId);

            if (from == null)
            {
                Console.WriteLine("ТЫ КАК СЮДА ПОПАЛ НЕ ВОЙДЯ В АКК???");
                return false;
            }

            if (to == null)
            {
                Console.WriteLine("Нету такого ID");
                return false;
            }

            if (from.Id == to.Id)
            {
                Console.WriteLine("Ты реально деньги сам себе переводишь????");
                return false;
            }

            if (from.Balance < amount)
            {
                Console.WriteLine("Кромеров не хватает.");
                return false;
            }

            from.Balance -= amount;
            to.Balance += amount;
            Save();

            Console.WriteLine($"Перевод {amount} кромерс. на счет {toAccountId} выполнен успешно!");
            return true;
        }
        public bool AddKromer(string username, decimal amount)
        {
            var who = Accounts.FirstOrDefault(a => a.Username == username);

            if (who == null)
            {
                Console.WriteLine("ТЫ КАК СЮДА ПОПАЛ НЕ ВОЙДЯ В АКК???");
                return false;
            }

            who.Balance += amount;
            Save();

            Console.WriteLine($"Счет успешно пополнен на {amount} кромеров!");
            return true;
        }

        public bool NoKromer(string username, decimal amount)
        {
            var who = Accounts.FirstOrDefault(a => a.Username == username);

            if (who == null)
            {
                Console.WriteLine("ТЫ КАК СЮДА ПОПАЛ НЕ ВОЙДЯ В АКК???");
                return false;
            }

            who.Balance -= amount;
            Save();

            Console.WriteLine($"Успешно выведенно {amount} кромеров!");
            return true;
        }
    }
}
