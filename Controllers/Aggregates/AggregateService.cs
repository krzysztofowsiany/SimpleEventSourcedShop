using System;
using System.Linq;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using SimpleEventSourcedShop.Controllers.Extensions;

namespace SimpleShop.Controllers.Aggregates
{
    public class AggregateService
    {

        private EventStoreBuilder _eventStoreBuilder;

        public AggregateService()
        {
            _eventStoreBuilder = new EventStoreBuilder();
        }

        public async Task SaveAggregate<TAggregate>(TAggregate aggregate, long version)
            where TAggregate : AggregateRoot, new()
        {
            var eventStoreConnection = _eventStoreBuilder.GetConnection();

            var events = aggregate.Events
                .Select(e => new EventData(Guid.NewGuid(), e.GetType().Name, true, e.Serialize(), null))
                .ToArray();

            aggregate.ClearEvents();

            await eventStoreConnection.AppendToStreamAsync(aggregate.Id.ToString(), version, events);
        }

        public async Task<(long, TAggregate)> GetAggregate<TAggregate>(string id)
            where TAggregate : AggregateRoot, new()
        {
            var eventStoreConnection = _eventStoreBuilder.GetConnection();

            var events = await eventStoreConnection.ReadStreamEventsForwardAsync(id, 0, 1000, true);

            return await ApplyEvents<TAggregate>(events);
        }

        public async Task<(long, TAggregate)> ApplyEvents<TAggregate>(StreamEventsSlice events)
            where TAggregate : AggregateRoot, new()
        {
            return await Task.Run(() =>
            {
                var version = ExpectedVersion.NoStream;
                var aggregate = new TAggregate();

                foreach (var @event in events.Events)
                {
                    var e = @event.Deserialize();
                    aggregate.ApplyEvent(e);
                    version++;
                }

                return (version, aggregate);
            });
        }
    }
}