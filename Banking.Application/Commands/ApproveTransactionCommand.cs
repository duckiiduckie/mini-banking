using System;
using MediatR;

namespace Banking.Application.Commands
{
    public class ApproveTransactionCommand : IRequest<bool>
    {
        public Guid TransactionId { get; set; }
        public Guid AdminId { get; set; }
    }
} 