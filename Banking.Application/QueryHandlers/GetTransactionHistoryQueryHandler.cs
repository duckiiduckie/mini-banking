using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Banking.Application.Queries;
using Banking.Core.Repositories;
using Banking.Infrastructure.Repositories;
using MediatR;

namespace Banking.Application.QueryHandlers
{
    public class GetTransactionHistoryQueryHandler : IRequestHandler<GetTransactionHistoryQuery, IEnumerable<TransactionDto>>
    {
        private readonly ITransactionRepository _transactionRepository;

        public GetTransactionHistoryQueryHandler(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<IEnumerable<TransactionDto>> Handle(GetTransactionHistoryQuery request, CancellationToken cancellationToken)
        {
            var transactions = await _transactionRepository.GetByCustomerId(
                request.CustomerId,
                request.FromDate,
                request.ToDate,
                request.Page,
                request.PageSize);

            return transactions.Select(t => new TransactionDto
            {
                Id = t.Id,
                Amount = t.Amount,
                Type = t.Type.ToString(),
                Status = t.Status.ToString(),
                CreatedAt = t.CreatedAt,
                CompletedAt = t.CompletedAt,
                Description = t.Description,
                PorterId = t.PorterId,
                HostId = t.HostId
            });
        }
    }
} 