using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Livekit;
using Newtonsoft.Json;

namespace LiveKit_CSharp.Auth
{
    public class VideoGrant
    {
        [JsonProperty("roomCreate")] public bool RoomCreate { get; set; }
        [JsonProperty("roomList")] public bool RoomList { get; set; }
        [JsonProperty("roomRecord")] public bool RoomRecord { get; set; }
        [JsonProperty("roomAdmin")] public bool RoomAdmin { get; set; }
        [JsonProperty("roomJoin")] public bool RoomJoin { get; set; }
        [JsonProperty("room")] public string Room { get; set; }
        [JsonProperty("canPublish")] public bool? CanPublish { get; set; }
        [JsonProperty("canSubscribe")] public bool? CanSubscribe { get; set; }

        [JsonProperty("canPublishData")] public bool? CanPublishData { get; set; }

        [JsonProperty("canPublishSources")] public List<TrackSource> CanPublishSources { get; set; }

        [JsonProperty("canUpdateOwnMetadata")] public bool? CanUpdateOwnMetadata { get; set; }

        [JsonProperty("ingressAdmin")] public bool IngressAdmin { get; set; }

        [JsonProperty("hidden")] public bool Hidden { get; set; }

        [JsonProperty("recorder")] public bool Recorder { get; set; }

        public Dictionary<string, object> ToDictionary()
        {
            var dictionary = new Dictionary<string, object>();
            var type = GetType();
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                var propertyName = property.GetCustomAttribute<JsonPropertyAttribute>()?.PropertyName;
                var propertyValue = property.GetValue(this);

                if (string.IsNullOrEmpty(propertyName) || propertyValue == null) continue;
                
                if (propertyValue is List<TrackSource> sourceList && sourceList.Any())
                {
                    
                    dictionary[propertyName] = sourceList.Select(x =>
                    {
                        var enumName = x.ToString(); // 获取枚举成员的名称
                        var words = Regex.Split(enumName, @"(?<!^)(?=[A-Z])"); // 使用正则表达式将名称拆分为单词
                        return  string.Join("_", words).ToLower();
                    }).ToList();
                }
                else if (propertyValue is bool boolValue && boolValue)
                {
                    dictionary[propertyName] = boolValue;
                }
                else if (!string.IsNullOrEmpty(propertyName) && !(propertyValue is bool))
                {
                    dictionary[propertyName] = propertyValue;
                }
            }

            return dictionary;
        }
    }
}