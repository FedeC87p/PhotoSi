using DB.EFCore.Context;
using DomainModel.Dtos;
using DomainModel.Entities.Products;
using DomainModel.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PhotoSi.Interfaces.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WebAPI.BackgroundService
{
    public class MigratorDBHostedService : IHostedService
    {
        private readonly ILogger<MigratorDBHostedService> _logger;
        private readonly IServiceProvider _serviceProvider;

        public MigratorDBHostedService(ILogger<MigratorDBHostedService> logger,
            IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Migration DB Hosted Service running.");

            using (var scope = _serviceProvider.CreateScope())
            {
                var loggerFactory = scope.ServiceProvider.GetRequiredService<ILoggerFactory>();
                var logger = loggerFactory.CreateLogger<MigratorDBHostedService>();
                var databaseConfig = scope.ServiceProvider.GetRequiredService<IOptionsSnapshot<DatabaseConfig>>();


                if (databaseConfig.Value.UseMigrationScript)
                {
                    logger.LogDebug("Run Migrate DatabaseContext");
                    var myDbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                    await myDbContext.Database.MigrateAsync();
                }

                try
                {
                    logger.LogDebug("Seed Database Data");
                    var productRepository = scope.ServiceProvider.GetRequiredService<IRepository<Product>>();
                    var categoryRepository = scope.ServiceProvider.GetRequiredService<IRepository<Category>>();
                    var optionRepository = scope.ServiceProvider.GetRequiredService<IRepository<Option>>();

                    await createProductAsync(productRepository);
                    await createCategoryAsync(categoryRepository);
                    await createOptionAsync(optionRepository);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred seeding the DB.");
                }
            }

            _logger.LogDebug("Migration DB END.");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private async Task createProductAsync(IRepository<Product> repository)
        {
            var dto = new ProductDto
            {
                Name = "Product1",
                Description = "Desc1",
                CategoryId = 1
            };
            repository.Add( (await Product.CreateProductAsync(dto, null)).ValidatedObject );
            await repository.SaveChangeAsync(); //Fatto ogni volta per garantirmi l'id crescente

            dto = new ProductDto
            {
                Name = "Product2",
                Description = "Desc2",
                CategoryId = 2
            };
            repository.Add((await Product.CreateProductAsync(dto, null)).ValidatedObject);
            await repository.SaveChangeAsync(); //Fatto ogni volta per garantirmi l'id crescente

            dto = new ProductDto
            {
                Name = "Product3",
                Description = "Desc3",
                CategoryId = 3
            };
            repository.Add((await Product.CreateProductAsync(dto, null)).ValidatedObject);
            await repository.SaveChangeAsync(); //Fatto ogni volta per garantirmi l'id crescente

            dto = new ProductDto
            {
                Name = "Product4",
                Description = "Desc4",
                CategoryId = 1
            };
            repository.Add((await Product.CreateProductAsync(dto, null)).ValidatedObject);
            await repository.SaveChangeAsync(); //Fatto ogni volta per garantirmi l'id crescente

            dto = new ProductDto
            {
                Name = "Product5",
                Description = "Desc5",
                CategoryId = 2
            };
            repository.Add((await Product.CreateProductAsync(dto, null)).ValidatedObject);
            await repository.SaveChangeAsync(); //Fatto ogni volta per garantirmi l'id crescente

            dto = new ProductDto
            {
                Name = "Product6",
                Description = "Desc6",
                CategoryId = null
            };
            repository.Add((await Product.CreateProductAsync(dto, null)).ValidatedObject);
            await repository.SaveChangeAsync(); //Fatto ogni volta per garantirmi l'id crescente
        }

        private async Task createCategoryAsync(IRepository<Category> repository)
        {
            var dto = new CategoryDto
            {
                Name = "Cat1",
                Description = "DescCat1",
                CategoryId = 1
            };
            repository.Add((await Category.CreateCategoryAsync(dto, null)).ValidatedObject);
            await repository.SaveChangeAsync(); //Fatto ogni volta per garantirmi l'id crescente

            dto = new CategoryDto
            {
                Name = "Cat2",
                Description = "DescCat2",
                CategoryId = 2
            };
            repository.Add((await Category.CreateCategoryAsync(dto, null)).ValidatedObject);
            await repository.SaveChangeAsync(); //Fatto ogni volta per garantirmi l'id crescente

            dto = new CategoryDto
            {
                Name = "Cat3",
                Description = "DescCat3",
                CategoryId = 3
            };
            repository.Add((await Category.CreateCategoryAsync(dto, null)).ValidatedObject);
            await repository.SaveChangeAsync(); //Fatto ogni volta per garantirmi l'id crescente
        }

        private async Task createOptionAsync(IRepository<Option> repository)
        {
            var dto = new OptionDto
            {
                Name = "Option1",
                Description = "DescOption1"
            };
            repository.Add((await Option.CreateOptionAsync(dto, null)).ValidatedObject);
            await repository.SaveChangeAsync(); //Fatto ogni volta per garantirmi l'id crescente

            dto = new OptionDto
            {
                Name = "Option2",
                Description = "DescOption2"
            };
            repository.Add((await Option.CreateOptionAsync(dto, null)).ValidatedObject);
            await repository.SaveChangeAsync(); //Fatto ogni volta per garantirmi l'id crescente

            dto = new OptionDto
            {
                Name = "Option3",
                Description = "DescOption3"
            };
            repository.Add((await Option.CreateOptionAsync(dto, null)).ValidatedObject);
            await repository.SaveChangeAsync(); //Fatto ogni volta per garantirmi l'id crescente

            dto = new OptionDto
            {
                Name = "Option4",
                Description = "DescOption4"
            };
            repository.Add((await Option.CreateOptionAsync(dto, null)).ValidatedObject);
            await repository.SaveChangeAsync(); //Fatto ogni volta per garantirmi l'id crescente

            dto = new OptionDto
            {
                Name = "Option5",
                Description = "DescOption5"
            };
            repository.Add((await Option.CreateOptionAsync(dto, null)).ValidatedObject);
            await repository.SaveChangeAsync(); //Fatto ogni volta per garantirmi l'id crescente
        }
    }
}
