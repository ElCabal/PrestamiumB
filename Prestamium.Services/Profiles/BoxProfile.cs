using AutoMapper;
using Prestamium.Dto.Request;
using Prestamium.Dto.Response;
using Prestamium.Entities;

namespace Prestamium.Services.Profiles
{
    public class BoxProfile : Profile
    {
        public BoxProfile()
        {
            CreateMap<BoxRequestDto, Box>();
            CreateMap<Box, BoxResponseDto>();
            CreateMap<BoxTransactionRequestDto, BoxTransaction>();
            CreateMap<BoxTransaction, BoxTransactionResponseDto>();
        }
    }
}
