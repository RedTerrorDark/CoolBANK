using System;

namespace BankSystem.Core
{
    public class Account
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public decimal Balance { get; set; }

        public Account()
        {
        }
        public Account(string username, string password, decimal initialBalance)
        {
            if (string.IsNullOrWhiteSpace(username)) throw new ArgumentException("Login cannot be null");

            Id = Guid.NewGuid().ToString("N")[..8];
            Username = username;
            Password = password;
            Balance = initialBalance;
        }
        public void Deposit(decimal amount)
        {
            if (amount <= 0) throw new InvalidOperationException("Deposit amount must be positive.");
            Balance += amount;
        }

        public bool TryWithdraw(decimal amount)
        {
            if (amount <= 0) return false;
            if (Balance < amount) return false;

            Balance -= amount;
            return true;
        }
        public bool VerifyPassword(string password)
        {
            return Password == password;
        }
    }
}