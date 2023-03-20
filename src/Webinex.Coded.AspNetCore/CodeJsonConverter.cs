using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Webinex.Coded.AspNetCore
{
    internal class CodeJsonConverter : JsonConverter<Code>
    {
        public override Code Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, Code value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value.Value, options);
        }
    }
}