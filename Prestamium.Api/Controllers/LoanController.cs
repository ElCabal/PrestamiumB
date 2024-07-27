using Microsoft.AspNetCore.Mvc;
using Prestamium.Dto.Request;
using Prestamium.Services.Interfaces;

namespace Prestamium.Api.Controllers
{
    [Route("api/loans")]
    [ApiController]
    public class LoanController : Controller
    {
        private readonly ILoanService loanService;

        public LoanController( ILoanService loanService)
        {
            this.loanService = loanService;
        }

        [HttpPost]
        public async Task<IActionResult> Post(LoanRequestDto loanRequestDto)
        {
            var response = await loanService.AddAsync(loanRequestDto);
            return response.Success ? Ok(response) : BadRequest(response);
        }

    }
}
