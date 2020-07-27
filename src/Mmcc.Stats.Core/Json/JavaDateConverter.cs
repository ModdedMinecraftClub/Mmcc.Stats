using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mmcc.Stats.Core.Json
{
    public class JavaDateConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var javaLongDate = reader.GetInt64();
            var dateTime = new DateTime(1970,1,1,0,0,0,0,System.DateTimeKind.Utc);
            dateTime = dateTime.AddMilliseconds(javaLongDate);
            return dateTime;
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue(value.Ticks);
        }
    }
}