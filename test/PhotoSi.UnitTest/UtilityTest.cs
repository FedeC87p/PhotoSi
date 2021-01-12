using DomainModel.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoSi.UnitTest
{
    static public class UtilityTest
    {
        public static async Task<List<DomainModel.Entities.Products.Product>> GenerateProductInSameCategoryAsync()
        {
            var products = new List<DomainModel.Entities.Products.Product>();
            for (var i = 1; i < 5; i++)
            {
                var productDto = new ProductDto
                {
                    Name = $"Maglia{i}",
                    Description = "Desc x Maglia{i}",
                    Note = "Note x maglia{i}",
                    CategoryId = 1,
                    OptionsId = new List<int> { 1, 2, 3 }
                };

                var productValidator = await DomainModel.Entities.Products.Product.CreateProductAsync(productDto, null);
                products.Add(productValidator.ValidatedObject);
            }
            return products;
        }

        public static async Task<List<DomainModel.Entities.Products.Product>> GenerateProductInDifferenttegoryAsync()
        {
            var products = new List<DomainModel.Entities.Products.Product>();
            for (var i = 1; i < 5; i++)
            {
                var productDto = new ProductDto
                {
                    Name = $"Maglia{i}",
                    Description = "Desc x Maglia{i}",
                    Note = "Note x maglia{i}",
                    CategoryId = i,
                    OptionsId = new List<int> { 1, 2, 3 }
                };

                var productValidator = await DomainModel.Entities.Products.Product.CreateProductAsync(productDto, null);
                products.Add(productValidator.ValidatedObject);
            }
            return products;
        }

        public static List<ProductDto> GenerateProductDtoInDifferenttegoryAsync()
        {
            var products = new List<ProductDto>();
            for (var i = 1; i < 5; i++)
            {
                var productDto = new ProductDto
                {
                    ProductId = i,
                    Name = $"Maglia{i}",
                    Description = "Desc x Maglia{i}",
                    Note = "Note x maglia{i}",
                    CategoryId = i,
                    OptionsId = new List<int> { 1, 2, 3 }
                };
                products.Add(productDto);
            }
            return products;
        }

    }
}
