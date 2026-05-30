using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.IO;
using BankSystem.Core;

namespace BankSystem.Data
{
    public sealed class AccountRepository
    {
        private static readonly Lazy<AccountRepository> _instance =
            new Lazy<AccountRepository>(() => new AccountRepository());

        public static AccountRepository Instance => _instance.Value;

        private List<Account> _accounts;
        private readonly string _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "accounts.json");

        private AccountRepository()
        {
            _accounts = LoadFromFile();
        }

        private List<Account> LoadFromFile()
        {
            if (!File.Exists(_filePath))
            {
                return new List<Account>();
            }

            try
            {
                var json = File.ReadAllText(_filePath);

                if (string.IsNullOrWhiteSpace(json))
                    return new List<Account>();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var list = JsonSerializer.Deserialize<List<Account>>(json, options);

                if (list == null)
                {
                    return new List<Account>();
                }

                return list;
            }
            catch (Exception ex)
            {
                return new List<Account>();
            }
        }

        public void SaveChanges()
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                File.WriteAllText(_filePath, JsonSerializer.Serialize(_accounts, options));
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error saving data: {ex.Message}");
            }
        }

        public Account? FindByUsername(string username)
        {
            return _accounts.FirstOrDefault(a =>
                a.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        }

        public Account? FindById(string id)
        {
            return _accounts.FirstOrDefault(a => a.Id == id);
        }

        public bool Exists(string username)
        {
            return _accounts.Any(a =>
                a.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        }

        public void Add(Account account)
        {
            _accounts.Add(account);
            SaveChanges();
        }
    }
}