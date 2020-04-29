using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using Newtonsoft.Json;
using SimpleShop.Controllers.Events;

namespace SimpleShop.Controllers
{
    public class AggregateService
    {
        UTF8Encoding Encoding = new UTF8Encoding(false);

        public async Task SaveAggregate<TAggregate>(TAggregate aggregate, long version)
            where TAggregate : AggregateRoot, new()
        {
            var eventStoreConnection = new EventStoreBuilder().Create();
            await eventStoreConnection.ConnectAsync();

            var events = aggregate.Events
                .Select(e => new EventData(Guid.NewGuid(), e.GetType().FullName, true, Serialize(e), null))
                .ToArray();

            aggregate.ClearEvents();

            await eventStoreConnection.AppendToStreamAsync(aggregate.Id.ToString(), version, events);
        }

        public async Task<(long, TAggregate)> GetAggregate<TAggregate>(string id)
            where TAggregate : AggregateRoot, new()
        {
            var eventStoreConnection = new EventStoreBuilder().Create();
            await eventStoreConnection.ConnectAsync();

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
                    var e = Deserialize(@event);
                    aggregate.ApplyEvent(e);
                    version++;
                }

                return (version, aggregate);
            });
        }

        private IEvent Deserialize(ResolvedEvent @event)
        {
            var data = Encoding.GetString(@event.Event.Data);
            var type = Type.GetType(@event.Event.EventType);

            return (IEvent) JsonConvert.DeserializeObject(data, type);
        }

        private byte[] Serialize(IEvent e) => Encoding.GetBytes(JsonConvert.SerializeObject(e));
    }
}