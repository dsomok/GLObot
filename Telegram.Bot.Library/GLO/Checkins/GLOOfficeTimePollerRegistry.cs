using System;
using System.Collections.Concurrent;

namespace Telegram.Bot.Library.GLO.Checkins
{
    internal class GloOfficeTimePollerRegistry
    {
        private readonly TimeSpan _pollingPeriod;
        private readonly GloOfficeTimeClient _client;

        private readonly ConcurrentDictionary<int, GloOfficeTimePoller> _registry = new ConcurrentDictionary<int, GloOfficeTimePoller>();

        public GloOfficeTimePollerRegistry(TimeSpan pollingPeriod, GloOfficeTimeClient client)
        {
            _pollingPeriod = pollingPeriod;
            _client = client;
        }

        public void StartPoller(int employeeId, Action<CheckinDetails> onNewTimestamp)
        {
            var poller = new GloOfficeTimePoller(this._pollingPeriod, this._client, onNewTimestamp);
            poller.Start(employeeId);

            this._registry.AddOrUpdate(employeeId, poller, (id, oldPoller) => oldPoller);
        }

        public void StopPoller(int employeeId)
        {
            if (this._registry.TryRemove(employeeId, out GloOfficeTimePoller poller))
            {
                poller.Dispose();
            }
        }
    }
}