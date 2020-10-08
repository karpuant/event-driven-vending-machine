using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VendingMachine.API.Application.Commands.DTO;
using VendingMachine.Domain.Abstractions;
using VendingMachine.Domain.Exceptions;

namespace VendingMachine.API.Application.Commands.Handlers
{
    public class PurchaseCommandHandler : IRequestHandler<PurchaseCommand, PurchasetDTO>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;
        private readonly ILogger<PurchaseCommandHandler> _logger;

        public PurchaseCommandHandler(IUnitOfWork unitOfWork,
                                      IMediator mediator,
                                      ILogger<PurchaseCommandHandler> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<PurchasetDTO> Handle(PurchaseCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var product = await _unitOfWork.Products.FindByIdAsync(request.ProductName);
                var wallet = await _unitOfWork.Wallet.FindByIdAsync("wallet");

                product.ValidatePriceAndAvailability(wallet.Deposit.Sum(c => c.TotalAmount));
                var change = wallet.MoveCoinsToWalletAndReturnChange(product.Price);
                product.CheckOut();

                _unitOfWork.BeginTransaction();

                await _unitOfWork.Products.UpdateAsync(product);
                await _unitOfWork.Wallet.UpdateAsync(wallet);

                if (!_unitOfWork.CommitTransaction())
                    throw new VendingMachineDomainException("Technical problem occured. Try again.");

                return new PurchasetDTO { ProductName = product.Name, Change = change.Select(c => CoinSetDTO.FromCoinSet(c)), Message = "Thank you!" };
            } 
            catch (VendingMachineDomainException vmde)
            {
                return new PurchasetDTO { ProductName = request.ProductName, Failed = true, Message = vmde.Message };
            }
        }
    }
}
