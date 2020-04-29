using SimpleShop.Controllers.Events;

namespace SimpleEventSourcedShop.Controllers.Projections
{
    public abstract class Projection
    {
        public abstract void ApplayEvent(IEvent e);
    }
}