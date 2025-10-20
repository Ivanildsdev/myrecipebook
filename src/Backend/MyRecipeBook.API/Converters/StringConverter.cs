using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace MyRecipeBook.API.Converters
{
    public partial class MyStringConverter : JsonConverter<string>
    {
        public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString()?.Trim();
            
            if (value == null) return null;

            return RemoveWhiteSpaces().Replace(value, " ");
        }

        public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value);
        }

        [GeneratedRegex(@"\s+")]
        private static partial Regex RemoveWhiteSpaces();
    }
}
