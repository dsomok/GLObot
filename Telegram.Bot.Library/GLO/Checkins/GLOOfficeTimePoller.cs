using System;
using System.Threading;

namespace Telegram.Bot.Library.GLO.Checkins
{
    internal class GloOfficeTimePoller : IDisposable
    {
        private readonly TimeSpan _period;
        private readonly GloOfficeTimeClient _client;
        private readonly Action<CheckinDetails> _onNewTimestamp;
        private Timer _timer;

        public GloOfficeTimePoller(TimeSpan period, GloOfficeTimeClient client, Action<CheckinDetails> onNewTimestamp)
        {
            _period = period;
            _client = client;
            _onNewTimestamp = onNewTimestamp ?? throw new ArgumentNullException(nameof(onNewTimestamp));
        }

        public void Start(int employeeId)
        {
            _timer = new Timer(async _ =>
            {
                var checkinDetails = await this._client.WhenLastSeen(employeeId);
                this._onNewTimestamp(checkinDetails);
            }, null, TimeSpan.Zero, this._period);
        }

        public void Dispose()
        {
            this._timer?.Dispose();
        }
    }
}