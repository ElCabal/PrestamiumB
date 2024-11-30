using Microsoft.AspNetCore.Mvc;
using Prestamium.Dto.Request;
using Prestamium.Services.Interfaces;

namespace Prestamium.Api.Controllers
{
    [Route("api/loans")]
    [ApiController]
    public class LoanController : ControllerBase
    {
        private readonly ILoanService _loanService;

        public LoanController(ILoanService loanService)
        {
            _loanService = loanService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllLoans()
        {
            var response = await _loanService.GetAllAsync();
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetLoanById(int id)
        {
            var response = await _loanService.GetByIdAsync(id);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpGet("client/{clientId}")]
        public async Task<IActionResult> GetLoansByClient(int clientId)
        {
            var response = await _loanService.GetByClientIdAsync(clientId);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateLoan([FromBody] LoanRequestDto request)
        {
            var response = await _loanService.CreateAsync(request);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpGet("installment/{installmentId}/late-fees")]
        public async Task<IActionResult> CalculateLateFeesAsync(int installmentId, [FromQuery] DateTime paymentDate)
        {
            var response = await _loanService.CalculateLateFeesAsync(installmentId, paymentDate);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpPost("installment/{installmentId}/payment")]
        public async Task<IActionResult> RegisterInstallmentPayment(
            int installmentId,
            [FromBody] InstallmentPaymentRequestDto request)
        {
            var response = await _loanService.RegisterInstallmentPaymentAsync(
                installmentId,
                request.Amount,
                request.BoxId);
            return response.Success ? Ok(response) : BadRequest(response);
        }
    }
}
