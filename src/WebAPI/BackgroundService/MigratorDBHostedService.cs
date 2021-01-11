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
                    var productRepository = scope.ServiceProvider.GetRequiredService<IProductRepository>();
                    var categoryRepository = scope.ServiceProvider.GetRequiredService<IRepository<Category>>();
                    var optionRepository = scope.ServiceProvider.GetRequiredService<IRepository<Option>>();

                    await seedDatabaseForProduct(productRepository, categoryRepository, optionRepository);
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

        private async Task seedDatabaseForProduct(IProductRepository repository, IRepository<Category> categoryRepository, IRepository<Option> optionRepository)
        {
            //Per fare presto (essendo un progettino di dimostrazione)
            //Popolo il database nel caos in cui non trovo nessun prodotto
            //Utilizzo direttamente ListAll per fare prima a scrivere il codice anche se sarà piu lento
            var products = await repository.ListAllAsync();
            var category = await categoryRepository.ListAllAsync();
            var option = await optionRepository.ListAllAsync();

            if (products.Count > 0 ||
                category.Count > 0 ||
                option.Count > 0)
            {
                return;
            }



            var categoryDto = new CategoryDto
            {
                Name = "Cat1",
                Description = "DescCat1",
                CategoryId = 1
            };
            var catOne = (await Category.CreateCategoryAsync(categoryDto, null)).ValidatedObject;
            categoryRepository.Add(catOne);
            await categoryRepository.SaveChangeAsync(); //Fatto ogni volta per garantirmi l'id crescente

            categoryDto = new CategoryDto
            {
                Name = "Cat2",
                Description = "DescCat2",
                CategoryId = 2
            };
            var catTwo = (await Category.CreateCategoryAsync(categoryDto, null)).ValidatedObject;
            categoryRepository.Add(catTwo);
            await categoryRepository.SaveChangeAsync(); //Fatto ogni volta per garantirmi l'id crescente

            categoryDto = new CategoryDto
            {
                Name = "Cat3",
                Description = "DescCat3",
                CategoryId = 3
            };
            var catThree = (await Category.CreateCategoryAsync(categoryDto, null)).ValidatedObject;
            categoryRepository.Add(catThree);
            await categoryRepository.SaveChangeAsync(); //Fatto ogni volta per garantirmi l'id crescente


            var dto = new ProductDto
            {
                Name = "Product1",
                Description = "Desc1",
                CategoryId = 1
            };
            var productOne = (await Product.CreateProductAsync(dto, null)).ValidatedObject;
            repository.Add(productOne);
            await repository.SaveChangeAsync(); //Fatto ogni volta per garantirmi l'id crescente

            dto = new ProductDto
            {
                Name = "Product2",
                Description = "Desc2",
                CategoryId = 2
            };
            var productTwo = (await Product.CreateProductAsync(dto, null)).ValidatedObject;
            repository.Add(productTwo);
            await repository.SaveChangeAsync(); //Fatto ogni volta per garantirmi l'id crescente

            dto = new ProductDto
            {
                Name = "Product3",
                Description = "Desc3",
                CategoryId = 3
            };
            var productThree = (await Product.CreateProductAsync(dto, null)).ValidatedObject;
            repository.Add(productThree);
            await repository.SaveChangeAsync(); //Fatto ogni volta per garantirmi l'id crescente

            dto = new ProductDto
            {
                Name = "Product4",
                Description = "Desc4",
                CategoryId = 1
            };
            var productFour = (await Product.CreateProductAsync(dto, null)).ValidatedObject;
            repository.Add(productFour);
            await repository.SaveChangeAsync(); //Fatto ogni volta per garantirmi l'id crescente

            dto = new ProductDto
            {
                Name = "Product5",
                Description = "Desc5",
                CategoryId = 2
            };
            var productFive = (await Product.CreateProductAsync(dto, null)).ValidatedObject;
            repository.Add(productFive);
            await repository.SaveChangeAsync(); //Fatto ogni volta per garantirmi l'id crescente

            dto = new ProductDto
            {
                Name = "Product6",
                Description = "Desc6",
                CategoryId = null
            };
            var productSix = (await Product.CreateProductAsync(dto, null)).ValidatedObject;
            repository.Add(productSix);
            await repository.SaveChangeAsync(); //Fatto ogni volta per garantirmi l'id crescente
        


            var optionDto = new OptionDto
            {
                Name = "Option1",
                Description = "DescOption1"
            };
            var option1 = (await Option.CreateOptionAsync(optionDto, null)).ValidatedObject;
            optionRepository.Add(option1);
            await optionRepository.SaveChangeAsync(); //Fatto ogni volta per garantirmi l'id crescente

            optionDto = new OptionDto
            {
                Name = "Option2",
                Description = "DescOption2"
            };
            var option2 = (await Option.CreateOptionAsync(optionDto, null)).ValidatedObject;
            optionRepository.Add(option2);
            await optionRepository.SaveChangeAsync(); //Fatto ogni volta per garantirmi l'id crescente

            optionDto = new OptionDto
            {
                Name = "Option3",
                Description = "DescOption3"
            };
            var option3 = (await Option.CreateOptionAsync(optionDto, null)).ValidatedObject;
            optionRepository.Add(option3);
            await optionRepository.SaveChangeAsync(); //Fatto ogni volta per garantirmi l'id crescente

            optionDto = new OptionDto
            {
                Name = "Option4",
                Description = "DescOption4"
            };
            var option4 = (await Option.CreateOptionAsync(optionDto, null)).ValidatedObject;
            optionRepository.Add(option4);
            await optionRepository.SaveChangeAsync(); //Fatto ogni volta per garantirmi l'id crescente

            optionDto = new OptionDto
            {
                Name = "Option5",
                Description = "DescOption5"
            };
            var option5 = (await Option.CreateOptionAsync(optionDto, null)).ValidatedObject;
            optionRepository.Add(option5);
            await optionRepository.SaveChangeAsync(); //Fatto ogni volta per garantirmi l'id crescente


            //repository.LinkCategory(productOne, catOne);
            //repository.LinkCategory(productFour, catOne);
            //repository.LinkCategory(productTwo, catTwo);
            //repository.LinkCategory(productFive, catTwo);
            //repository.LinkCategory(productThree, catThree);

            repository.LinkOption(productOne, option1);
            repository.LinkOption(productOne, option2); 
            repository.LinkOption(productOne, option3);

            repository.LinkOption(productTwo, option1);
            repository.LinkOption(productTwo, option2);
            repository.LinkOption(productTwo, option3);

            repository.LinkOption(productThree, option4);

            repository.LinkOption(productFour, option1);
            repository.LinkOption(productFour, option5);

            await optionRepository.SaveChangeAsync();
        }
    }
}
