#nullable enable

using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Playwright.Axe
{
    /// <summary>
    /// Converter for Cross Tree Selector
    /// </summary>
    public sealed class CrossTreeSelectorJsonConverter : JsonConverter<AxeCrossTreeSelector>
    {
        /// <inheritdoc/>
        public override AxeCrossTreeSelector? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if(reader.TokenType == JsonTokenType.StartArray)
            {
                reader.Read();

                IList<string> elements = new List<string>();

                while (reader.TokenType != JsonTokenType.EndArray)
                {
                    string? arrVal = reader.GetString();

                    if(arrVal == null)
                    {
                        throw new JsonException("Unexpected value in selector array.");
                    }

                    elements.Add(arrVal);

                    reader.Read();
                }

                return new AxeCrossTreeSelector(elements);
            }

            string? str = reader.GetString();

            if(str != null)
            {
                return new AxeCrossTreeSelector(str);
            }

            throw new JsonException("Unexpected selector value.");
        }

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, AxeCrossTreeSelector value, JsonSerializerOptions options)
        {
            if(value.StringValue != null)
            {
                writer.WriteStringValue(value.StringValue);
            } 
            else
            {
                writer.WriteStartArray();

                foreach(string str in value.ArrayValue!)
                {
                    writer.WriteStringValue(str);
                }

                writer.WriteEndArray();
            }
        }
    }
}
