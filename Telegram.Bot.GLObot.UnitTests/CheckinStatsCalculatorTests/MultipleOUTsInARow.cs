using System;
using System.Collections.Generic;
using System.Text;
using Shouldly;
using Telegram.Bot.Library.GLO.Checkins;
using Telegram.Bot.Library.GLO.Serialization.Types;
using Xunit;

namespace Telegram.Bot.GLObot.UnitTests.CheckinStatsCalculatorTests
{
    public class MultipleOUTsInARow
    {
        [Fact]
        public void ThreeOUTsInARow()
        {
            var checkinEvents = CheckinEventsListPrimer1_70();
            var result = new CheckinStatsCalculator(checkinEvents).Calculate();
            result.WorkingTimeToday.TotalMinutes.ShouldBe(70);
        }

        [Fact]
        public void ThreeOUTsInARowNotInWorkArea()
        {
            var checkinEvents = CheckinEventsListPrimer2_100();
            var result = new CheckinStatsCalculator(checkinEvents).Calculate();
            result.WorkingTimeToday.TotalMinutes.ShouldBe(100);
        }



        private List<CheckinEvent> CheckinEventsListPrimer1_70()
        {
            return new List<CheckinEvent>
            {
                new CheckinEvent("Office KBP3-R", CheckinDirection.In, "2018/02/27 10:00:00", 0, true, ""),
                new CheckinEvent("Office KBP3-R", CheckinDirection.Out, "2018/02/27 10:10:00", 0, true, ""),
                new CheckinEvent("Office KBP2-R", CheckinDirection.Out, "2018/02/27 10:20:00", 0, true, ""),
                new CheckinEvent("Office KBP3-R", CheckinDirection.Out, "2018/02/27 10:40:00", 0, true, ""),
                new CheckinEvent("Office KBP5-R", CheckinDirection.In, "2018/02/27 11:00:00", 0, true, ""),
                new CheckinEvent("Office KBP5-R", CheckinDirection.Out, "2018/02/27 12:00:00", 0, true, "")
            };
        }

        private List<CheckinEvent> CheckinEventsListPrimer2_100()
        {
            return new List<CheckinEvent>
            {
                new CheckinEvent("Office KBP3-R", CheckinDirection.In, "2018/02/27 10:00:00", 0, true, ""),
                new CheckinEvent("Office KBP3-R", CheckinDirection.Out, "2018/02/27 10:10:00", 0, false, ""),
                new CheckinEvent("Office KBP2-R", CheckinDirection.Out, "2018/02/27 10:20:00", 0, false, ""),
                new CheckinEvent("Office KBP3-R", CheckinDirection.Out, "2018/02/27 10:40:00", 0, true, ""),
                new CheckinEvent("Office KBP5-R", CheckinDirection.In, "2018/02/27 11:00:00", 0, true, ""),
                new CheckinEvent("Office KBP5-R", CheckinDirection.Out, "2018/02/27 12:00:00", 0, true, "")
            };
        }



    }
}
