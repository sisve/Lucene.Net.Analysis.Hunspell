using System;
using System.IO;

namespace Lucene.Net.Analysis.Hunspell.Tests.Helpers {
    internal static class ResourceToFile {
        /*public static void RscToFile(string resourceName, string outputPath) {
            using (var imgStream = typeof(ResourceToFile).Assembly.GetManifestResourceStream(resourceName)) {
                using (var file = File.Create(outputPath)) {
                    imgStream.CopyTo(file);
                }
            }
        }*/

        public static Stream RscToStream(String resourceName) {
            return typeof (ResourceToFile).Assembly.GetManifestResourceStream(resourceName);
        }
    }
}