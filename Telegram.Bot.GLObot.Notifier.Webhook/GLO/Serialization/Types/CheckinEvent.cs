using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using J = Newtonsoft.Json.JsonPropertyAttribute;

namespace Telegram.Bot.GLObot.Notifier.Webhook.GLO.Checkins
{
    public class CheckinEvent : CheckinBase
    {
        [J("locationid")] public long Locationid { get; set; }
        [J("working")] public bool Working { get; set; }
        [J("_comment")] public string Comment { get; set; }

        public static List<CheckinEvent> FromJson(string json) => JsonConvert.DeserializeObject<List<CheckinEvent>>(json, Converter.Settings);

        public CheckinEvent(string area, CheckinDirection direction, string timestamp, long locationid, bool working, string comment) 
            : base(area, direction, timestamp)
        {
            Locationid = locationid;
            Working = working;
            Comment = comment;
        }
    }

    internal class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters = {
                new IsoDateTimeConverter()
                {
                    DateTimeStyles = DateTimeStyles.AssumeUniversal,
                },
            },
        };
    }
}