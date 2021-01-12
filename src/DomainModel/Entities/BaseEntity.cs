using DomainModel.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainModel.Entities
{
    public class BaseEntity 
    {

        private List<INotification> _domainEvents;
        public IReadOnlyCollection<INotification> DomainEvents => _domainEvents?.AsReadOnly();

        private List<IPublicEvent> _publicEvents;
        public IReadOnlyCollection<IPublicEvent> PublicEvents => _publicEvents?.AsReadOnly();

        public void AddDomainEvent(INotification eventItem)
        {
            _domainEvents = _domainEvents ?? new List<INotification>();
            _domainEvents.Add(eventItem);
        }

        public void RemoveDomainEvent(INotification eventItem)
        {
            _domainEvents?.Remove(eventItem);
        }

        public void AddPublicEvent(IPublicEvent eventItem)
        {
            _publicEvents = _publicEvents ?? new List<IPublicEvent>();
            _publicEvents.Add(eventItem);
        }

        public void RemovePublicEvent(IPublicEvent eventItem)
        {
            _publicEvents?.Remove(eventItem);
        }

        public void ClearAllDomainEvents()
        {
            _domainEvents?.Clear();
            _publicEvents?.Clear();
        }

        public void ClearDomainEvents()
        {
            _domainEvents?.Clear();
        }

        public void ClearPublicDomainEvents()
        {
            _publicEvents?.Clear();
        }

    }
}
