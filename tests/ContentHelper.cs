using System;
using System.IO;

namespace Lucene.Net.Analysis.Hunspell.Tests {
    public static class ContentHelper {
        public static Stream Stream(String contentName) {
            var resourceName = "Lucene.Net.Analysis.Hunspell.Tests.Content." + contentName;

            var stream = typeof(ContentHelper).Assembly.GetManifestResourceStream(resourceName);
            if (stream == null)
                throw new ArgumentException(String.Format("Failed to read resource '{0}'", resourceName));

            return stream;
        }

        public static HunspellDictionary Dictionary(String baseName) {
            using (var affixStream = Stream(baseName + ".aff"))
            using (var dictStream = Stream(baseName + ".dic"))
                return new HunspellDictionary(affixStream, dictStream);
        }
    }
}