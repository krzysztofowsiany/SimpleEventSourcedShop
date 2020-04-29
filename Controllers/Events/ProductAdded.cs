using System;

namespace SimpleShop.Controllers.Events
{
    public class ProductAdded : IEvent
    {
        public ProductAdded(Guid productId)
        {
            ProductId = productId;
        }

        public Guid ProductId { get; }
    }
}