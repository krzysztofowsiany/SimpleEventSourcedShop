﻿using System;
using System.Net;
using EventStore.ClientAPI;
using EventStore.ClientAPI.SystemData;
using System.Threading.Tasks;
using EventStore.ClientAPI.Common.Log;
using EventStore.ClientAPI.Projections;

namespace SimpleShop
{
    public class EventStoreBuilder
    {
        private readonly UserCredentials _credentials;
        private readonly ConnectionSettings _settings;
        private IEventStoreConnection _connection;
        private ProjectionsManager _projectionsManager;

        public EventStoreBuilder()
        {
            _credentials = new UserCredentials("ops", "ops");

            _settings = ConnectionSettings.Create()
                .SetDefaultUserCredentials(_credentials)
                .Build();

            _projectionsManager = new ProjectionsManager(new ConsoleLogger(),
                    new IPEndPoint(IPAddress.Parse("127.0.0.1"), 2113),
                    TimeSpan.FromMilliseconds(5000));

            _connection = EventStoreConnection.Create(_settings, new IPEndPoint(IPAddress.Parse("127.0.0.1"), 1113));
            _connection.ConnectAsync();
        }

        public IEventStoreConnection GetConnection()
        {
            return _connection;
        }

        public async Task<string> GetProjection(string projectionName)
        {
            return await _projectionsManager.GetStateAsync(projectionName, _credentials);
        }
    }
}