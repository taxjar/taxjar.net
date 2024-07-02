using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Taxjar;

internal class TaxjarDateTimeFilterJsonConverter : JsonConverter<DateTime?>
{
    public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var dateTimeString = reader.GetString();
        if (string.IsNullOrEmpty(dateTimeString))
        {
            return null;
        }

        return DateTime.Parse(dateTimeString, CultureInfo.InvariantCulture);
    }

    public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value?.ToString("yyyy/MM/dd"));
    }
}
