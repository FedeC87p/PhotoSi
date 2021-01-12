using DomainModel.Dtos;
using DomainModel.Specifications.Rules;
using SpecificationPattern.Rules;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace PhotoSi.UnitTest.Entity
{
    public class Product
    {
        [Fact]
        public async Task Create_WithCorrectRules_Ok()
        {
            var rules = new List<IRuleSpecification<ProductDto>>();
            rules.Add(new ProductNameMax50Specification());
            var productDto = new ProductDto
            {
                Name = "Maglia",
                Description = "Desc x Maglia",
                Note = "Note x maglia",
                CategoryId = 1,
                OptionsId = new List<int> { 1, 2, 3 }
            };

            var productValidator = await DomainModel.Entities.Products.Product.CreateProductAsync(productDto, rules);

            Assert.True(productValidator.IsValid);
            Assert.NotNull(productValidator.ValidatedObject);
            Assert.Equal(productDto.Name, productValidator.ValidatedObject.Name);
            Assert.Equal(productDto.Description, productValidator.ValidatedObject.Description);
            Assert.Equal(productDto.Note, productValidator.ValidatedObject.Note);
        }

        [Fact]
        public async Task Create_WithCorrectEmptyRules_Ok()
        {
            var productDto = new ProductDto
            {
                Name = "Maglia",
                Description = "Desc x Maglia",
                Note = "Note x maglia",
                CategoryId = 1,
                OptionsId = new List<int> { 1, 2, 3 }
            };

            var productValidator = await DomainModel.Entities.Products.Product.CreateProductAsync(productDto, null);

            Assert.True(productValidator.IsValid);
            Assert.NotNull(productValidator.ValidatedObject);
            Assert.Equal(productDto.Name, productValidator.ValidatedObject.Name);
            Assert.Equal(productDto.Description, productValidator.ValidatedObject.Description);
            Assert.Equal(productDto.Note, productValidator.ValidatedObject.Note);
        }

        [Fact]
        public async Task Create_WithInCorrectRules_NullObject()
        {
            var rules = new List<IRuleSpecification<ProductDto>>();
            rules.Add(new ProductNameMax50Specification());
            var productDto = new ProductDto
            {
                Name = "12345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901",
                Description = "Desc x Maglia",
                Note = "Note x maglia",
                CategoryId = 1,
                OptionsId = new List<int> { 1, 2, 3 }
            };

            var productValidator = await DomainModel.Entities.Products.Product.CreateProductAsync(productDto, rules);

            Assert.False(productValidator.IsValid);
            Assert.Null(productValidator.ValidatedObject);
        }
    }
}
