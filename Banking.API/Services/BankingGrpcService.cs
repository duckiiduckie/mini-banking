using System;
using System.Threading.Tasks;
using Banking.Application.Commands;
using Banking.Application.Queries;
using Grpc.Core;
using Google.Protobuf.WellKnownTypes;
using MediatR;

namespace Banking.API.Services
{
    public class BankingGrpcService : BankingService.BankingServiceBase
    {
        private readonly IMediator _mediator;

        public BankingGrpcService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override async Task<WithdrawResponse> Withdraw(WithdrawRequest request, ServerCallContext context)
        {
            var command = new WithdrawCommand
            {
                CustomerId = Guid.Parse(request.CustomerId),
                Amount = (decimal)request.Amount,
                Description = request.Description
            };

            var transactionId = await _mediator.Send(command);
            return new WithdrawResponse { TransactionId = transactionId.ToString() };
        }

        public override async Task<DepositResponse> Deposit(DepositRequest request, ServerCallContext context)
        {
            var command = new DepositCommand
            {
                CustomerId = Guid.Parse(request.CustomerId),
                Amount = (decimal)request.Amount,
                Description = request.Description
            };

            var transactionId = await _mediator.Send(command);
            return new DepositResponse { TransactionId = transactionId.ToString() };
        }

        public override async Task<PaymentResponse> Payment(PaymentRequest request, ServerCallContext context)
        {
            var command = new PaymentCommand
            {
                CustomerId = Guid.Parse(request.CustomerId),
                Amount = (decimal)request.Amount,
                PorterId = Guid.Parse(request.PorterId),
                HostId = Guid.Parse(request.HostId),
                Description = request.Description
            };

            var transactionId = await _mediator.Send(command);
            return new PaymentResponse { TransactionId = transactionId.ToString() };
        }

        public override async Task<ApproveTransactionResponse> ApproveTransaction(ApproveTransactionRequest request, ServerCallContext context)
        {
            var command = new ApproveTransactionCommand
            {
                TransactionId = Guid.Parse(request.TransactionId),
                AdminId = Guid.Parse(request.AdminId)
            };

            var success = await _mediator.Send(command);
            return new ApproveTransactionResponse { Success = success };
        }

        public override async Task<GetTransactionHistoryResponse> GetTransactionHistory(GetTransactionHistoryRequest request, ServerCallContext context)
        {
            var query = new GetTransactionHistoryQuery
            {
                CustomerId = Guid.Parse(request.CustomerId),
                FromDate = string.IsNullOrEmpty(request.FromDate) ? null : DateTime.Parse(request.FromDate),
                ToDate = string.IsNullOrEmpty(request.ToDate) ? null : DateTime.Parse(request.ToDate),
                Page = request.Page,
                PageSize = request.PageSize
            };

            var transactions = await _mediator.Send(query);
            var response = new GetTransactionHistoryResponse();

            foreach (var transaction in transactions)
            {
                response.Transactions.Add(new TransactionDto
                {
                    Id = transaction.Id.ToString(),
                    Amount = (double)transaction.Amount,
                    Type = transaction.Type,
                    Status = transaction.Status,
                    CreatedAt = transaction.CreatedAt.ToString("O"),
                    CompletedAt = transaction.CompletedAt?.ToString("O"),
                    Description = transaction.Description,
                    PorterId = transaction.PorterId?.ToString(),
                    HostId = transaction.HostId?.ToString()
                });
            }

            return response;
        }
    }
} 