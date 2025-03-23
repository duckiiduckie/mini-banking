using System;
using System.Collections.Generic;
using MediatR;

namespace Banking.Application.Queries
{
    public class GetTransactionHistoryQuery : IRequest<IEnumerable<TransactionDto>>
    {
        public Guid CustomerId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }

    public class TransactionDto
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public string Description { get; set; }
        public Guid? PorterId { get; set; }
        public Guid? HostId { get; set; }
    }
} 