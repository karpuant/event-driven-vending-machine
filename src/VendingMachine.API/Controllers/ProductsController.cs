using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VendingMachine.API.Application.Commands;
using VendingMachine.API.Application.Commands.DTO;
using VendingMachine.API.Application.Queries;
using VendingMachine.API.Application.Queries.DTO;

namespace VendingMachine.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<DepositController> _logger;
        private readonly IMediator _mediator;
        private readonly IProductQueries _productQueries;

        public ProductsController(ILogger<DepositController> logger,
                                  IMediator mediator,
                                  IProductQueries productQueries)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _productQueries = productQueries ?? throw new ArgumentNullException(nameof(productQueries));
        }


        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ProductDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetAvailableProducts()
        
        {
            var products = await _productQueries.GetAvailableProductsAsync();
            return Ok(products);
        }

        /// <summary>
        /// Returns deposited coins
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(IEnumerable<PurchasetDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Purchase([FromBody]PurchaseCommand command)
        {
            _logger.LogInformation($"{command}");

            var purchase = await _mediator.Send(command);
            return Ok(purchase);
        }
    }
}
