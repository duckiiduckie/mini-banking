using System;
using MediatR;

namespace Banking.Application.Commands
{
    public class DepositCommand : IRequest<Guid>
    {
        public Guid CustomerId { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
    }
} 