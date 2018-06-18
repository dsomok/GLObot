using System.Collections.Generic;
using Shouldly;
using Telegram.Bot.Library.GLO.Checkins;
using Telegram.Bot.Library.GLO.Serialization.Types;
using Xunit;

namespace Telegram.Bot.GLObot.UnitTests.CheckinStatsCalculatorTests
{
    public class MultipleINsInARow
    {
        [Fact]
        public void ThreeINsInARow()
        {
            var checkinEvents = CheckinEventsListPrimer1_70();
            var result = new CheckinStatsCalculator(checkinEvents).Calculate();
            result.WorkingTimeToday.TotalMinutes.ShouldBe(70);
        }

        [Fact]
        public void ThreeINsInARowWithWorkingFalse()
        {
            var checkinEvents = CheckinEventsListPrimer2_70();
            var result = new CheckinStatsCalculator(checkinEvents).Calculate();
            result.WorkingTimeToday.TotalMinutes.ShouldBe(70);
        }

        [Fact]
        public void FourINsInARowWithWorkingFalse()
        {
            var checkinEvents = CheckinEventsListPrimer3_50();
            var result = new CheckinStatsCalculator(checkinEvents).Calculate();
            result.WorkingTimeToday.TotalMinutes.ShouldBe(50);
        }

        [Fact]
        public void TwoINsInARowWithWorkingFalse()
        {
            var checkinEvents = CheckinEventsListPrimer4_30();
            var result = new CheckinStatsCalculator(checkinEvents).Calculate();
            result.WorkingTimeToday.TotalMinutes.ShouldBe(30);
        }

        private List<CheckinEvent> CheckinEventsListPrimer1_70()
        {
            return new List<CheckinEvent>
            {
                new CheckinEvent("Office KBP3-R", CheckinDirection.In, "2018/02/27 10:00:00", 0, true, ""),
                new CheckinEvent("Office KBP3-R", CheckinDirection.Out, "2018/02/27 10:10:00", 0, true, ""),
                new CheckinEvent("Office KBP2-R", CheckinDirection.In, "2018/02/27 10:20:00", 0, true, ""),
                new CheckinEvent("Office KBP3-R", CheckinDirection.In, "2018/02/27 10:40:00", 0, true, ""),
                new CheckinEvent("Office KBP5-R", CheckinDirection.In, "2018/02/27 11:00:00", 0, true, ""),
                new CheckinEvent("Office KBP5-R", CheckinDirection.Out, "2018/02/27 12:00:00", 0, true, "")
            };
        }

        private List<CheckinEvent> CheckinEventsListPrimer2_70()
        {
            return new List<CheckinEvent>
            {
                new CheckinEvent("Office KBP3-R", CheckinDirection.In, "2018/02/27 10:00:00", 0, true, ""),
                new CheckinEvent("Office KBP3-R", CheckinDirection.Out, "2018/02/27 10:10:00", 0, true, ""),
                new CheckinEvent("Office KBP2-R", CheckinDirection.In, "2018/02/27 10:20:00", 0, true, ""),
                new CheckinEvent("Office KBP3-R", CheckinDirection.In, "2018/02/27 10:40:00", 0, false, ""),
                new CheckinEvent("Office KBP3-R", CheckinDirection.In, "2018/02/27 10:50:00", 0, false, ""),
                new CheckinEvent("Office KBP5-R", CheckinDirection.In, "2018/02/27 11:00:00", 0, true, ""),
                new CheckinEvent("Office KBP5-R", CheckinDirection.Out, "2018/02/27 12:00:00", 0, true, "")
            };
        }

        private List<CheckinEvent> CheckinEventsListPrimer3_50()
        {
            return new List<CheckinEvent>
            {
                new CheckinEvent("Office KBP3-R", CheckinDirection.Out, "2018/02/27 10:10:00", 0, true, ""),
                new CheckinEvent("Office KBP3-R", CheckinDirection.In, "2018/02/27 10:20:00", 0, true, ""),
                new CheckinEvent("Office KBP3-R", CheckinDirection.In, "2018/02/27 10:40:00", 0, false, ""),
                new CheckinEvent("Office KBP5-R", CheckinDirection.In, "2018/02/27 11:00:00", 0, false, ""),
                new CheckinEvent("Office KBP5-R", CheckinDirection.In, "2018/02/27 11:10:00", 0, true, ""),
                new CheckinEvent("Office KBP5-R", CheckinDirection.Out, "2018/02/27 12:00:00", 0, true, "")
            };
        }

        private List<CheckinEvent> CheckinEventsListPrimer4_30()
        {
            return new List<CheckinEvent>
            {
                new CheckinEvent("Office KBP3-R", CheckinDirection.In, "2018/02/27 10:10:00", 0, true, ""),
                new CheckinEvent("Office KBP3-R", CheckinDirection.Out, "2018/02/27 10:20:00", 0, true, ""),
                new CheckinEvent("Office KBP3-R", CheckinDirection.In, "2018/02/27 10:40:00", 0, false, ""),
                new CheckinEvent("Office KBP5-R", CheckinDirection.In, "2018/02/27 11:00:00", 0, true, ""),
                new CheckinEvent("Office KBP5-R", CheckinDirection.Out, "2018/02/27 11:20:00", 0, true, "")
            };
        }
    }
}
