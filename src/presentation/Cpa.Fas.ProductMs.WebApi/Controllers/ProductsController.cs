using Cpa.Fas.ProductMs.Application.Products;
using Cpa.Fas.ProductMs.Application.Products.Commands.CreateProduct;
using Cpa.Fas.ProductMs.Application.Products.Queries.GetProductById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Cpa.Fas.ProductMs.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductRequestViewModel request)
        {
            // Todo: The userGuid should be retrieved from the authenticated user's claims or context
            // For now, we will use a placeholder or generate a new Guid.
            // If empty we should throw an exception or handle it appropriately.
            Guid userGuid = Guid.Parse(User.FindFirst("userGuid")?.Value ?? Guid.NewGuid().ToString());
            CreateProductCommand command = new CreateProductCommand(
                request.Name,
                request.Price,
                request.Stock,
                userGuid
            );

            var productId = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetProductById), new { id = productId }, productId);
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductDto>> GetProductById(Guid id)
        {
            var query = new GetProductByIdQuery(id);
            var product = await _mediator.Send(query);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }
    }
}
