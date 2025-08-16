using Cpa.Fas.ProductMs.Application.Products.Commands.CreateProduct;
using Cpa.Fas.ProductMs.Application.Products.Queries.GetProductById;
using Cpa.Fas.ProductMs.WebApi.Models;
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
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductCommandRequest request)
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
            var apiResponse = new ApiResponse<Guid>
            {
                Result = productId,
                Success = true,
                Message = $"Product with name {request.Name} is created successfully."
            };
            return CreatedAtAction(nameof(GetProductById), new { id = productId }, apiResponse);
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProductById(Guid id)
        {
            var query = new GetProductByIdQuery(id);
            var product = await _mediator.Send(query);

            var apiResponse = new ApiResponse<GetProductByIdQueryResponse>
            {
                Success = true,
                Message = $"Product with Id {id} is found",
                Result = product
            };

            return Ok(apiResponse);
        }
    }
}