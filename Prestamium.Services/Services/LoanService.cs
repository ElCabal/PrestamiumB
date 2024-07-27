using AutoMapper;
using Microsoft.Extensions.Logging;
using Prestamium.Dto.Request;
using Prestamium.Dto.Response;
using Prestamium.Entities;
using Prestamium.Repositories.Interfaces;
using Prestamium.Services.Interfaces;

namespace Prestamium.Services.Services
{
    public class LoanService : ILoanService
    {
        private readonly ILoanRepository loanRepository;
        private readonly ILogger<LoanService> logger;
        private readonly IMapper mapper;

        public LoanService(ILoanRepository loanRepository, ILogger<LoanService> logger, IMapper mapper) 
        {
            this.loanRepository = loanRepository;
            this.logger = logger;
            this.mapper = mapper;
        }
        public async Task<BaseResponseGeneric<int>> AddAsync(LoanRequestDto request)
        {
            var response = new BaseResponseGeneric<int>();
            try
            {
                var loan = mapper.Map<Loan>(request);
                loan.TotalInterestReceivable = loan.Amount * loan.InterestRate / 100;
                loan.TotalAmountDue = loan.Amount + loan.TotalInterestReceivable;

                response.Data = await loanRepository.AddAsync(loan);
                response.Success = response.Data > 0;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Ocurrió un error al registrar el préstamo";
                logger.LogError(ex, "{ErrorMessage} {Message}", response.ErrorMessage, ex.Message);
            }
            return response;
        }

        public Task<BaseResponseGeneric<ICollection<LoanResponseDto>>> GetAsync()
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponseGeneric<LoanResponseDto>> GetAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
