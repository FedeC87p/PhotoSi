using MediatR;
using Microsoft.Extensions.DependencyInjection;
using PhotoSi.Command.Product;
using PhotoSi.Interfaces.Mediator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PhotoSi.AC.Modules
{
    public class ApplicationCore
    {
        public static void ConfigureApplicationCore(IServiceCollection services)
        {
            //CQRS
            List<Assembly> assemblies = new List<Assembly>
            {
                //typeof(NodeEndPointReferenceChangedPublicEvent).GetTypeInfo().Assembly, //DomainEvent
                //typeof(NodeByIdQuery).GetTypeInfo().Assembly, //Query
                typeof(CreateProductCommand).GetTypeInfo().Assembly, //Command
                //typeof(NodeEndPointReferenceChangedHandler).GetTypeInfo().Assembly //Subcribers
            };
            services.AddMediatR(assemblies.Distinct().ToArray());

            services.AddScoped<IMediatorService, MediatorService>();
        }
    }
}
