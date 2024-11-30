using Microsoft.AspNetCore.Mvc;
using Prestamium.Dto.Request;
using Prestamium.Services.Interfaces;

namespace Prestamium.Api.Controllers
{
    [Route("api/clients")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IClientService _clientService;

        public ClientController(IClientService clientService)
        {
            _clientService = clientService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllClients()
        {
            var response = await _clientService.GetAllAsync();
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetClientById(int id)
        {
            var response = await _clientService.GetByIdAsync(id);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpGet("document/{documentNumber}")]
        public async Task<IActionResult> GetClientByDocument(string documentNumber)
        {
            var response = await _clientService.GetByDocumentNumberAsync(documentNumber);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateClient([FromBody] ClientRequestDto request)
        {
            var response = await _clientService.CreateAsync(request);
            return response.Success ? Ok(response) : BadRequest(response);
        }
    }
}
