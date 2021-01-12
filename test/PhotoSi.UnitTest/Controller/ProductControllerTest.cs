using AutoMapper;
using DomainModel.Dtos;
using Microsoft.Extensions.Logging;
using Moq;
using PhotoSi.Command.Product;
using PhotoSi.Interfaces.Mediator;
using PhotoSi.Query.Product.QueryResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebAPI.Controllers;
using Xunit;

namespace PhotoSi.UnitTest.Controller
{
    public class ProductControllerTest
    {
        [Fact]
        public async Task Get_ProductsWithResult_Ok()
        {
            var productsDto = UtilityTest.GenerateProductDtoInDifferenttegoryAsync();

            var _mediatorMock = new Mock<IMediatorService>();
            _mediatorMock.Setup(x => x.Send(It.IsAny<GetAllProductQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(productsDto);

            var _loggerMock = new Mock<ILogger<ProductsController>>();

            var _mockMapper = new Mock<IMapper>();
            //mockMapper.Setup(x => x.Map<DomainItem, ServiceItem>(It.IsAny<DomainItem>()))
            //.Returns(expected);



            var productsController = new ProductsController(_loggerMock.Object, _mediatorMock.Object, _mockMapper.Object);
            var actionResult = await productsController.GetProducts();



            var viewResult = Assert.IsType<Microsoft.AspNetCore.Mvc.OkObjectResult>(actionResult);
            Assert.IsAssignableFrom<List<ProductDto>>(viewResult.Value);
        }

        [Fact]
        public async Task Get_ProductsWithoutResult_Ok()
        {
            var productsDto = UtilityTest.GenerateProductDtoInDifferenttegoryAsync();

            var _mediatorMock = new Mock<IMediatorService>();
            _mediatorMock.Setup(x => x.Send(It.IsAny<GetAllProductQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(productsDto);

            var _loggerMock = new Mock<ILogger<ProductsController>>();

            var _mockMapper = new Mock<IMapper>();



            var productsController = new ProductsController(_loggerMock.Object, _mediatorMock.Object, _mockMapper.Object);
            var actionResult = await productsController.GetProducts();



            var viewResult = Assert.IsType<Microsoft.AspNetCore.Mvc.OkObjectResult>(actionResult);
            Assert.IsAssignableFrom<List<ProductDto>>(viewResult.Value);
        }

        [Fact]
        public async Task Get_ProductSingleWithResult_Ok()
        {
            var productDto = UtilityTest.GenerateProductDtoInDifferenttegoryAsync().First();

            var modelView = new ProductModelView
            {
                ProductId = productDto.ProductId,
                CategoryId = productDto.CategoryId,
                Name = productDto.Name,
                CategoryName = "CategoryName",
                Description = "Desc",
                Note = "Note",
                Options = new Dictionary<int, string> { { 1, "Bianco" } }
            };

            var _mediatorMock = new Mock<IMediatorService>();
            _mediatorMock.Setup(x => x.Send(It.IsAny<GetProductByIdQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(modelView);

            var _loggerMock = new Mock<ILogger<ProductsController>>();

            var _mockMapper = new Mock<IMapper>();



            var productsController = new ProductsController(_loggerMock.Object, _mediatorMock.Object, _mockMapper.Object);
            var actionResult = await productsController.GetProduct(1);



            var viewResult = Assert.IsType<Microsoft.AspNetCore.Mvc.OkObjectResult>(actionResult);
            Assert.IsAssignableFrom<ProductModelView>(viewResult.Value);
        }

        [Fact]
        public async Task Get_ProductSingleWithoutResult_NotFound()
        {
            var productDto = UtilityTest.GenerateProductDtoInDifferenttegoryAsync().First();

            var modelView = new ProductModelView
            {
                ProductId = productDto.ProductId,
                CategoryId = productDto.CategoryId,
                Name = productDto.Name,
                CategoryName = "CategoryName",
                Description = "Desc",
                Note = "Note",
                Options = new Dictionary<int, string> { { 1, "Bianco" } }
            };

            var _mediatorMock = new Mock<IMediatorService>();
            _mediatorMock.Setup(x => x.Send(It.IsAny<GetProductByIdQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((ProductModelView)null);

            var _loggerMock = new Mock<ILogger<ProductsController>>();

            var _mockMapper = new Mock<IMapper>();



            var productsController = new ProductsController(_loggerMock.Object, _mediatorMock.Object, _mockMapper.Object);
            var actionResult = await productsController.GetProduct(100);



            var viewResult = Assert.IsType<Microsoft.AspNetCore.Mvc.NotFoundObjectResult>(actionResult);
            Assert.IsAssignableFrom<string>(viewResult.Value);
            Assert.Equal("Product id 100 not found", viewResult.Value);
        }

    }
}
