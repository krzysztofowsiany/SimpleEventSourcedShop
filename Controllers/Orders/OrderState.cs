using System;
using System.Collections.Generic;

namespace SimpleShop.Controllers.Orders
{
    public class OrderState
    {
        public Guid ClientId;

        public ISet<Guid> Products = new HashSet<Guid>();

        public bool Confirmed;
    }
}