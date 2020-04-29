using System;

namespace SimpleShop.Controllers.Events
{
    public class ClientApplied : IEvent
    {
        public ClientApplied(Guid clientId)
        {
            ClientId = clientId;
        }

        public Guid ClientId { get; }
    }
}