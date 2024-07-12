using AutoMapper;
using Prestamium.Dto.Request;
using Prestamium.Dto.Response;
using Prestamium.Entities;

namespace Prestamium.Services.Profiles
{
    public class ClientProfile : Profile
    {
        public ClientProfile()
        {
            CreateMap<Client, ClientResponseDto>();
            CreateMap<ClientRequestDto, Client>();
        }
    }
}
