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
        private readonly IInstallmentRepository installmentRepository;
        private readonly ILogger<LoanService> logger;
        private readonly IMapper mapper;

        public LoanService
            (
                ILoanRepository loanRepository,
                IInstallmentRepository installmentRepository,
                ILogger<LoanService> logger, 
                IMapper mapper
            )
        {
            this.loanRepository = loanRepository;
            this.installmentRepository = installmentRepository;
            this.logger = logger;
            this.mapper = mapper;
        }
        public async Task<BaseResponseGeneric<int>> AddAsync(LoanRequestDto request)
        {
            var response = new BaseResponseGeneric<int>();
            try
            {
                var loan = mapper.Map<Loan>(request);

                switch (loan.Frecuency.ToLower())
                {
                    case "biweekly": //quincenal
                        loan.TotalInterestReceivable = loan.Amount * ((loan.InterestRate / 100) * loan.Fees);
                        loan.TotalAmountDue = loan.Amount + loan.TotalInterestReceivable;
                        loan.PaymentAmount = CalculateBiweeklyPayment(loan.Amount, loan.Fees, loan.InterestRate);
                        loan.EndDate = loan.StartDate.AddDays(loan.Fees * 15);
                        break;
                    case "monthly": //mensual
                        loan.TotalInterestReceivable = loan.Amount * ((loan.InterestRate / 100) * loan.Fees);
                        loan.TotalAmountDue = loan.Amount + loan.TotalInterestReceivable;
                        loan.PaymentAmount = CalculateMonthlyPayment(loan.Amount, loan.Fees, loan.InterestRate);
                        loan.EndDate = loan.StartDate.AddMonths(loan.Fees);
                        break;
                    default:
                        throw new ArgumentException("Frecuencia no válida");
                }
                loan.RemainingBalance = loan.TotalAmountDue;

                response.Data = await loanRepository.AddAsync(loan);
                response.Success = response.Data > 0;

                // Si el préstamo se guardó correctamente, generar y guardar las cuotas
                if (response.Success)
                {
                    await GenerateInstallments(loan);
                }
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Ocurrió un error al registrar el préstamo";
                logger.LogError(ex, "{ErrorMessage} {Message}", response.ErrorMessage, ex.Message);
            }
            return response;
        }

        private decimal CalculateBiweeklyPayment(decimal amount, int fees, decimal interestRate)
        {
            decimal interestAmount = amount * (interestRate / 100);
            decimal totalInterest = interestAmount * fees;
            decimal totalAmountDue = amount + totalInterest;
            return totalAmountDue / fees;
        }

        private decimal CalculateMonthlyPayment(decimal amount, int fees, decimal interestRate)
        {
            decimal interestAmount = amount * (interestRate / 100);
            decimal totalInterest = interestAmount * fees;
            decimal totalAmountDue = amount + totalInterest;
            return totalAmountDue / fees;
        }

        private async Task GenerateInstallments(Loan loan)
        {
            for (int i = 0; i < loan.Fees; i++)
            {
                var installment = new Installment
                {
                    LoanId = loan.Id,
                    Amount = loan.PaymentAmount,
                    DueDate = loan.Frecuency == "biweekly"
                                ? loan.StartDate.AddMonths(i + 1)
                                : loan.StartDate.AddDays((i + 1) * 15),
                    IsPaid = false
                };

                // Guardar cada cuota en la base de datos
                await installmentRepository.AddAsync(installment);
            }
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
