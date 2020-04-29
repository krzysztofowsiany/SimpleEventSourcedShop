using System;

namespace SimpleShop.Controllers.Events
{
    public class OrderCreated : IEvent
    {
        public OrderCreated(Guid orderId)
        {
            OrderId = orderId;
        }

        public Guid OrderId { get; }
    }
}