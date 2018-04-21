using System;
using System.Collections.Concurrent;

namespace Telegram.Bot.Library.GLO.Checkins
{
    class GLOOfficeTimePollerRegistry
    {
        private readonly TimeSpan _pollingPeriod;
        private readonly GloOfficeTimeClient _client;

        private readonly ConcurrentDictionary<int, GLOOfficeTimePoller> _registry = new ConcurrentDictionary<int, GLOOfficeTimePoller>();

        public GLOOfficeTimePollerRegistry(TimeSpan pollingPeriod, GloOfficeTimeClient client)
        {
            _pollingPeriod = pollingPeriod;
            _client = client;
        }

        public void StartPoller(int employeeId, Action<CheckinDetails> onNewTimestamp)
        {
            var poller = new GLOOfficeTimePoller(this._pollingPeriod, this._client, onNewTimestamp);
            poller.Start(employeeId);

            this._registry.AddOrUpdate(employeeId, poller, (id, oldPoller) => oldPoller);
        }

        public void StopPoller(int employeeId)
        {
            if (this._registry.TryRemove(employeeId, out GLOOfficeTimePoller poller))
            {
                poller.Dispose();
            }
        }
    }
}