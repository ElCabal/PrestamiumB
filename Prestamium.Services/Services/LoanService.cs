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
                loan.RemainingBalance = -loan.TotalAmountDue;

                switch (loan.Frecuency.ToLower())
                {
                    case "diaria":
                        loan.PaymentAmount = CalculateDailyPayment(loan.Amount, loan.Fees, loan.InterestRate);
                        loan.EndDate = loan.StartDate.AddDays(loan.Fees);
                        break;
                    case "semanal":
                        loan.PaymentAmount = CalculateWeeklyPayment(loan.Amount, loan.Fees, loan.InterestRate);
                        loan.EndDate = loan.StartDate.AddDays(loan.Fees * 7);
                        break;
                    case "mensual":
                        loan.PaymentAmount = CalculateMonthlyPayment(loan.Amount, loan.Fees, loan.InterestRate);
                        loan.EndDate = loan.StartDate.AddMonths(loan.Fees);
                        break;
                    default:
                        throw new ArgumentException("Frecuencia no válida");
                }

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

        private decimal CalculateDailyPayment(decimal amount, int fees, decimal interestRate)
        {
            var dailyInterestRate = interestRate / 100 / 365; // Convertir la tasa de interés anual a diaria
            var paymentAmount = amount * dailyInterestRate / (1 - (decimal)Math.Pow((double)(1 + dailyInterestRate), -fees));
            return paymentAmount;
        }

        private decimal CalculateWeeklyPayment(decimal amount, int fees, decimal interestRate)
        {
            var weeklyInterestRate = interestRate / 100 / 52; // Convertir la tasa de interés anual a semanal
            var paymentAmount = amount * weeklyInterestRate / (1 - (decimal)Math.Pow((double)(1 + weeklyInterestRate), -fees));
            return paymentAmount;
        }

        private decimal CalculateMonthlyPayment(decimal amount, int fees, decimal interestRate)
        {
            var monthlyInterestRate = interestRate / 100 / 12;
            var paymentAmount = amount * monthlyInterestRate / (1 - (decimal)Math.Pow((double)(1 + monthlyInterestRate), -fees));
            return paymentAmount;
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
