using System;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VendingMachine.API.Application.Commands;
using VendingMachine.API.Application.Commands.DTO;
using VendingMachine.API.Application.Queries;

namespace VendingMachine.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepositController : ControllerBase
    {
        private readonly ILogger<DepositController> _logger;
        private readonly IDepositQueries _depositQueries;
        private readonly IMediator _mediator;

        public DepositController(ILogger<DepositController> logger,
                                 IDepositQueries depositQueries,
                                 IMediator mediator)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _depositQueries = depositQueries ?? throw new ArgumentNullException(nameof(depositQueries));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        /// <summary>
        /// Current deposit 
        /// </summary>
        /// <returns>Total deposited coins</returns>
        [HttpGet]
        [ProducesResponseType(typeof(DepositDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetCurrent()
        {
            _logger.LogInformation($"Get deposit query");

            var currentDeposit = await _depositQueries.GetDepositAsync();
            return Ok(currentDeposit);
        }

        /// <summary>
        /// Accepts coins 
        /// </summary>
        /// <param name="coins">Certain amounts of coins of certain denomination</param>
        /// <returns>Total deposited coins</returns>
        [HttpPut]
        [ProducesResponseType(typeof(DepositDTO), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Accept([FromBody]AcceptCoinsCommand command)
        {
            _logger.LogInformation($"{command}");

            var currentDeposit = await _mediator.Send(command);
            return Created(HttpContext.Request.GetDisplayUrl(), currentDeposit);
        }

        /// <summary>
        /// Returns deposited coins
        /// </summary>
        [HttpDelete]
        [ProducesResponseType(typeof(DepositDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Return()
        {
            var command = new ReturnCoinsCommand();
            _logger.LogInformation($"{command}");

            var depositedCoins = await _mediator.Send(command);
            return Ok(depositedCoins);
        }
    }
}
