using System;
using System.Threading;
using System.Threading.Tasks;
using Banking.Application.Commands;
using Banking.Core.Domain;
using Banking.Core.Repositories;
using Banking.Infrastructure.Repositories;
using MediatR;

namespace Banking.Application.CommandHandlers
{
    public class ApproveTransactionCommandHandler : IRequestHandler<ApproveTransactionCommand, bool>
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IAccountRepository _accountRepository;

        public ApproveTransactionCommandHandler(
            ITransactionRepository transactionRepository,
            IAccountRepository accountRepository)
        {
            _transactionRepository = transactionRepository;
            _accountRepository = accountRepository;
        }

        public async Task<bool> Handle(ApproveTransactionCommand request, CancellationToken cancellationToken)
        {
            var transaction = await _transactionRepository.GetById(request.TransactionId);
            if (transaction == null)
                throw new InvalidOperationException("Transaction not found");

            if (transaction.Status != TransactionStatus.Pending)
                throw new InvalidOperationException("Transaction is not in pending status");

            var customerAccount = await _accountRepository.GetByUserId(transaction.CustomerId);
            if (customerAccount == null)
                throw new InvalidOperationException("Customer account not found");

            switch (transaction.Type)
            {
                case TransactionType.Withdrawal:
                    customerAccount.Withdraw(transaction.Amount);
                    break;

                case TransactionType.Deposit:
                    customerAccount.Deposit(transaction.Amount);
                    break;

                case TransactionType.Payment:
                    var porterAccount = await _accountRepository.GetByUserId(transaction.PorterId.Value);
                    var hostAccount = await _accountRepository.GetByUserId(transaction.HostId.Value);

                    if (porterAccount == null || hostAccount == null)
                        throw new InvalidOperationException("Porter or host account not found");

                    customerAccount.Withdraw(transaction.Amount);
                    porterAccount.Deposit(transaction.Amount * 0.7m); // 70% for porter
                    hostAccount.Deposit(transaction.Amount * 0.3m); // 30% for host
                    break;
            }

            transaction.Complete();
            await _transactionRepository.Update(transaction);
            return true;
        }
    }
} 