using AutoMapper;
using Microsoft.Extensions.Logging;
using Prestamium.Dto.Request;
using Prestamium.Dto.Response;
using Prestamium.Entities;
using Prestamium.Repositories.Interfaces;
using Prestamium.Services.Interfaces;

namespace Prestamium.Services.Services
{
    public class BoxService : IBoxService
    {
        private readonly IBoxRepository boxRepository;
        private readonly IBoxTransactionRepository transactionRepository;
        private readonly IMapper mapper;
        private readonly ILogger<BoxService> logger;

        public BoxService(
            IBoxRepository boxRepository,
            IBoxTransactionRepository transactionRepository,
            IMapper mapper,
            ILogger<BoxService> logger)
        {
            this.boxRepository = boxRepository;
            this.transactionRepository = transactionRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<BaseResponseGeneric<int>> CreateAsync(BoxRequestDto request)
        {
            var response = new BaseResponseGeneric<int>();
            try
            {
                var box = mapper.Map<Box>(request);
                box.CurrentBalance = request.InitialBalance;

                response.Data = await boxRepository.CreateAsync(box);

                if (response.Data > 0)
                {
                    var transaction = new BoxTransaction
                    {
                        BoxId = response.Data,
                        Amount = request.InitialBalance,
                        Description = "Saldo inicial",
                        Type = "income",
                        TransactionDate = DateTime.Now,
                        PreviousBalance = 0,
                        NewBalance = request.InitialBalance
                    };

                    await transactionRepository.CreateAsync(transaction);
                }

                response.Success = response.Data > 0;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Error al registrar la caja";
                logger.LogError(ex, "{ErrorMessage} {Message}", response.ErrorMessage, ex.Message);
            }
            return response;
        }

        public async Task<BaseResponseGeneric<ICollection<BoxResponseDto>>> GetAllAsync()
        {
            var response = new BaseResponseGeneric<ICollection<BoxResponseDto>>();
            try
            {
                var boxes = await boxRepository.GetAllAsync();
                response.Data = mapper.Map<ICollection<BoxResponseDto>>(boxes);
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Error al obtener las cajas";
                logger.LogError(ex, "{ErrorMessage} {Message}", response.ErrorMessage, ex.Message);
            }
            return response;
        }

        public async Task<BaseResponseGeneric<BoxResponseDto>> GetByIdAsync(int id)
        {
            var response = new BaseResponseGeneric<BoxResponseDto>();
            try
            {
                var box = await boxRepository.GetByIdAsync(id);
                if (box != null)
                {
                    response.Data = mapper.Map<BoxResponseDto>(box);
                    response.Success = true;
                }
                else
                {
                    response.ErrorMessage = "Caja no encontrada";
                }
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Error al obtener la caja";
                logger.LogError(ex, "{ErrorMessage} {Message}", response.ErrorMessage, ex.Message);
            }
            return response;
        }

        public async Task<BaseResponseGeneric<int>> CreateTransactionAsync(BoxTransactionRequestDto request)
        {
            var response = new BaseResponseGeneric<int>();
            try
            {
                var box = await boxRepository.GetByIdAsync(request.BoxId);
                if (box == null)
                {
                    response.ErrorMessage = "Caja no encontrada";
                    return response;
                }

                decimal previousBalance = box.CurrentBalance;
                decimal newBalance = request.Type.ToLower() == "income"
                    ? box.CurrentBalance + request.Amount
                    : box.CurrentBalance - request.Amount;

                if (newBalance < 0)
                {
                    response.ErrorMessage = "Saldo insuficiente";
                    return response;
                }

                var transaction = mapper.Map<BoxTransaction>(request);
                transaction.TransactionDate = DateTime.Now;
                transaction.PreviousBalance = previousBalance;
                transaction.NewBalance = newBalance;

                response.Data = await transactionRepository.CreateAsync(transaction);

                if (response.Data > 0)
                {
                    box.CurrentBalance = newBalance;
                    await boxRepository.UpdateAsync();
                    response.Success = true;
                }
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Error al registrar la transacción";
                logger.LogError(ex, "{ErrorMessage} {Message}", response.ErrorMessage, ex.Message);
            }
            return response;
        }
    }
}
