using System.Collections.Generic;
using Shouldly;
using Telegram.Bot.GLObot.Notifier.Webhook.GLO.Checkins;
using Xunit;

namespace Telegram.Bot.GLObot.UnitTests.CheckinStatsCalculatorTests
{
    public class SimpleNoEdgeCases
    {
        private readonly List<CheckinEvent> _checkinEvents;

        public SimpleNoEdgeCases()
        {
            _checkinEvents = CheckinEventsListPrimer1_90();
        }

        [Fact]
        public void SimpleScenario()
        {
            var result = new CheckinStatsCalculator(_checkinEvents).Calculate();
            result.WorkingTimeToday.TotalMinutes.ShouldBe(90);
        }

        private List<CheckinEvent> CheckinEventsListPrimer1_90()
        {
            return new List<CheckinEvent>
            {
                new CheckinEvent("Office KBP3-R", CheckinDirection.In, "2018/02/27 10:00:00", 0, true, ""),
                new CheckinEvent("Office KBP3-R", CheckinDirection.Out, "2018/02/27 10:10:00", 0, true, ""),
                new CheckinEvent("Office KBP2-R", CheckinDirection.In, "2018/02/27 10:20:00", 0, true, ""),
                new CheckinEvent("Office KBP2-R", CheckinDirection.Out, "2018/02/27 10:40:00", 0, true, ""),
                new CheckinEvent("Office KBP5-R", CheckinDirection.In, "2018/02/27 11:00:00", 0, true, ""),
                new CheckinEvent("Office KBP5-R", CheckinDirection.Out, "2018/02/27 12:00:00", 0, true, "")
            };
        }
    }
}
