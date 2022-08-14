#nullable enable

namespace Playwright.Axe
{
    /// <summary>
    /// Serialized Run Context
    /// </summary>
    public sealed class AxeSerialContext
    {
        /// <summary>
        /// Selector which specifies what to include.
        /// </summary>
        public AxeSerialSelector? Include { get; }

        /// <summary>
        /// Selector which specifies what to exclude.
        /// </summary>
        public AxeSerialSelector? Exclude { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        public AxeSerialContext(AxeSerialSelector? include, AxeSerialSelector? exclude)
        {
            Include = include;
            Exclude = exclude;
        }
    }
}
