using System;
using System.Collections.Generic;
using SimpleShop.Controllers.Events;

namespace SimpleShop.Controllers.Aggregates
{
    public abstract class AggregateRoot
    {
        public IEnumerable<IEvent> Events => _events;

        ISet<IEvent> _events = new HashSet<IEvent>();
        public Guid Id { get; set; }
        public void ClearEvents()
        {
            _events.Clear();
        }

        protected void AddEvent(IEvent e) =>
            _events.Add(e);

        public abstract void ApplyEvent(IEvent e);
    }
}