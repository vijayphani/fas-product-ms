using Cpa.Fas.ProductMs.Application.Products;
using Cpa.Fas.ProductMs.Application.Products.Commands.CreateProduct;
using Cpa.Fas.ProductMs.Application.Products.Queries.GetProductById;
using Cpa.Fas.ProductMs.WebApi.Controllers;
using Cpa.Fas.ProductMs.WebApi.Models;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;

namespace Cpa.Fas.ProductMs.WebApi.Tests.Controllers
{
    public class ProductsControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ProductsController _controller;

        public ProductsControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new ProductsController(_mediatorMock.Object);
        }

        private void SetUserWithGuid(Guid userGuid)
        {
            var claims = new List<Claim> { new Claim("userGuid", userGuid.ToString()) };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var principal = new ClaimsPrincipal(identity);
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = principal }
            };
        }

        [Fact]
        public async Task CreateProduct_ShouldReturnCreatedAtAction_WhenProductIsCreated()
        {
            // Arrange  
            var userGuid = Guid.NewGuid();
            SetUserWithGuid(userGuid);

            var request = new CreateProductRequestViewModel(
                Name: "Test Product",
                Price: 10.5m,
                Stock: 5
            );
            var createdProductId = Guid.NewGuid();

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<CreateProductCommand>(), default))
                .ReturnsAsync(createdProductId);

            // Act  
            var result = await _controller.CreateProduct(request);

            // Assert  
            var createdAtAction = result as CreatedAtActionResult;
            createdAtAction.Should().NotBeNull();
            createdAtAction!.ActionName.Should().Be(nameof(_controller.GetProductById));
            createdAtAction.RouteValues!["id"].Should().Be(createdProductId);

            var apiResponse = createdAtAction.Value as ApiResponse<Guid>;
            apiResponse.Should().NotBeNull();
            apiResponse!.Success.Should().BeTrue();
            apiResponse.Result.Should().Be(createdProductId);
            apiResponse.Message.Should().Contain(request.Name);

            _mediatorMock.Verify(m => m.Send(It.Is<CreateProductCommand>(c =>
                c.Name == request.Name &&
                c.Price == request.Price &&
                c.Stock == request.Stock &&
                c.userGuid == userGuid
            ), default), Times.Once);
        }

        [Fact]
        public async Task CreateProduct_ShouldUseGeneratedGuid_WhenUserGuidClaimIsMissing()
        {
            // Arrange  
            // No userGuid claim set  
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal() }
            };

            var request = new CreateProductRequestViewModel(
                Name: "Test Product",
                Price: 10.5m,
                Stock: 5
            );
            var createdProductId = Guid.NewGuid();

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<CreateProductCommand>(), default))
                .ReturnsAsync(createdProductId);

            // Act  
            var result = await _controller.CreateProduct(request);

            // Assert  
            var createdAtAction = result as CreatedAtActionResult;
            createdAtAction.Should().NotBeNull();

            _mediatorMock.Verify(m => m.Send(It.Is<CreateProductCommand>(c =>
                c.Name == request.Name &&
                c.Price == request.Price &&
                c.Stock == request.Stock &&
                c.userGuid != Guid.Empty
            ), default), Times.Once);
        }

        [Fact]
        public async Task GetProductById_ShouldReturnOk_WhenProductExists()
        {
            // Arrange  
            var productId = Guid.NewGuid();
            var productDto = new ProductDto(
                Id: productId,
                Name: "Test Product",
                Price: 10.5m,
                Stock: 5
            );

            _mediatorMock
                .Setup(m => m.Send(It.Is<GetProductByIdQuery>(q => q.Id == productId), default))
                .ReturnsAsync(productDto);

            // Act  
            var result = await _controller.GetProductById(productId);

            // Assert  
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();

            var apiResponse = okResult!.Value as ApiResponse<ProductDto>;
            apiResponse.Should().NotBeNull();
            apiResponse!.Success.Should().BeTrue();
            apiResponse.Result.Should().Be(productDto);
            apiResponse.Message.Should().Contain(productId.ToString());
        }

        [Fact]
        public async Task GetProductById_ShouldReturnNotFound_WhenProductDoesNotExist()
        {
            // Arrange
            var productId = Guid.NewGuid();

            _mediatorMock
                .Setup(m => m.Send(It.Is<GetProductByIdQuery>(q => q.Id == productId), default))
                .ReturnsAsync((ProductDto?)null);

            // Act
            var result = await _controller.GetProductById(productId);

            // Assert
            var notFoundResult = result as NotFoundObjectResult;
            notFoundResult.Should().NotBeNull();

            var apiResponse = notFoundResult!.Value as ApiResponse<ProductDto>;
            apiResponse.Should().NotBeNull();
            apiResponse!.Success.Should().BeFalse();
            apiResponse.Result.Should().BeNull();
            apiResponse.Message.Should().Contain(productId.ToString());
        }
    }
}
