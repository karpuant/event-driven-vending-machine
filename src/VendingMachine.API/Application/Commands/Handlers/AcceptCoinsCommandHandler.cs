using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using VendingMachine.API.Application.Commands.DTO;
using VendingMachine.Domain.Abstractions;

namespace VendingMachine.API.Application.Commands.Handlers
{
    public class AcceptCoinsCommandHandler : IRequestHandler<AcceptCoinsCommand, DepositDTO>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;
        private readonly ILogger<AcceptCoinsCommandHandler> _logger;

        public AcceptCoinsCommandHandler(IUnitOfWork unitOfWork,
                                         IMediator mediator,
                                         ILogger<AcceptCoinsCommandHandler> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<DepositDTO> Handle(AcceptCoinsCommand request, CancellationToken cancellationToken)
        {
            var wallet = await _unitOfWork.Wallet.FindByIdAsync("wallet");
            wallet.ValidateAndPutCoinsOnDeposit(request.Denomination, request.Amount);

            await _unitOfWork.Wallet.UpdateAsync(wallet);

            return DepositDTO.FromCoinSets(wallet.Deposit);
        }
    }
}
