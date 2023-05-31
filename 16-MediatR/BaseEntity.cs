using MediatR;
using System.ComponentModel.DataAnnotations.Schema;

namespace _16_MediatR;

public class BaseEntity : IDomainEvents
{
    [NotMapped]
    private readonly IList<INotification> events= new List<INotification>();

    public void AddDomainEvent(INotification notification)
    {
        events.Add(notification);
    }

    public void ClearDomainEvents()
    {
        events.Clear();
    }

    public IEnumerable<INotification> GetDomainEvents()
    {
        return events;
    }
}
