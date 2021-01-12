using DomainModel.Dtos;
using DomainModel.Events;
using DomainModel.Specifications.Rules;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using PhotoSi.Command.Product;
using PhotoSi.Interfaces.Mediator;
using PhotoSi.Subcribers;
using SpecificationPattern.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using static PhotoSi.Interfaces.Configuration.EntityConfig;

namespace PhotoSi.AC.Modules
{
    public class ApplicationCore
    {
        public static void ConfigureValidationRules(IServiceCollection services, ValidationRulesConfig validationRulesConfig)
        {
            if (validationRulesConfig == null)
            {
                return;
            }

            //Product
            if (validationRulesConfig.Product == null || //Default Rule
                    validationRulesConfig.Product.Any(i => i.Equals("UniqueName", StringComparison.InvariantCultureIgnoreCase)))
            {
                services.AddScoped<IRuleSpecification<ProductDto>, ProductUniqueNameSpecification>();
            }
            if (validationRulesConfig.Product != null &&
                    validationRulesConfig.Product.Any(i => i.Equals("NameMaxLength50", StringComparison.InvariantCultureIgnoreCase)))
            {
                services.AddScoped<IRuleSpecification<ProductDto>, ProductNameMax50Specification>();
            }
            //Category
            if (validationRulesConfig.Category == null || //Default Rule
                    validationRulesConfig.Category.Any(i => i.Equals("UniqueName", StringComparison.InvariantCultureIgnoreCase)))
            {
                services.AddScoped<IRuleSpecification<CategoryDto>, CategoryUniqueNameSpecification>();
            }
            //Option
            if (validationRulesConfig.Option == null || //Default Rule
                    validationRulesConfig.Option.Any(i => i.Equals("UniqueName", StringComparison.InvariantCultureIgnoreCase)))
            {
                services.AddScoped<IRuleSpecification<OptionDto>, OptionUniqueNameSpecification>();
            }

            //Order
            //services.AddScoped<IRuleSpecification<OrderDto>, OrderAcceptOnlyOneProductForCategorySpecification>(); //Usata per fare il contrario della regola successiva
            services.AddScoped<IRuleSpecification<OrderDto>, OrderAcceptOnlyForProductInSameCategorySpecification>();
            
        }

        public static void ConfigureApplicationCore(IServiceCollection services)
        {
            //CQRS
            List<Assembly> assemblies = new List<Assembly>
            {
                typeof(OrderConfirmedPublicEvent).GetTypeInfo().Assembly, //Event
                typeof(GetAllProductQuery).GetTypeInfo().Assembly, //Query
                typeof(CreateProductCommand).GetTypeInfo().Assembly, //Command
                typeof(OrderCreatedHandler).GetTypeInfo().Assembly //Subcribers
            };
            services.AddMediatR(assemblies.Distinct().ToArray());

            services.AddScoped<IMediatorService, MediatorService>();
        }
    }
}
