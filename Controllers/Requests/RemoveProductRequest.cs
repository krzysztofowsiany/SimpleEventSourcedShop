using System;

namespace SimpleShop.Controllers.Requests
{
    public class RemoveProductRequest
    {
        public Guid OrderId { get; set; }

        public Guid ProductId { get; set; }
    }
}