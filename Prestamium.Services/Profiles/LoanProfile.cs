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
            CreateMap<Loan, LoanResponseDto>()
            .ForMember(dest => dest.ClientName,
                opt => opt.MapFrom(src => $"{src.Client.FirstName} {src.Client.LastName}"));
            CreateMap<Installment, InstallmentResponseDto>();

            CreateMap<Loan, LoanDetailResponseDto>()
            .ForMember(dest => dest.ClientName,
                opt => opt.MapFrom(src => $"{src.Client.FirstName} {src.Client.LastName}"))
            .ForMember(dest => dest.BoxName,
                opt => opt.MapFrom(src => src.Box.Name));
        }
    }
}
