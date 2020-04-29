﻿using System;
using System.Net;
using EventStore.ClientAPI;
using EventStore.ClientAPI.SystemData;

namespace SimpleShop
{
    public class EventStoreBuilder
    {
        private readonly ConnectionSettings _settings;
        private IEventStoreConnection _connection;

        public EventStoreBuilder()
        {
            var credentials = new UserCredentials("ops", "ops");
           
            _settings = ConnectionSettings.Create()
                .SetDefaultUserCredentials(credentials)
                .Build();
        }

        public IEventStoreConnection Create()
        {
            return _connection ?? (_connection =
                EventStoreConnection.Create(_settings, new IPEndPoint(IPAddress.Parse("127.0.0.1"), 1113)));
        }
    }
}