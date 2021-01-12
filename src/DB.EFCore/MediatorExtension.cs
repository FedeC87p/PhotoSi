using DomainModel.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DB.EFCore
{
    internal static class MediatorExtension
    {
        public static async Task DispatchDomainEventsAsync(this IMediator mediator, List<EntityEntry<BaseEntity>> entities)
        {
            var domainEvents = entities
                .SelectMany(x => x.Entity.DomainEvents)
                .ToList();

            entities.ToList()
                .ForEach(entity => entity.Entity.ClearDomainEvents());

            foreach (var domainEvent in domainEvents)
                await mediator.Publish(domainEvent);
        }

        public static async Task DispatchPublicDomainEventsAsync(this IMediator mediator,
            List<EntityEntry<BaseEntity>> entities)
        {
            var publicDomainEvents = entities
                .SelectMany(x => x.Entity.PublicEvents)
                .ToList();

            entities.ToList()
                .ForEach(entity => entity.Entity.ClearPublicDomainEvents());

            foreach (var domainEvent in publicDomainEvents)
                await mediator.Publish(domainEvent);
        }
    }
}
