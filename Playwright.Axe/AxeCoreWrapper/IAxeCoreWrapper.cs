#nullable enable

using Microsoft.Playwright;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Playwright.Axe.AxeCoreWrapper
{
    /// <summary>
    /// Wrapper for Axe Core Library
    /// </summary>
    public interface IAxeCoreWrapper
    {
        /// <summary>
        /// Gets Rule Metadata based on tags.
        /// </summary>
        public Task<IList<AxeRuleMetadata>> GetRules(IPage page, IList<string>? tags = null);

        /// <summary>
        /// Calls the Run/Analyze method of Axe Core.
        /// </summary>
        public Task<AxeResults> Run(IPage page, AxeRunContext? context = null, AxeRunOptions? options = null);

        /// <summary>
        /// Runs Axe on a Playwright Locator
        /// </summary>
        public Task<AxeResults> RunOnLocator(ILocator locator, AxeRunOptions? options = null);

        /// <summary>
        /// Gets the frames for a particular run context and determines what context object to use in that frame.
        /// </summary>
        public Task<IList<AxeFrameContext>> GetFrameContexts(IPage page, AxeRunContext? context = null, AxeRunOptions? options = null);
    }
}
