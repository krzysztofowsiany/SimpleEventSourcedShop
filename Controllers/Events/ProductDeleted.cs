using System;

namespace SimpleShop.Controllers.Events
{
    public class ProductDeleted : IEvent
    {
        public ProductDeleted(Guid productId)
        {
            ProductId = productId;
        }

        public Guid ProductId { get; }
    }
}