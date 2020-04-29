using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using Newtonsoft.Json;
using SimpleEventSourcedShop.Controllers.Extensions;
using SimpleEventSourcedShop.Controllers.Projections;
using SimpleShop.Controllers.Events;

namespace SimpleShop.Controllers
{
    public class ProjectionService
    {
        UTF8Encoding Encoding = new UTF8Encoding(false);
        private EventStoreBuilder _eventStoreBuilder;

        public ProjectionService()
        {
            _eventStoreBuilder = new EventStoreBuilder();
        }

        public async Task<TProjection> GetProjection<TProjection>(string id)
            where TProjection : Projection, new()
        {
            var eventStoreConnection = _eventStoreBuilder.GetConnection();

            var events = await eventStoreConnection.ReadStreamEventsForwardAsync(id, 0, 1000, true);

            return await Hydrate<TProjection>(events);
        }

        public async Task<TProjection> Hydrate<TProjection>(StreamEventsSlice events)
            where TProjection : Projection, new()
        {
            return await Task.Run(() =>
            {
                var projetion = new TProjection();

                foreach (var @event in events.Events)
                {
                    var e = @event.Deserialize();
                    projetion.ApplayEvent(e);
                }

                return projetion;
            });
        }
    }
}