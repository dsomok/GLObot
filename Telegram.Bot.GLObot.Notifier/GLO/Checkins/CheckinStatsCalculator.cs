using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telegram.Bot.GLObot.Notifier.GLO.Checkins;

namespace Telegram.Bot.GLObot.Notifier.GLO
{
    public class CheckinStatsCalculator
    {
        public List<CheckinEvent> CheckinEvents { get; set; }
        public CheckinStatsCalculator(List<CheckinEvent> checkinEvents)
        {
            CheckinEvents = checkinEvents;
        }

        public CheckinStats Calculate()
        {
            TimeSpan workingTime;
            byte teleportsCount = 0; //working wrong for now

            CheckinDirection previousDirection = CheckinDirection.Out;
            DateTime previousTime = DateTime.MinValue;
            string previousLocation = null;  //cannot use locationId because of GLOT bug
            bool previousIntervalOpened = false;

            foreach (var checkinEvent in CheckinEvents)
            {
                if (checkinEvent.Direction == CheckinDirection.In)
                {
                    if (!checkinEvent.Working)
                    {
                        if (previousIntervalOpened)
                        {
                            previousIntervalOpened = false;
                            previousDirection = CheckinDirection.Out;
                            //teleportsCount++;
                        }
                        continue;
                    }

                    if (previousDirection == CheckinDirection.In)
                    {
                       // teleportsCount++;
                    }

                    previousIntervalOpened = true;
                    previousTime = checkinEvent.TimeStamp;
                    previousLocation = checkinEvent.Area;
                    previousDirection = CheckinDirection.In;
                    continue;
                }

                if (checkinEvent.Direction == CheckinDirection.Out)
                {
                    if (previousDirection == CheckinDirection.In && checkinEvent.Area == previousLocation && checkinEvent.Working)
                    {
                        previousDirection = CheckinDirection.Out;
                        workingTime += checkinEvent.TimeStamp - previousTime;
                        previousTime = checkinEvent.TimeStamp;
                        previousIntervalOpened = false;
                        continue;
                    }

                    if (previousDirection == CheckinDirection.Out && checkinEvent.Area == previousLocation
                                                                  && (checkinEvent.TimeStamp - previousTime).TotalSeconds < 10) //GLOT doesn't count 2 outs from same location if they are close in time(?)
                    {
                        previousDirection = CheckinDirection.Out;
                        continue;
                    }

                    //previousIntervalOpened = false;
                    previousDirection = CheckinDirection.Out;
                    //teleportsCount++;
                }

            }

            return new CheckinStats(workingTime, teleportsCount, CheckinEvents.FirstOrDefault()?.TimeStamp);
        }
    }
}
