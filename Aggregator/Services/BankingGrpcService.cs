using Aggregator.Protos;
using Banking.API;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using ApproveTransactionRequest = Aggregator.Protos.ApproveTransactionRequest;
using ApproveTransactionResponse = Aggregator.Protos.ApproveTransactionResponse;
using DepositRequest = Aggregator.Protos.DepositRequest;
using DepositResponse = Aggregator.Protos.DepositResponse;
using GetTransactionHistoryRequest = Aggregator.Protos.GetTransactionHistoryRequest;
using GetTransactionHistoryResponse = Aggregator.Protos.GetTransactionHistoryResponse;
using PaymentRequest = Aggregator.Protos.PaymentRequest;
using PaymentResponse = Aggregator.Protos.PaymentResponse;
using WithdrawRequest = Aggregator.Protos.WithdrawRequest;
using WithdrawResponse = Aggregator.Protos.WithdrawResponse;

namespace Aggregator.Services;

public interface IBankingGrpcService
{
    Task<WithdrawResponse> WithdrawAsync(WithdrawRequest request);
    Task<DepositResponse> DepositAsync(DepositRequest request);
    Task<PaymentResponse> PaymentAsync(PaymentRequest request);
    Task<ApproveTransactionResponse> ApproveTransactionAsync(ApproveTransactionRequest request);
    Task<GetTransactionHistoryResponse> GetTransactionHistoryAsync(GetTransactionHistoryRequest request);
}

public class BankingGrpcService : IBankingGrpcService
{
    private readonly Protos.BankingService.BankingServiceClient _client;

    public BankingGrpcService(IConfiguration configuration)
    {
        var bankingApiUrl = configuration["BankingApi:GrpcUrl"] ?? "http://localhost:5001";
        var channel = GrpcChannel.ForAddress(bankingApiUrl);
        _client = new Protos.BankingService.BankingServiceClient(channel);
    }

    public async Task<WithdrawResponse> WithdrawAsync(WithdrawRequest request)
    {
        return await _client.WithdrawAsync(request);
    }

    public async Task<DepositResponse> DepositAsync(DepositRequest request)
    {
        return await _client.DepositAsync(request);
    }

    public async Task<PaymentResponse> PaymentAsync(PaymentRequest request)
    {
        return await _client.PaymentAsync(request);
    }

    public async Task<ApproveTransactionResponse> ApproveTransactionAsync(ApproveTransactionRequest request)
    {
        return await _client.ApproveTransactionAsync(request);
    }

    public async Task<GetTransactionHistoryResponse> GetTransactionHistoryAsync(GetTransactionHistoryRequest request)
    {
        return await _client.GetTransactionHistoryAsync(request);
    }
} 