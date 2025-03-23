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
    public class WithdrawCommandHandler : IRequestHandler<WithdrawCommand, Guid>
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IAccountRepository _accountRepository;

        public WithdrawCommandHandler(
            ITransactionRepository transactionRepository,
            IAccountRepository accountRepository)
        {
            _transactionRepository = transactionRepository;
            _accountRepository = accountRepository;
        }

        public async Task<Guid> Handle(WithdrawCommand request, CancellationToken cancellationToken)
        {
            var customerAccount = await _accountRepository.GetByUserId(request.CustomerId);
            if (customerAccount == null)
                throw new InvalidOperationException("Customer account not found");

            if (customerAccount.Balance < request.Amount)
                throw new InvalidOperationException("Insufficient funds");

            var transaction = Transaction.CreateWithdrawal(
                request.CustomerId,
                request.Amount,
                request.Description);

            await _transactionRepository.Add(transaction);
            return transaction.Id;
        }
    }
} 