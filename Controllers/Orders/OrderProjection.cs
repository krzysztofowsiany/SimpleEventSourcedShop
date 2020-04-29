using System;
using System.Collections.Generic;
using SimpleShop.Controllers.Events;

namespace SimpleEventSourcedShop.Controllers.Projections
{
    public class OrderProjection : Projection
    {
        public Guid ClientId { get; private set; }
        public Guid OrderId { get; private set; }
        public ISet<Guid> Products { get; private set; }

        public bool Confirmed { get; private set; }

        public OrderProjection()
        {
            Products = new HashSet<Guid>();
        }

        public override void ApplayEvent(IEvent e)
        {
            switch (e)
            {
                case OrderCreated @event:
                    Apply(@event);
                    break;

                case ProductAdded @event:
                    Apply(@event);
                    break;

                case ProductDeleted @event:
                    Apply(@event);
                    break;

                case OrderConfirmed @event:
                    Apply(@event);
                    break;

                case ClientApplied @event:
                    Apply(@event);
                    break;
            }
        }

        private void Apply(OrderCreated @event)
        {
            OrderId = @event.OrderId;
        }

        private void Apply(ProductAdded @event)
        {
            Products.Add(@event.ProductId);
        }

        private void Apply(ProductDeleted @event)
        {
            if (Products.Contains(@event.ProductId))
                Products.Remove(@event.ProductId);
        }

        private void Apply(OrderConfirmed @event)
        {
            Confirmed = true;
        }

        private void Apply(ClientApplied @event)
        {
            ClientId = @event.ClientId;
        }
    }
}