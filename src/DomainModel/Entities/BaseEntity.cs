using DomainModel.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainModel.Entities
{
    public class BaseEntity 
    {
        public virtual long? CreatorUserId { get; set; }
        public virtual DateTime CreationTime { get; set; }
        public virtual long? LastModifierUserId { get; set; }
        public virtual DateTime? LastModificationTime { get; set; }

        private List<INotification> _domainEvents;
        public IReadOnlyCollection<INotification> DomainEvents => _domainEvents?.AsReadOnly();

        private List<IPublicEvent> _publicDomainEvents;
        public IReadOnlyCollection<IPublicEvent> PublicDomainEvents => _publicDomainEvents?.AsReadOnly();

        public void AddDomainEvent(INotification eventItem)
        {
            _domainEvents = _domainEvents ?? new List<INotification>();
            _domainEvents.Add(eventItem);
        }

        public void RemoveDomainEvent(INotification eventItem)
        {
            _domainEvents?.Remove(eventItem);
        }

        public void AddDomainEvent(IPublicEvent eventItem)
        {
            _publicDomainEvents = _publicDomainEvents ?? new List<IPublicEvent>();
            _publicDomainEvents.Add(eventItem);
        }

        public void RemoveDomainEvent(IPublicEvent eventItem)
        {
            _publicDomainEvents?.Remove(eventItem);
        }

        public void ClearAllDomainEvents()
        {
            _domainEvents?.Clear();
            _publicDomainEvents?.Clear();
        }

        public void ClearDomainEvents()
        {
            _domainEvents?.Clear();
        }

        public void ClearPublicDomainEvents()
        {
            _publicDomainEvents?.Clear();
        }

    }
}
