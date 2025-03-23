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
    public class PaymentCommandHandler : IRequestHandler<PaymentCommand, Guid>
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IAccountRepository _accountRepository;

        public PaymentCommandHandler(
            ITransactionRepository transactionRepository,
            IAccountRepository accountRepository)
        {
            _transactionRepository = transactionRepository;
            _accountRepository = accountRepository;
        }

        public async Task<Guid> Handle(PaymentCommand request, CancellationToken cancellationToken)
        {
            var customerAccount = await _accountRepository.GetByUserId(request.CustomerId);
            var porterAccount = await _accountRepository.GetByUserId(request.PorterId);
            var hostAccount = await _accountRepository.GetByUserId(request.HostId);

            if (customerAccount == null || porterAccount == null || hostAccount == null)
                throw new InvalidOperationException("One or more accounts not found");

            if (customerAccount.Balance < request.Amount)
                throw new InvalidOperationException("Insufficient funds");

            var transaction = Transaction.CreatePayment(
                request.CustomerId,
                request.Amount,
                request.PorterId,
                request.HostId,
                request.Description);

            await _transactionRepository.Add(transaction);
            return transaction.Id;
        }
    }
} 