using System.Text.Json;
using System.Text.Json.Serialization;

namespace MyConvert;

public class DateTimeJsonConverter2Timestamp : JsonConverter<DateTime>
{
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var tsDatetime = reader.GetInt64().MillisecondsToDateTime();
        return tsDatetime;
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        var jsonDateTimeFormat = value.ToTimestampMilliseconds();
        writer.WriteNumberValue(jsonDateTimeFormat);
    }
}
