using Aggregator.Protos;
using Aggregator.Services;
using Banking.API;
using Microsoft.AspNetCore.Mvc;
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

namespace Aggregator.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BankingController : ControllerBase
{
    private readonly IBankingGrpcService _bankingService;

    public BankingController(IBankingGrpcService bankingService)
    {
        _bankingService = bankingService;
    }

    [HttpPost("withdraw")]
    public async Task<ActionResult<WithdrawResponse>> Withdraw([FromBody] WithdrawRequest request)
    {
        try
        {
            var result = await _bankingService.WithdrawAsync(request);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error processing withdrawal", error = ex.Message });
        }
    }

    [HttpPost("deposit")]
    public async Task<ActionResult<DepositResponse>> Deposit([FromBody] DepositRequest request)
    {
        try
        {
            var result = await _bankingService.DepositAsync(request);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error processing deposit", error = ex.Message });
        }
    }

    [HttpPost("payment")]
    public async Task<ActionResult<PaymentResponse>> Payment([FromBody] PaymentRequest request)
    {
        try
        {
            var result = await _bankingService.PaymentAsync(request);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error processing payment", error = ex.Message });
        }
    }

    [HttpPost("transactions/{transactionId}/approve")]
    public async Task<ActionResult<ApproveTransactionResponse>> ApproveTransaction(
        string transactionId,
        [FromBody] ApproveTransactionRequest request)
    {
        try
        {
            request.TransactionId = transactionId;
            var result = await _bankingService.ApproveTransactionAsync(request);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error approving transaction", error = ex.Message });
        }
    }

    [HttpGet("transactions")]
    public async Task<ActionResult<GetTransactionHistoryResponse>> GetTransactionHistory(
        [FromQuery] string customerId,
        [FromQuery] string fromDate,
        [FromQuery] string toDate,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        try
        {
            var request = new GetTransactionHistoryRequest
            {
                CustomerId = customerId,
                FromDate = fromDate,
                ToDate = toDate,
                Page = page,
                PageSize = pageSize
            };
            var result = await _bankingService.GetTransactionHistoryAsync(request);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error retrieving transaction history", error = ex.Message });
        }
    }
} 