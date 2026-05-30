using System;
using BankSystem.Core;
using BankSystem.Data;

namespace BankSystem.Services
{
    public class BankService
    {

        public OperationResult Register(string username, string password, decimal startBalance = 0)
        {
            if (AccountRepository.Instance.Exists(username))
                return OperationResult.Failure("User with this username already exists.");

            try
            {
                var newAccount = new Account(username, password, startBalance);
                AccountRepository.Instance.Add(newAccount);
                return OperationResult.Success($"Account created. Your ID: {newAccount.Id}");
            }
            catch (Exception ex)
            {
                return OperationResult.Failure(ex.Message);
            }
        }

        public OperationResult<Account?> Login(string username, string password)
        {
            var account = AccountRepository.Instance.FindByUsername(username);

            if (account == null || !account.VerifyPassword(password))
                return OperationResult<Account?>.Failure("Invalid username or password.");

            return OperationResult<Account?>.Success(account);
        }

        public OperationResult Transfer(string senderUsername, string recipientId, decimal amount)
        {
            if (amount <= 0)
                return OperationResult.Failure("Amount must be positive.");

            var sender = AccountRepository.Instance.FindByUsername(senderUsername);
            var recipient = AccountRepository.Instance.FindById(recipientId);

            if (sender == null)
                return OperationResult.Failure("Sender account not found.");

            if (recipient == null)
                return OperationResult.Failure("Recipient account not found.");

            if (sender.Id == recipient.Id)
                return OperationResult.Failure("Cannot transfer to yourself.");

            if (sender.TryWithdraw(amount))
            {
                recipient.Deposit(amount);
                AccountRepository.Instance.SaveChanges();
                return OperationResult.Success($"Transfer of {amount} completed successfully.");
            }
            else
            {
                return OperationResult.Failure("Insufficient funds.");
            }
        }

        public OperationResult Deposit(string username, decimal amount)
        {
            var account = AccountRepository.Instance.FindByUsername(username);
            if (account == null)
                return OperationResult.Failure("Account not found.");

            try
            {
                account.Deposit(amount);
                AccountRepository.Instance.SaveChanges();
                return OperationResult.Success($"Deposited {amount}. New balance: {account.Balance}");
            }
            catch (Exception ex)
            {
                return OperationResult.Failure(ex.Message);
            }
        }

        public OperationResult Withdraw(string username, decimal amount)
        {
            var account = AccountRepository.Instance.FindByUsername(username);
            if (account == null)
                return OperationResult.Failure("Account not found.");

            if (account.TryWithdraw(amount))
            {
                AccountRepository.Instance.SaveChanges();
                return OperationResult.Success($"Withdrew {amount}. New balance: {account.Balance}");
            }
            else
            {
                return OperationResult.Failure("Insufficient funds or invalid amount.");
            }
        }

        public decimal GetBalance(string username)
        {
            var acc = AccountRepository.Instance.FindByUsername(username);
            return acc?.Balance ?? 0;
        }

        public string? GetId(string username)
        {
            var acc = AccountRepository.Instance.FindByUsername(username);
            return acc?.Id;
        }
    }

    public class OperationResult
    {
        public bool IsSuccess { get; }
        public string Message { get; }

        protected OperationResult(bool isSuccess, string message)
        {
            IsSuccess = isSuccess;
            Message = message;
        }

        public static OperationResult Success(string message) => new OperationResult(true, message);
        public static OperationResult Failure(string message) => new OperationResult(false, message);
    }

    public class OperationResult<T> : OperationResult
    {
        public T Data { get; }

        private OperationResult(bool isSuccess, string message, T data)
            : base(isSuccess, message)
        {
            Data = data;
        }

        public static OperationResult<T> Success(T data) => new OperationResult<T>(true, "OK", data);
        public new static OperationResult<T> Failure(string message) => new OperationResult<T>(false, message, default!);
    }
}