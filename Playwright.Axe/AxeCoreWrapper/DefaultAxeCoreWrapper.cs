﻿#nullable enable

using Microsoft.Playwright;
using Playwright.Axe.AxeContent;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Playwright.Axe.AxeCoreWrapper
{
    /// <inheritdoc/>
    internal sealed class DefaultAxeCoreWrapper : IAxeCoreWrapper
    {
        private static readonly JsonSerializerOptions s_jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters =
            {
                new JsonStringEnumConverter(JsonNamingPolicy.CamelCase),
                new RunContextJsonConverter()
            },
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            ReferenceHandler = ReferenceHandler.IgnoreCycles
        };

        private readonly IAxeContentEmbedder m_axeContentEmbedder;

        private readonly IAxeContentProvider m_contentContentProvider;

        public DefaultAxeCoreWrapper(IAxeContentEmbedder axeContentEmbedder, IAxeContentProvider axeContentProvider)
        {
            m_axeContentEmbedder = axeContentEmbedder;
            m_contentContentProvider = axeContentProvider;
        }

        /// <inheritdoc/>
        public async Task<IList<AxeRuleMetadata>> GetRules(IPage page, IList<string>? tags = null)
        {
            await m_axeContentEmbedder.EmbedAxeCoreIntoPage(page, false);

            object jsonObject = await page.EvaluateAsync<object>("(paramTags) => window.axe.getRules(paramTags)", tags);
            return DeserializeResult<List<AxeRuleMetadata>>(jsonObject);
        }

        /// <inheritdoc/>
        public async Task<AxeResults> Run(IPage page, AxeRunContext? context = null, AxeRunOptions? options = null)
        {
            await m_axeContentEmbedder.EmbedAxeCoreIntoPage(page, options?.Iframes);

            AxeResults axeResult = await EvaluateAxeRun<AxeResults>(page, context, options);

            return axeResult;
        }

        /// <inheritdoc/>
        public async Task<AxeResults> RunOnLocator(ILocator locator, AxeRunOptions? options = null)
        {
            await m_axeContentEmbedder.EmbedAxeCoreIntoPage(locator.Page, options?.Iframes);

            string? paramString = JsonSerializer.Serialize(options, s_jsonOptions);
            string runParamTemplate = options != null ? "JSON.parse(runOptions)" : string.Empty;

            object jsonObject = await locator.EvaluateAsync<object>($"(node, runOptions) => window.axe.run(node, {runParamTemplate})", paramString);
            return DeserializeResult<AxeResults>(jsonObject);
        }

        private static async Task<TResult> EvaluateAxeRun<TResult>(IPage page, AxeRunContext? context = null, object? param = null)
            where TResult : class
        {
            string? paramString = JsonSerializer.Serialize(param, s_jsonOptions);
            string runParamTemplate = param != null ? "JSON.parse(runOptions)" : string.Empty;

            string? contextParam = context is null ? string.Empty : ($"JSON.parse(\'{JsonSerializer.Serialize(context, s_jsonOptions)}\'),");

            object jsonObject = await page.EvaluateAsync<object>($"(runOptions) => window.axe.run({contextParam}{runParamTemplate})", paramString);

            return DeserializeResult<TResult>(jsonObject);
        }

        private async Task<AxeResults> EvaluateAxeRunPartial(IPage page, AxeRunContext? context = null, object? param = null)
        {
            string runPartialContent = m_contentContentProvider.GetLibContent();

            string? paramString = param is null 
                ? "{}"
                : JsonSerializer.Serialize(param, s_jsonOptions);
            string? contextParam = context is null 
                ? "document" 
                : ($"JSON.parse(\'{JsonSerializer.Serialize(context, s_jsonOptions)}\'),");

            object partialRunList = await page.EvaluateAsync<object>($"async () => await (() => {{{runPartialContent} return exposedModules.runPartialAndMerge({contextParam}, {paramString}, window)}})()");

            throw new NotImplementedException();
        }

        private static TResult DeserializeResult<TResult>(object jsonObject)
        {
            // Temporary solution for handling jsonelements with metadata properties which cause exceptions in some circumstances.
            string jsonString = JsonSerializer.Serialize(jsonObject, s_jsonOptions);
            TResult results = JsonSerializer.Deserialize<TResult>(jsonString, s_jsonOptions);

            if (results is null)
            {
                throw new Exception();
            }

            return results;
        }
    }
}
