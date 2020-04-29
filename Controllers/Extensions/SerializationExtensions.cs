using System;
using System.Linq;
using System.Text;
using EventStore.ClientAPI;
using Newtonsoft.Json;
using SimpleShop.Controllers.Events;

namespace SimpleEventSourcedShop.Controllers.Extensions
{
    public static class SerializationExtensions
    {
        static UTF8Encoding Encoding = new UTF8Encoding(false);

        static public IEvent Deserialize(this ResolvedEvent @event)
        {
            var data = Encoding.GetString(@event.Event.Data);
            var type = FindType(@event.Event.EventType);

            return (IEvent)JsonConvert.DeserializeObject(data, type);
        }

        static public byte[] Serialize(this IEvent e)
            => Encoding.GetBytes(JsonConvert.SerializeObject(e));

        private static Type FindType(string name)
            =>
                AppDomain.CurrentDomain.GetAssemblies()
                    .Where(a => !a.IsDynamic)
                    .SelectMany(a => a.GetTypes())
                    .FirstOrDefault(t => t.Name.Equals(name));
    }
}