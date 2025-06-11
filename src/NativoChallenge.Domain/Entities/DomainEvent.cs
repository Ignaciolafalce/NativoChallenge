using MediatR;

namespace NativoChallenge.Domain.Entities
{
    public class DomainEvent : INotification
    {
        public bool IsPublished { get; set; }
        public DateTimeOffset DateOcurred { get; set; } = DateTime.UtcNow;

        protected DomainEvent()
        {
            DateOcurred = DateTime.UtcNow;
        }
    }

}
