using System.Text.Json;
using System.Text.Json.Serialization;
using DevolucionesGarantias.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DevolucionesGarantias.Persistence.Configurations;

internal static class ValueObjectConversionHelpers
{
    private static readonly JsonSerializerOptions JsonOptions = CreateJsonOptions();

    public static ValueConverter<TValueObject, string> JsonConverter<TValueObject>()
        where TValueObject : class
    {
        return new ValueConverter<TValueObject, string>(
            value => JsonSerializer.Serialize(value, JsonOptions),
            json => JsonSerializer.Deserialize<TValueObject>(json, JsonOptions)!);
    }

    public static ValueComparer<TValueObject> JsonComparer<TValueObject>()
        where TValueObject : class
    {
        return new ValueComparer<TValueObject>(
            (left, right) => JsonSerializer.Serialize(left, JsonOptions) == JsonSerializer.Serialize(right, JsonOptions),
            value => value == null ? 0 : StringComparer.Ordinal.GetHashCode(JsonSerializer.Serialize(value, JsonOptions)),
            value => JsonSerializer.Deserialize<TValueObject>(JsonSerializer.Serialize(value, JsonOptions), JsonOptions)!);
    }

    private static JsonSerializerOptions CreateJsonOptions()
    {
        var options = new JsonSerializerOptions(JsonSerializerDefaults.Web);
        options.Converters.Add(new MoneyJsonConverter());
        options.Converters.Add(new FilePathJsonConverter());
        options.Converters.Add(new DateRangeJsonConverter());
        options.Converters.Add(new JsonStringEnumConverter());
        return options;
    }

    private sealed class MoneyJsonConverter : JsonConverter<Money>
    {
        public override Money Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using var document = JsonDocument.ParseValue(ref reader);
            var root = document.RootElement;

            var amount = root.TryGetProperty("amount", out var amountElement)
                ? amountElement.GetDecimal()
                : root.GetProperty("Amount").GetDecimal();

            var currency = root.TryGetProperty("currency", out var currencyElement)
                ? currencyElement.GetString()
                : root.GetProperty("Currency").GetString();

            return new Money(amount, currency ?? "COP");
        }

        public override void Write(Utf8JsonWriter writer, Money value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteNumber("amount", value.Amount);
            writer.WriteString("currency", value.Currency);
            writer.WriteEndObject();
        }
    }

    private sealed class FilePathJsonConverter : JsonConverter<FilePath>
    {
        public override FilePath Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using var document = JsonDocument.ParseValue(ref reader);
            var root = document.RootElement;

            var bucket = root.TryGetProperty("bucket", out var bucketElement)
                ? bucketElement.GetString()
                : root.GetProperty("Bucket").GetString();

            var path = root.TryGetProperty("path", out var pathElement)
                ? pathElement.GetString()
                : root.GetProperty("Path").GetString();

            string? url = null;
            if (root.TryGetProperty("url", out var urlElement) || root.TryGetProperty("Url", out urlElement))
            {
                url = urlElement.GetString();
            }

            return new FilePath(bucket ?? string.Empty, path ?? string.Empty, url);
        }

        public override void Write(Utf8JsonWriter writer, FilePath value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteString("bucket", value.Bucket);
            writer.WriteString("path", value.Path);
            writer.WriteString("url", value.Url);
            writer.WriteEndObject();
        }
    }

    private sealed class DateRangeJsonConverter : JsonConverter<DateRange>
    {
        public override DateRange Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using var document = JsonDocument.ParseValue(ref reader);
            var root = document.RootElement;

            var start = root.TryGetProperty("start", out var startElement)
                ? startElement.GetDateTimeOffset()
                : root.GetProperty("Start").GetDateTimeOffset();

            var end = root.TryGetProperty("end", out var endElement)
                ? endElement.GetDateTimeOffset()
                : root.GetProperty("End").GetDateTimeOffset();

            return new DateRange(start, end);
        }

        public override void Write(Utf8JsonWriter writer, DateRange value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteString("start", value.Start);
            writer.WriteString("end", value.End);
            writer.WriteEndObject();
        }
    }
}
