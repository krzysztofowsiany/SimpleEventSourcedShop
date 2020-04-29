using System;

namespace SimpleShop.Controllers.Requests
{
    public class ApplyClientRequest
    {
        public Guid OrderId { get; set; }

        public Guid ClientId { get; set; }
    }
}