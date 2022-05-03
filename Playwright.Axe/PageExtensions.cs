﻿#nullable enable

using Microsoft.Playwright;
using Playwright.Axe.AxeContent;
using Playwright.Axe.AxeCoreWrapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Playwright.Axe
{
    /// <summary>
    /// Extensions for adding Axe to Playwright Page.
    /// </summary>
    public static class PageExtensions
    {
        /// <summary>
        /// Retrieves rule known in Axe.
        /// </summary>
        /// <param name="page">The Playwright Page object.</param>
        /// <param name="tags">Array of tags used to filter returned rules.</param>
        /// <returns>List of <see cref="AxeRuleMetadata"/></returns>
        public static async Task<IList<AxeRuleMetadata>> GetAxeRules(this IPage page, IList<string>? tags = null)
        {
            IAxeContentProvider axeContentProvider = new DefaultAxeContentProvider();
            IAxeContentEmbedder axeContentEmbedder = new DefaultAxeContentEmbedder(axeContentProvider);

            IAxeCoreWrapper axeCoreWrapper = new DefaultAxeCoreWrapper(axeContentEmbedder);

            return await axeCoreWrapper.GetRules(page, tags);
        }

        /// <summary>
        /// Runs Axe against the page in its current state.
        /// </summary>
        /// <param name="page">The Playwright Page object</param>
        /// <param name="options">Options for running Axe.</param>
        /// <returns>The AxeResults</returns>
        public static async Task<AxeResults> RunAxe(this IPage page, AxeRunOptions? options = null)
        {
            IAxeContentProvider axeContentProvider = new DefaultAxeContentProvider();
            IAxeContentEmbedder axeContentEmbedder = new DefaultAxeContentEmbedder(axeContentProvider);

            IAxeCoreWrapper axeCoreWrapper = new DefaultAxeCoreWrapper(axeContentEmbedder);

            return await axeCoreWrapper.Run(page, options);
        }
    }
}
