using System;

namespace Banking.Core.Domain
{
    public class Account
    {
        public Guid Id { get; private set; }
        public Guid UserId { get; private set; }
        public decimal Balance { get; private set; }
        public AccountType Type { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? LastUpdatedAt { get; private set; }

        private Account() { }

        public static Account Create(Guid userId, AccountType type)
        {
            return new Account
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Balance = 0,
                Type = type,
                CreatedAt = DateTime.UtcNow
            };
        }

        public void Deposit(decimal amount)
        {
            if (amount <= 0)
                throw new InvalidOperationException("Deposit amount must be greater than 0");

            Balance += amount;
            LastUpdatedAt = DateTime.UtcNow;
        }

        public void Withdraw(decimal amount)
        {
            if (amount <= 0)
                throw new InvalidOperationException("Withdrawal amount must be greater than 0");

            if (Balance < amount)
                throw new InvalidOperationException("Insufficient funds");

            Balance -= amount;
            LastUpdatedAt = DateTime.UtcNow;
        }

        public void Transfer(Account destination, decimal amount)
        {
            if (amount <= 0)
                throw new InvalidOperationException("Transfer amount must be greater than 0");

            if (Balance < amount)
                throw new InvalidOperationException("Insufficient funds");

            Withdraw(amount);
            destination.Deposit(amount);
        }
    }

    public enum AccountType
    {
        Customer,
        Porter,
        Host
    }
} 