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

        private List<IPublicEvent> _integrationEvents;
        public IReadOnlyCollection<IPublicEvent> IntegrationEvents => _integrationEvents?.AsReadOnly();

        public void AddDomainEvent(INotification eventItem)
        {
            _domainEvents = _domainEvents ?? new List<INotification>();
            _domainEvents.Add(eventItem);
        }

        public void RemoveDomainEvent(INotification eventItem)
        {
            _domainEvents?.Remove(eventItem);
        }

        public void AddIntegrationEvent(IPublicEvent eventItem)
        {
            _integrationEvents = _integrationEvents ?? new List<IPublicEvent>();
            _integrationEvents.Add(eventItem);
        }

        public void RemoveIntegrationEvent(IPublicEvent eventItem)
        {
            _integrationEvents?.Remove(eventItem);
        }

        public void ClearAllDomainEvents()
        {
            _domainEvents?.Clear();
            _integrationEvents?.Clear();
        }

        public void ClearDomainEvents()
        {
            _domainEvents?.Clear();
        }

        public void ClearPublicDomainEvents()
        {
            _integrationEvents?.Clear();
        }

    }
}
