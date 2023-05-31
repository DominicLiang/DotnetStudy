using MediatR;
using Microsoft.EntityFrameworkCore;

namespace _16_MediatR;

public class MyDbContext : DbContext
{
    public DbSet<User> Users { get; set; }

    private readonly IMediator mediator;

    public MyDbContext(DbContextOptions<MyDbContext> opt, IMediator mediator) : base(opt)
    {
        this.mediator = mediator;
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var domainEntities = ChangeTracker.Entries<IDomainEvents>().Where(e => e.Entity.GetDomainEvents().Any());

        var domainEvents = domainEntities.SelectMany(e => e.Entity.GetDomainEvents()).ToList();
        domainEntities.ToList().ForEach(e => e.Entity.ClearDomainEvents());

        domainEvents.ForEach(e => mediator.Publish(e));

        return base.SaveChangesAsync(cancellationToken);
    }
}
