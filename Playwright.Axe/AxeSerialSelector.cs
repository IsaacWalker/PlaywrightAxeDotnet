#nullable enable

using System.Collections.Generic;
using System.Linq;

namespace Playwright.Axe
{
    /// <summary>
    /// Axe Serial Selector
    /// </summary>
    public sealed class AxeSerialSelector
    {
        /// <summary>
        /// Single Value Selector
        /// </summary>
        public AxeCrossTreeSelector? Single { get; }

        /// <summary>
        /// Array Value Selector
        /// </summary>
        public IList<AxeCrossTreeSelector>? Array { get; }

        /// <summary>
        /// Single Value Constructor
        /// </summary>
        public AxeSerialSelector(AxeCrossTreeSelector single)
        {
            Single = single;
        }

        /// <summary>
        /// Array Value Constructor
        /// </summary>
        public AxeSerialSelector(IList<AxeCrossTreeSelector> array)
        {
            Array = array;
        }

        /// <inheritdoc />
        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            if (other is AxeSerialSelector axeSerialSelector)
            {
                if (Single == null && axeSerialSelector.Single == null)
                {
                    return true;
                }

                if (Single == null || axeSerialSelector.Single == null)
                {
                    return false;
                }

                if(Single.Equals(axeSerialSelector.Single))
                {
                    return true;
                }

                if (Array == null && axeSerialSelector.Array == null)
                {
                    return true;
                }

                if (Array == null || axeSerialSelector.Array == null)
                {
                    return false;
                }

                return Enumerable.SequenceEqual(Array, axeSerialSelector.Array);
            }

            return false;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            if(Single != null)
                return Single.GetHashCode();
            else
                return Array!.GetHashCode();
        }
    }
}
