using System.Linq;
using NUnit.Framework;

namespace Lucene.Net.Analysis.Hunspell.Tests {
    [TestFixture]
    public class HunspellDictionaryTests {
        [Test(Description = "en_US affix and dict files are loaded without error, with 2 suffixes for 'ings' being loaded, 2 prefixes for 'in' and 1 word for 'drink' ")]
        public void TestHunspellDictionary_LoadEnUSDict() {
            var dictionary = ContentHelper.Dictionary("en_US");

            Assert.AreEqual(2, dictionary.LookupSuffix(new[] { 'i', 'n', 'g', 's' }, 0, 4).Count());
            Assert.AreEqual(1, dictionary.LookupPrefix(new[] { 'i', 'n' }, 0, 2).Count());
            Assert.AreEqual(1, dictionary.LookupWord("drink").Count());
        }

        [Test(Description = "fr-moderne affix and dict files are loaded without error")]
        public void TestHunspellDictionary_LoadFrModerneDict() {
            Assert.DoesNotThrow(() => ContentHelper.Dictionary("fr-moderne"));
        }
    }
}
