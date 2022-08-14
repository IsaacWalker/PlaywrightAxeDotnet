﻿#nullable enable

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Text.Json;

namespace Playwright.Axe.Test
{
    [TestClass]
    public class JsonSerializerOptionsTests
    {
        [TestMethod]
        [DynamicData(nameof(GetAxeSerialSelectorSerializationData), DynamicDataSourceType.Method)]
        public void AxeSerialSelector_WithOptions_SerializesToExpected(AxeSerialSelector axeSerialSelector, string expectedJson)
        {
            string actualJson = JsonSerializer.Serialize(axeSerialSelector, AxeJsonSerializerOptions.Value);
            Assert.AreEqual(expectedJson, actualJson);
        }

        [TestMethod]
        [DynamicData(nameof(GetAxeSerialSelectorDeserializationData), DynamicDataSourceType.Method)]
        public void AxeSerialSelector_WithOptions_DeserializesToExpected(string jsonInput, AxeSerialSelector expectedAxeSerialSelector)
        {
            AxeSerialSelector actualAxeSerialSelector = JsonSerializer.Deserialize<AxeSerialSelector>(jsonInput, AxeJsonSerializerOptions.Value);
            Assert.AreEqual(expectedAxeSerialSelector, actualAxeSerialSelector!);
        }

        private static IEnumerable<object[]> GetAxeSerialSelectorSerializationData()
        {
            yield return new object[]
            {
                new AxeSerialSelector(new AxeCrossTreeSelector("#id")),
                "\"#id\""
            };

            yield return new object[]
            {
                new AxeSerialSelector(new List<AxeCrossTreeSelector>() 
                { 
                    new AxeCrossTreeSelector("#id1"), 
                    new AxeCrossTreeSelector("#id2") 
                }),
                "[\"#id1\",\"#id2\"]"
            };
        }

        private static IEnumerable<object[]> GetAxeSerialSelectorDeserializationData()
        {
            yield return new object[]
            {
                "\"#id\"",
                new AxeSerialSelector(new AxeCrossTreeSelector("#id"))
            };

            yield return new object[]
            {
                "[\"#id1\",\"#id2\"]",
                new AxeSerialSelector(new List<AxeCrossTreeSelector>()
                {
                    new AxeCrossTreeSelector("#id1"),
                    new AxeCrossTreeSelector("#id2")
                })
            };
        }
    }
}