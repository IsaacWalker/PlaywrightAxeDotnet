#nullable enable

using System.Collections.Generic;
using System.Linq;

namespace Playwright.Axe
{
    /// <summary>
    /// Axe Cross Tree Selector
    /// </summary>
    public sealed class AxeCrossTreeSelector
    {
        /// <summary>
        /// String Value Selector
        /// </summary>
        public string? StringValue { get; }

        /// <summary>
        /// Array Value Selector
        /// </summary>
        public IList<string>? ArrayValue { get; }

        /// <summary>
        /// Constructor which expects a string.
        /// </summary>
        public AxeCrossTreeSelector(string stringValue)
        {
            StringValue = stringValue;
        }

        /// <summary>
        /// Constructor which expects an Array
        /// </summary>
        public AxeCrossTreeSelector(IList<string> arrayValue)
        {
            ArrayValue = arrayValue;
        }

        /// <inheritdoc />
        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            if (other is AxeCrossTreeSelector axeSerialSelector)
            {
                if(string.Equals(StringValue, axeSerialSelector.StringValue))
                    return true;

                if(ArrayValue == null && axeSerialSelector.ArrayValue == null)
                {
                    return true;
                }

                if (ArrayValue == null || axeSerialSelector.ArrayValue == null)
                {
                    return false;
                }

                return Enumerable.SequenceEqual(ArrayValue, axeSerialSelector.ArrayValue);
            }

            return false;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            if (StringValue != null)
                return StringValue.GetHashCode();
            else
                return ArrayValue!.GetHashCode();
        }
    }
}
