using AutoMapper;
using Microsoft.Extensions.Logging;
using Prestamium.Dto.Request;
using Prestamium.Dto.Response;
using Prestamium.Entities;
using Prestamium.Repositories.Interfaces;
using Prestamium.Services.Interfaces;

namespace Prestamium.Services.Services
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository clientRepository;
        private readonly IMapper mapper;
        private readonly ILogger<ClientService> logger;

        public ClientService(
            IClientRepository clientRepository,
            IMapper mapper,
            ILogger<ClientService> logger)
        {
            this.clientRepository = clientRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<BaseResponseGeneric<int>> CreateAsync(ClientRequestDto request)
        {
            var response = new BaseResponseGeneric<int>();
            try
            {
                var existingClient = await clientRepository.GetByDocumentNumberAsync(request.DocumentNumber);
                if (existingClient != null)
                {
                    response.ErrorMessage = "Ya existe un cliente con este número de documento";
                    return response;
                }

                var client = mapper.Map<Client>(request);
                response.Data = await clientRepository.CreateAsync(client);
                response.Success = response.Data > 0;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Error al registrar el cliente";
                logger.LogError(ex, "{ErrorMessage} {Message}", response.ErrorMessage, ex.Message);
            }
            return response;
        }

        public async Task<BaseResponseGeneric<ICollection<ClientResponseDto>>> GetAllAsync()
        {
            var response = new BaseResponseGeneric<ICollection<ClientResponseDto>>();
            try
            {
                var clients = await clientRepository.GetAllAsync();
                response.Data = mapper.Map<ICollection<ClientResponseDto>>(clients);
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Error al obtener los clientes";
                logger.LogError(ex, "{ErrorMessage} {Message}", response.ErrorMessage, ex.Message);
            }
            return response;
        }

        public async Task<BaseResponseGeneric<ClientResponseDto>> GetByIdAsync(int id)
        {
            var response = new BaseResponseGeneric<ClientResponseDto>();
            try
            {
                var client = await clientRepository.GetByIdAsync(id);
                if (client != null)
                {
                    response.Data = mapper.Map<ClientResponseDto>(client);
                    response.Success = true;
                }
                else
                {
                    response.ErrorMessage = "Cliente no encontrado";
                }
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Error al obtener el cliente";
                logger.LogError(ex, "{ErrorMessage} {Message}", response.ErrorMessage, ex.Message);
            }
            return response;
        }

        public async Task<BaseResponseGeneric<ClientResponseDto>> GetByDocumentNumberAsync(string documentNumber)
        {
            var response = new BaseResponseGeneric<ClientResponseDto>();
            try
            {
                var client = await clientRepository.GetByDocumentNumberAsync(documentNumber);
                if (client != null)
                {
                    response.Data = mapper.Map<ClientResponseDto>(client);
                    response.Success = true;
                }
                else
                {
                    response.ErrorMessage = "Cliente no encontrado";
                }
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Error al obtener el cliente";
                logger.LogError(ex, "{ErrorMessage} {Message}", response.ErrorMessage, ex.Message);
            }
            return response;
        }
    }
}
