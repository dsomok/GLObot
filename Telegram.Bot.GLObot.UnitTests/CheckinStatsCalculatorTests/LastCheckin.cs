using System;
using System.Collections.Generic;
using System.Text;
using Shouldly;
using Telegram.Bot.Library.GLO.Checkins;
using Telegram.Bot.Library.GLO.Serialization.Types;
using Xunit;

namespace Telegram.Bot.GLObot.UnitTests.CheckinStatsCalculatorTests
{
    public class LastCheckin
    {
        [Fact]
        public void NoCheckinEvents()
        {
            var checkinEvents = new List<CheckinEvent>();
            var result = new CheckinStatsCalculator(checkinEvents).Calculate();
            result.LastCheckin.ShouldBeNull();
        }

        [Fact]
        public void InWorkingArea()
        {
            var checkinEvents = new List<CheckinEvent>
            {
                new CheckinEvent("Office KBP3-R", CheckinDirection.In, "2018/02/27 11:00:00", 0, true, ""),
                new CheckinEvent("Office KBP3-R", CheckinDirection.Out, "2018/02/27 12:00:00", 0, true, "Pass current server timestamp to client")
            };
            var result = new CheckinStatsCalculator(checkinEvents).Calculate();
            result.LastCheckin.ShouldNotBeNull();
            result.LastCheckin.Timestamp.ShouldBe("2018/02/27 11:00:00");
            result.LastCheckin.Direction.ShouldBe(CheckinDirection.In);
            result.LastCheckin.Area.ShouldBe("Office KBP3-R");
        }

        [Fact]
        public void InNotWorkingArea()
        {
            var checkinEvents = new List<CheckinEvent>
            {
                new CheckinEvent("G-club", CheckinDirection.In, "2018/02/27 11:00:00", 0, false, "")
            };
            var result = new CheckinStatsCalculator(checkinEvents).Calculate();
            result.LastCheckin.ShouldNotBeNull();
            result.LastCheckin.Timestamp.ShouldBe("2018/02/27 11:00:00");
            result.LastCheckin.Direction.ShouldBe(CheckinDirection.In);
            result.LastCheckin.Area.ShouldBe("G-club");
        }
    }
}
