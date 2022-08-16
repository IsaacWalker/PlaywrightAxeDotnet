#nullable enable

using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Playwright.Axe
{
    /// <summary>
    /// Json Converter for Frame Selector
    /// </summary>
    public sealed class SerialSelectorJsonConverter : JsonConverter<AxeSerialSelector>
    {
        /// <inheritdoc/>
        public override AxeSerialSelector? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Selector is either a string
            if(reader.TokenType == JsonTokenType.String)
            {
                string stringSelector = reader.GetString()!;
                return new AxeSerialSelector(new AxeCrossTreeSelector(stringSelector));
            }

            // Selector is an array
            if(reader.TokenType == JsonTokenType.StartArray)
            {
                reader.Read();

                IList<AxeCrossTreeSelector> crossTreeSelectors = new List<AxeCrossTreeSelector>();

                while(reader.TokenType != JsonTokenType.EndArray)
                {
                    IList<string> innerTreeSelectors = new List<string>();

                    // Selector is an array of arrays
                    if (reader.TokenType == JsonTokenType.StartArray)
                    {
                        reader.Read();

                        while (reader.TokenType != JsonTokenType.EndArray)
                        {
                            if (reader.TokenType == JsonTokenType.String)
                            {
                                innerTreeSelectors.Add(reader.GetString()!);
                            } 
                            else
                            {
                                throw new JsonException();
                            }
                                
                            reader.Read();
                        }

                        crossTreeSelectors.Add(new AxeCrossTreeSelector(innerTreeSelectors));
                    }
                    else if (reader.TokenType == JsonTokenType.String)
                    {
                        crossTreeSelectors.Add(new AxeCrossTreeSelector(reader.GetString()!));
                    }

                    reader.Read();
                }
                
                return new AxeSerialSelector(crossTreeSelectors);
            }

            throw new JsonException("Unexpected selector value.");
        }

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, AxeSerialSelector value, JsonSerializerOptions options)
        {
            if(value.Single != null)
            {
                writer.WriteRawValue(JsonSerializer.Serialize(value.Single, options));
            } 
            else
            {
                writer.WriteRawValue(JsonSerializer.Serialize(value.Array, options));
            }
        }
    }
}
