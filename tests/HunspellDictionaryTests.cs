using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Lucene.Net.Analysis.Hunspell.Tests.Helpers;

namespace Lucene.Net.Analysis.Hunspell.Tests
{
    public class HunspellDictionaryTests
    {
        public void WhenParsingAffFileWithAliases_ThenItWorks()
        {
            var dico = new HunspellDictionary(
                ResourceToFile.RscToStream("Lucene.Net.Analysis.Hunspell.Tests.Content.fr-moderne.aff"),
                ResourceToFile.RscToStream("Lucene.Net.Analysis.Hunspell.Tests.Content.fr-moderne.dic")
                );

            Assert.True(true);
        }
    }
}
