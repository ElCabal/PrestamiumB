using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Prestamium.Dto.Request;
using Prestamium.Services.Interfaces;

namespace Prestamium.Api.Controllers
{
    [Authorize]
    [Route("api/boxes")]
    [ApiController]
    public class BoxController : ControllerBase
    {
        private readonly IBoxService _boxService;

        public BoxController(IBoxService boxService)
        {
            _boxService = boxService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBoxes()
        {
            var response = await _boxService.GetAllAsync();
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBoxById(int id)
        {
            var response = await _boxService.GetByIdAsync(id);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBox([FromBody] BoxRequestDto request)
        {
            var response = await _boxService.CreateAsync(request);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpGet("{id}/detail")]
        public async Task<IActionResult> GetBoxDetail(int id)
        {
            var response = await _boxService.GetDetailAsync(id);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpPost("transactions")]
        public async Task<IActionResult> CreateTransaction([FromBody] BoxTransactionRequestDto request)
        {
            var response = await _boxService.CreateTransactionAsync(request);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpGet("{boxId}/transactions")]
        public async Task<IActionResult> GetBoxTransactions(int boxId)
        {
            var response = await _boxService.GetTransactionsByBoxIdAsync(boxId);
            return response.Success ? Ok(response) : BadRequest(response);
        }
    }
}
