using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Lucene.Net.Analysis.Hunspell.Tests.Helpers
{
    static class ResourceToFile
    {
        public static void RscToFile(string resourceName, string outputPath)
        {
            using (var imgStream = typeof(ResourceToFile).Assembly.GetManifestResourceStream(resourceName))
            {
                using (var file = File.Create(outputPath))
                {
                    imgStream.CopyTo(file);
                }
            }
        }

        public static Stream RscToStream(string resourceName)
        {
            return typeof(ResourceToFile).Assembly.GetManifestResourceStream(resourceName);
        }
    }
}
