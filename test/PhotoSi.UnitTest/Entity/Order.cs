using DomainModel.Dtos;
using DomainModel.Interfaces;
using DomainModel.Specifications.Query;
using DomainModel.Specifications.Rules;
using Moq;
using SpecificationPattern.Rules;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PhotoSi.UnitTest.Entity
{
    public class Order
    {
        [Fact]
        public async Task Create_WithCorrectRules_Ok()
        {
            var productsMok = (IReadOnlyList<DomainModel.Entities.Products.Product>)(await UtilityTest.GenerateProductInSameCategoryAsync()); 
            
            var productRepositoryMock = new Mock<IRepository<DomainModel.Entities.Products.Product>>();
            productRepositoryMock
                .Setup(x => x.FindAsync(It.IsAny<IQuerySpecification<DomainModel.Entities.Products.Product>>()))
                .ReturnsAsync(productsMok);


            var rules = new List<IRuleSpecification<OrderDto>>();
            rules.Add(new OrderAcceptOnlyForProductInSameCategorySpecification(productRepositoryMock.Object));

            var orderDto = new OrderDto
            {
                Code = "12345",
                ProductItems = new List<OrderItemDto>
              {
                  new OrderItemDto {
                      Name = "Pantanoni Lana",
                      ProductId = 1,
                      OptionItems = new List<OrderItemOptionDto> {
                          new OrderItemOptionDto { OptionId = 1, Name = "Colore", Value = "Blu" }
                      }
                  },
                  new OrderItemDto {
                      Name = "Camicia estiva",
                      ProductId = 3,
                      OptionItems = new List<OrderItemOptionDto> {
                          new OrderItemOptionDto { OptionId = 1, Name = "Colore", Value = "Celeste" },
                          new OrderItemOptionDto { OptionId = 2, Name = "Taglia", Value = "XL" }
                      }
                  }
              }
            };

            var orderValidator = await DomainModel.Entities.Orders.Order.CreateOrderAsync(orderDto, rules);
            
            Assert.True(orderValidator.IsValid);
            Assert.NotNull(orderValidator.ValidatedObject);
            Assert.Equal(orderDto.Code, orderValidator.ValidatedObject.Code);
            Assert.Equal(orderDto.ProductItems.Count, orderValidator.ValidatedObject.OrderItems.Count);
            Assert.Contains(orderDto.ProductItems[0].Name, orderValidator.ValidatedObject.OrderItems.Select(i=>i.Name));
            Assert.Contains(orderDto.ProductItems[1].Name, orderValidator.ValidatedObject.OrderItems.Select(i => i.Name));
        }

        [Fact]
        public async Task Create_WithInvalidRules_NullObject()
        {
            var productsMok = (IReadOnlyList<DomainModel.Entities.Products.Product>)(await UtilityTest.GenerateProductInDifferenttegoryAsync());

            var productRepositoryMock = new Mock<IRepository<DomainModel.Entities.Products.Product>>();
            productRepositoryMock
                .Setup(x => x.FindAsync(It.IsAny<IQuerySpecification<DomainModel.Entities.Products.Product>>()))
                .ReturnsAsync(productsMok);


            var rules = new List<IRuleSpecification<OrderDto>>();
            rules.Add(new OrderAcceptOnlyForProductInSameCategorySpecification(productRepositoryMock.Object));

            var orderDto = new OrderDto
            {
                Code = "12345",
                ProductItems = new List<OrderItemDto>
              {
                  new OrderItemDto {
                      Name = "Pantanoni Lana",
                      ProductId = 1,
                      OptionItems = new List<OrderItemOptionDto> {
                          new OrderItemOptionDto { OptionId = 1, Name = "Colore", Value = "Blu" }
                      }
                  },
                  new OrderItemDto {
                      Name = "Camicia estiva",
                      ProductId = 3,
                      OptionItems = new List<OrderItemOptionDto> {
                          new OrderItemOptionDto { OptionId = 1, Name = "Colore", Value = "Celeste" },
                          new OrderItemOptionDto { OptionId = 2, Name = "Taglia", Value = "XL" }
                      }
                  }
              }
            };

            var orderValidator = await DomainModel.Entities.Orders.Order.CreateOrderAsync(orderDto, rules);
            Assert.False(orderValidator.IsValid);
            Assert.Null(orderValidator.ValidatedObject);
        }

        [Fact]
        public async Task Create_WithEmptyProduct_NullObject()
        {
            var orderDto = new OrderDto
            {
                Code = "12345"
            };

            var orderValidator = await DomainModel.Entities.Orders.Order.CreateOrderAsync(orderDto, null);
            Assert.False(orderValidator.IsValid);
            Assert.Null(orderValidator.ValidatedObject);
        }

    }
}
