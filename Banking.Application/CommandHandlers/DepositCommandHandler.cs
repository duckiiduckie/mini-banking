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
    public class DepositCommandHandler : IRequestHandler<DepositCommand, Guid>
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IAccountRepository _accountRepository;

        public DepositCommandHandler(
            ITransactionRepository transactionRepository,
            IAccountRepository accountRepository)
        {
            _transactionRepository = transactionRepository;
            _accountRepository = accountRepository;
        }

        public async Task<Guid> Handle(DepositCommand request, CancellationToken cancellationToken)
        {
            var customerAccount = await _accountRepository.GetByUserId(request.CustomerId);
            if (customerAccount == null)
                throw new InvalidOperationException("Customer account not found");

            var transaction = Transaction.CreateDeposit(
                request.CustomerId,
                request.Amount,
                request.Description);

            await _transactionRepository.Add(transaction);
            return transaction.Id;
        }
    }
} 