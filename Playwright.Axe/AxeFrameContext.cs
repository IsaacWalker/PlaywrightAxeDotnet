#nullable enable

namespace Playwright.Axe
{
    /// <summary>
    /// Run Context for a Frame
    /// </summary>
    public sealed class AxeFrameContext
    {
        /// <summary>
        /// Selector for the Frame in question.
        /// </summary>
        public AxeCrossTreeSelector FrameSelector { get; }

        /// <summary>
        /// Run Context for the Frame.
        /// </summary>
        public AxeSerialContext FrameContext { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        public AxeFrameContext(
            AxeCrossTreeSelector frameSelector,
            AxeSerialContext frameContext)
        {
            FrameSelector = frameSelector;
            FrameContext = frameContext;
        }
    }
}
