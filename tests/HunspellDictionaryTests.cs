using Lucene.Net.Analysis.Hunspell.Tests.Helpers;
using NUnit.Framework;

namespace Lucene.Net.Analysis.Hunspell.Tests {
    [TestFixture]
    public class HunspellDictionaryTests {
        [Test]
        public void WhenParsingAffFileWithAliases_ThenItWorks() {
            new HunspellDictionary(
                ResourceToFile.RscToStream("Lucene.Net.Analysis.Hunspell.Tests.Content.fr-moderne.aff"),
                ResourceToFile.RscToStream("Lucene.Net.Analysis.Hunspell.Tests.Content.fr-moderne.dic")
            );

            Assert.True(true);
        }
    }
}