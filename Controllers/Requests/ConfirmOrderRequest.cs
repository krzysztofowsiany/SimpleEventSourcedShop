using System;

namespace SimpleShop.Controllers.Requests
{
    public class ConfirmOrderRequest
    {
        public Guid OrderId { get; set; }
    }
}