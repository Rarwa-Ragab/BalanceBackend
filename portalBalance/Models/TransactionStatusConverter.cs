using portalBalance.Models.DTO;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

public class TransactionStatusConverter : JsonConverter<TransactionStatus>
{
    public override TransactionStatus Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string value = reader.GetString();
        return value switch
        {
            "New" => TransactionStatus.New,
            "Confirmed" => TransactionStatus.Confirmed,
            "Approved" => TransactionStatus.Approved,
            _ => throw new JsonException($"Value '{value}' is not valid for enum type '{nameof(TransactionStatus)}'")
        };
    }

    public override void Write(Utf8JsonWriter writer,
        TransactionStatus value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}
