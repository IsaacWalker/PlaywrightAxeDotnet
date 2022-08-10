﻿#nullable enable

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Playwright.Axe.AxeContent
{
    /// <inheritdoc />
    public sealed class DefaultAxeContentProvider : IAxeContentProvider
    {
        /// <inheritdoc />
        public string GetAxeCoreScriptContent() => GetFileContents("axe.js");

        /// <inheritdoc />
        public string GetLibContent() => GetFileContents("lib.js");

        /// <inheritdoc />
        public IDictionary<string, string> GetHtmlReportFiles()
        {
            IList<string> reportStaticFiles = new List<string>()
            {
                "index.html",
                "index.report.js"
            };

            return reportStaticFiles.ToDictionary(sf => sf, sf => GetFileContents(sf));
        }

        private string GetFileContents(string filename)
        {
            Stream resourceStream = Assembly
                .GetExecutingAssembly()
                .GetManifestResourceStream($"Playwright.Axe.{filename}");

            using var reader = new StreamReader(resourceStream, Encoding.UTF8);
                return reader.ReadToEnd();
        }
    }
}
