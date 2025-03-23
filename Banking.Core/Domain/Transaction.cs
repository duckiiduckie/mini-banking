using System;

namespace Banking.Core.Domain
{
    public class Transaction
    {
        public Guid Id { get; private set; }
        public Guid CustomerId { get; private set; }
        public decimal Amount { get; private set; }
        public TransactionType Type { get; private set; }
        public TransactionStatus Status { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? CompletedAt { get; private set; }
        public string Description { get; private set; }
        public Guid? PorterId { get; private set; }
        public Guid? HostId { get; private set; }

        private Transaction() { }

        public static Transaction CreateWithdrawal(Guid customerId, decimal amount, string description)
        {
            return new Transaction
            {
                Id = Guid.NewGuid(),
                CustomerId = customerId,
                Amount = amount,
                Type = TransactionType.Withdrawal,
                Status = TransactionStatus.Pending,
                CreatedAt = DateTime.UtcNow,
                Description = description
            };
        }

        public static Transaction CreateDeposit(Guid customerId, decimal amount, string description)
        {
            return new Transaction
            {
                Id = Guid.NewGuid(),
                CustomerId = customerId,
                Amount = amount,
                Type = TransactionType.Deposit,
                Status = TransactionStatus.Pending,
                CreatedAt = DateTime.UtcNow,
                Description = description
            };
        }

        public static Transaction CreatePayment(Guid customerId, decimal amount, Guid porterId, Guid hostId, string description)
        {
            return new Transaction
            {
                Id = Guid.NewGuid(),
                CustomerId = customerId,
                Amount = amount,
                Type = TransactionType.Payment,
                Status = TransactionStatus.Pending,
                CreatedAt = DateTime.UtcNow,
                PorterId = porterId,
                HostId = hostId,
                Description = description
            };
        }

        public void Complete()
        {
            Status = TransactionStatus.Completed;
            CompletedAt = DateTime.UtcNow;
        }

        public void Reject()
        {
            Status = TransactionStatus.Rejected;
            CompletedAt = DateTime.UtcNow;
        }
    }

    public enum TransactionType
    {
        Withdrawal,
        Deposit,
        Payment
    }

    public enum TransactionStatus
    {
        Pending,
        Completed,
        Rejected
    }
} 