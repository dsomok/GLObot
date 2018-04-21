using System;
using J = Newtonsoft.Json.JsonPropertyAttribute;

namespace Telegram.Bot.Library.GLO.Checkins
{
    public enum CheckinDirection
    {
        In,
        Out
    }
    public class CheckinBase
    {
        public CheckinBase(string area, CheckinDirection direction, string timestamp)
        {
            Area = area;
            Direction = direction;
            Timestamp = timestamp;
        }

        [J("area")]      public string Area { get; private set; }
        [J("timestamp")] public string Timestamp { get; private set; }
        [J("direction")] public CheckinDirection Direction { get; private set; }


        public string DirectionName => Enum.GetName(typeof(CheckinDirection), this.Direction);

        public DateTime TimeStamp
        {
            get
            {
                DateTime.TryParse(Timestamp, out var dateTime);
                return dateTime;
            }
        }
    }
}
