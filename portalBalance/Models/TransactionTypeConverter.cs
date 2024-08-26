using portalBalance.Models.DTO;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

public class TransactionTypeConverter : JsonConverter<TransactionType>
{
    public override TransactionType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string value = reader.GetString();
        return value switch
        {
            "Credit" => TransactionType.Credit,
            "Debit" => TransactionType.Debit,
            _ => throw new JsonException($"Value '{value}' is not valid for enum type '{nameof(TransactionType)}'")
        };
    }

    public override void Write(Utf8JsonWriter writer, TransactionType value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}
