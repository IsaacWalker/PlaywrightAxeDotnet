#nullable enable

using System.Text.Json;
using System.Text.Json.Serialization;

namespace Playwright.Axe
{
    /// <summary>
    /// Json Options for Serializing and Deserializing Axe Models
    /// </summary>
    public static class AxeJsonSerializerOptions
    {
        /// <summary>
        /// Value
        /// </summary>
        public static readonly JsonSerializerOptions Value = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters =
            {
                new JsonStringEnumConverter(JsonNamingPolicy.CamelCase),
                new RunContextJsonConverter(),
                new CrossTreeSelectorJsonConverter(),
                new SerialSelectorJsonConverter()
            },
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            ReferenceHandler = ReferenceHandler.IgnoreCycles
        };
    }
}
