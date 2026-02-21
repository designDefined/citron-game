using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

[JsonConverter(typeof(StringEnumConverter))]
public enum EmotionEnum
{
    Happy,
    Sad,
    Angry,
    Tasty,
    Embarrassed,
    Spoiler,
}
