using System;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace DB.EFCore.Context
{
    public class DatabaseContext : DbContext
    {
        private readonly IMediator _mediator;
        private IDbContextTransaction _currentTransaction;

        public DatabaseContext(DbContextOptions<DatabaseContext> options, IMediator mediator)
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public bool HasActiveTransaction => _currentTransaction != null;

        public DbSet<Hub> Hubs { get; set; }
        public DbSet<Node> Nodes { get; set; }
        public DbSet<ViewTemplate> ViewTemplates { get; set; }
        public DbSet<TransatableItem> TransatableItem { get; set; }
        public DbSet<UserAudit> UserAuditEvents { get; set; }
        public DbSet<Dashboard> Dashboards { get; set; }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default,
            bool dispatchDomainEvent = true)
        {
            var entities = ChangeTracker.Entries<Entity>()
                .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any())
                .ToList();

            if (dispatchDomainEvent) await _mediator.DispatchDomainEventsAsync(entities);

            var result = await base.SaveChangesAsync(cancellationToken);

            if (dispatchDomainEvent) await _mediator.DispatchPublicDomainEventsAsync(entities);

            return result;
        }

        public IDbContextTransaction GetCurrentTransaction()
        {
            return _currentTransaction;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

    }
}