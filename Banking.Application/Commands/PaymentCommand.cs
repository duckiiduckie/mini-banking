using System;
using MediatR;

namespace Banking.Application.Commands
{
    public class PaymentCommand : IRequest<Guid>
    {
        public Guid CustomerId { get; set; }
        public decimal Amount { get; set; }
        public Guid PorterId { get; set; }
        public Guid HostId { get; set; }
        public string Description { get; set; }
    }
} 