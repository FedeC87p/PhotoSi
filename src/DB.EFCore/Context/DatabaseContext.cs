using System;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using DomainModel.Entities;
using DomainModel.Entities.Orders;
using DomainModel.Entities.Products;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace DB.EFCore.Context
{
    public class DatabaseContext : DbContext
    {
        private readonly IMediator _mediator;

        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DatabaseContext(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public override int SaveChanges()
        {
            var result = base.SaveChanges();
            return result;
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var result = await base.SaveChangesAsync(cancellationToken);
            return result;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default,
            bool dispatchDomainEvent = true)
        {
            var entities = ChangeTracker.Entries<BaseEntity>()
                .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any())
                .ToList();

            if (dispatchDomainEvent) await _mediator.DispatchDomainEventsAsync(entities);

            var result = await base.SaveChangesAsync(cancellationToken);

            if (dispatchDomainEvent) await _mediator.DispatchPublicDomainEventsAsync(entities);

            return result;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

    }
}