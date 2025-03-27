using Aggregator.DTOs;
using Banking.API;
using Grpc.Net.Client;


namespace Aggregator.Services;

public interface IBankingGrpcService
{
    Task<WithdrawResponseHttp> WithdrawAsync(WithdrawRequestHttp request);
    Task<DepositResponse> DepositAsync(DepositRequest request);
    Task<PaymentResponse> PaymentAsync(PaymentRequest request);
    Task<ApproveTransactionResponse> ApproveTransactionAsync(ApproveTransactionRequest request);
    Task<GetTransactionHistoryResponse> GetTransactionHistoryAsync(GetTransactionHistoryRequest request);
}

public class BankingGrpcService : IBankingGrpcService
{
    private readonly BankingService.BankingServiceClient _client;

    public BankingGrpcService(IConfiguration configuration)
    {
        var bankingApiUrl = configuration["BankingApi:GrpcUrl"] ?? "http://localhost:5001";
        var channel = GrpcChannel.ForAddress(bankingApiUrl);
        _client = new BankingService.BankingServiceClient(channel);
    }

    public async Task<WithdrawResponseHttp> WithdrawAsync(WithdrawRequestHttp requestHttp)
    {
        try
        {
            var request = new WithdrawRequest
            {
                CustomerId = requestHttp.CustomerId,
                Amount = requestHttp.Amount,
                Description = requestHttp.Description
            };
            var response = await _client.WithdrawAsync(request);
            return new WithdrawResponseHttp
            {
                TransactionId = response.TransactionId,
                Status = "OK",
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
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