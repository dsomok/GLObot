using System;
using System.Net;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Telegram.Bot.Examples.Echo
{
    public partial class LastSeenResponse
    {
        [JsonProperty("timestamp")]
        public string Timestamp { get; set; }

        [JsonProperty("diff_s")]
        public long DiffS { get; set; }

        [JsonProperty("locationid")]
        public long Locationid { get; set; }

        [JsonProperty("direction")]
        public string Direction { get; set; }

        [JsonProperty("area")]
        public string Area { get; set; }

        [JsonProperty("working")]
        public bool Working { get; set; }

        [JsonProperty("diff_str")]
        public string DiffStr { get; set; }
    }

    public partial class LastSeenResponse
    {
        public static LastSeenResponse FromJson(string json) => JsonConvert.DeserializeObject<LastSeenResponse>(json, Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this LastSeenResponse self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }

    public class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
        };
    }
}
