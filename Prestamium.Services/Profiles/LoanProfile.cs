using AutoMapper;
using Prestamium.Dto.Request;
using Prestamium.Dto.Response;
using Prestamium.Entities;

namespace Prestamium.Services.Profiles
{
    public class LoanProfile : Profile
    {
        public LoanProfile()
        {
            CreateMap<LoanRequestDto, Loan>();
            CreateMap<Loan, LoanResponseDto>();
        }
    }
}
