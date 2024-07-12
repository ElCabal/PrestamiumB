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
        private readonly IClientRepository repository;
        private readonly ILogger<ClientService> logger;
        private readonly IMapper mapper;

        public ClientService(IClientRepository repository, ILogger<ClientService> logger, IMapper mapper) 
        {
            this.repository = repository;
            this.logger = logger;
            this.mapper = mapper;
        }
        public async Task<BaseResponseGeneric<ICollection<ClientResponseDto>>> GetAsync()
        {
            var response = new BaseResponseGeneric<ICollection<ClientResponseDto>>();
            try
            {
                response.Data = mapper.Map<ICollection<ClientResponseDto>>(await repository.GetAsync());
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Ocurrió un error al obtener la información";
                logger.LogError(ex, "{ErrorMessage} {Message}", response.ErrorMessage, ex.Message);
            }
            return response;
        }

        public async Task<BaseResponseGeneric<ClientResponseDto>> GetAsync(int id)
        {
            var response = new BaseResponseGeneric<ClientResponseDto>();
            try
            {
                response.Data = mapper.Map<ClientResponseDto>(await repository.GetAsync(id));
                response.Success = response.Data != null;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Ocurrió un error al obtener la información del cliente";
                logger.LogError(ex, "{ErrorMessage} {Message}", response.ErrorMessage, ex.Message);
            }
            return response;
        }

        public async Task<BaseResponseGeneric<int>> AddAsync(ClientRequestDto request)
        {
            var response = new BaseResponseGeneric<int>();
            try
            {
                response.Data = await repository.AddAsync(mapper.Map<Client>(request));
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Ocurrió un error al añadir un cliente";
                logger.LogError(ex, "{ErrorMessage} {Message}", response.ErrorMessage, ex.Message);
            }
            return response;
        }
        public async Task<BaseResponse> UpdateAsync(int id, ClientRequestDto request)
        {
            var response = new BaseResponseGeneric<int>();
            try
            {
                response.Data = await repository.AddAsync(mapper.Map<Client>(request));
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Ocurrió un error al actualizar la información del cliente";
                logger.LogError(ex, "{ErrorMessage} {Message}", response.ErrorMessage, ex.Message);
            }
            return response;
        }

        public async Task<BaseResponse> DeleteAsync(int id)
        {
            var response = new BaseResponse();
            try
            {
                var data = await repository.GetAsync(id);
                if (data is null)
                {
                    response.ErrorMessage = $"No existe el cliente con id {id}.";
                    return response;
                }

                await repository.DeleteAsync(id);
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Ocurrió un error al eliminar el cliente";
                logger.LogError(ex, "{ErrorMessage} {Message}", response.ErrorMessage, ex.Message);
            }
            return response;
        }

    }
}
