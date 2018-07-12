using System;
using System.Collections.Generic;
using System.Linq;
using Telegram.Bot.Library.GLO.Serialization.Types;

namespace Telegram.Bot.Library.GLO.Checkins
{
    internal class CheckinStatsCalculator
    {
        public List<CheckinEvent> CheckinEvents { get; set; }
        public CheckinStatsCalculator(List<CheckinEvent> checkinEvents)
        {
            CheckinEvents = checkinEvents;
        }

        public CheckinStats Calculate()
        {
            CheckinDetails lastCheckin = null;

            var lastCheckinEvent = CheckinEvents.Where(e => string.IsNullOrEmpty(e.Comment))
                                                .OrderByDescending(e => e.TimeStamp)
                                                .FirstOrDefault();
            if (lastCheckinEvent != null)
            {
                lastCheckin = new CheckinDetails(
                    area: lastCheckinEvent.Area,
                    secondsAgo: DateTime.UtcNow.AddHours(2) - lastCheckinEvent.TimeStamp,
                    direction: lastCheckinEvent.Direction,
                    timestamp: lastCheckinEvent.Timestamp
                );
            }

            TimeSpan workingTime = TimeSpan.Zero;

            byte teleportsCount = 0; //working wrong for now as its not clear how GLOT counts it

            CheckinDirection previousDirection = CheckinDirection.Out;
            DateTime previousTime = DateTime.MinValue;
            string previousLocation = null;  //cannot use locationId because of GLOT bug

            CheckinEvents.RemoveAll(x => x.Working == false);
            TimeSpan lostTeleportTime = TimeSpan.Zero;

            foreach (var checkinEvent in CheckinEvents)
            {
                if (checkinEvent.Direction == CheckinDirection.In)
                {
                    if (previousDirection == CheckinDirection.In)
                    {
                        lostTeleportTime += checkinEvent.TimeStamp - previousTime;
                    }
                    previousTime = checkinEvent.TimeStamp;
                    previousLocation = checkinEvent.Area;
                    previousDirection = CheckinDirection.In;
                    continue;
                }

                if (checkinEvent.Direction == CheckinDirection.Out)
                {
                    if (previousDirection == CheckinDirection.In && checkinEvent.Area == previousLocation)
                    {
                        previousDirection = CheckinDirection.Out;
                        workingTime += checkinEvent.TimeStamp - previousTime;
                        previousTime = checkinEvent.TimeStamp;
                        continue;
                    }

                    if (previousDirection == CheckinDirection.Out && checkinEvent.Area == previousLocation
                                                                  && (checkinEvent.TimeStamp - previousTime).TotalSeconds < 10) //GLOT doesn't count 2 outs from same location if they are close in time(?)
                    {
                        previousDirection = CheckinDirection.Out;
                        continue;
                    }

                    if (previousDirection == CheckinDirection.Out && previousTime != DateTime.MinValue)
                    {
                        lostTeleportTime += checkinEvent.TimeStamp - previousTime;
                    }

                    previousDirection = CheckinDirection.Out;
                }

            }

            return new CheckinStats(workingTime, teleportsCount, lostTeleportTime, CheckinEvents.FirstOrDefault()?.TimeStamp, lastCheckin);
        }
    }
}
