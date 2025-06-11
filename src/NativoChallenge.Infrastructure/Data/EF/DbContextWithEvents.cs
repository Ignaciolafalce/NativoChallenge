using MediatR;
using Microsoft.Extensions.Logging;
using NativoChallenge.Domain.Entities;

namespace NativoChallenge.Infrastructure.Data.EF;

public class DbContextWithEvents
{
    public AppDbContext Context => _context;

    private readonly AppDbContext _context;
    private readonly IPublisher _publisher;
    private readonly ILogger<DbContextWithEvents> _logger;

    public DbContextWithEvents(AppDbContext context, IPublisher publisher, ILogger<DbContextWithEvents> logger)
    {
        _context = context;
        _publisher = publisher;
        _logger = logger;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var result = await _context.SaveChangesAsync(cancellationToken);

        var domainEvents = _context.ChangeTracker
                                    .Entries<IHasDomainEvent>()
                                    .SelectMany(e => e.Entity.DomainEvents)
                                    .Where(e => !e.IsPublished)
                                    .ToArray();

        foreach (var domainEvent in domainEvents)
        {
            domainEvent.IsPublished = true;

            _logger.LogInformation("Publishing domain event: {EventName}", domainEvent.GetType().Name);

            await _publisher.Publish(domainEvent, cancellationToken);
        }

        return result;
    }
}
