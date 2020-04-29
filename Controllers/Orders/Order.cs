using System;
using SimpleShop.Controllers.Events;

namespace SimpleShop.Controllers.Orders
{
    public class Order : AggregateRoot
    {
        private OrderState _state = new OrderState();
        public OrderState State => _state;

        public override void ApplyEvent(IEvent e)
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

        public Order()
        {
            
        }
        
        public Order(Guid id)
        {
            var e = new OrderCreated(id);
            Apply(e);
            AddEvent(e);
        }

        public void ApplyClient(Guid clientId)
        {
            var e = new ClientApplied(clientId);
            Apply(e);
            AddEvent(e);
        }

        public void AddProduct(Guid productId)
        {
            var e = new ProductAdded(productId);
            Apply(e);
            AddEvent(e);
        }
        
        public void RemoveProduct(Guid productId)
        {            
            var e = new ProductDeleted(productId);
            Apply(e);
            AddEvent(e);
        }
        
        public void Confirm()
        {
            var e = new OrderConfirmed();
            Apply(e);
            AddEvent(e);
        }

        private void Apply(OrderCreated @event)
        {
            Id = @event.OrderId;
        }
        
        private void Apply(ProductAdded @event)
        {
            _state.Products.Add(@event.ProductId);
        }
        
        private void Apply(ProductDeleted @event)
        {
            if (_state.Products.Contains(@event.ProductId))
                _state.Products.Remove(@event.ProductId);
        }
        
        private void Apply(OrderConfirmed @event)
        {
            _state.Confirmed = true;
        }
        
        private void Apply(ClientApplied @event)
        {
            _state.ClientId = @event.ClientId;
        }
    }
}