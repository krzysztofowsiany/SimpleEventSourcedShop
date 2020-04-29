using System;

namespace SimpleShop.Controllers.Requests
{
    public class AddProductRequest
    {
        public Guid OrderId { get; set; }

        public Guid ProductId { get; set; }
    }
}