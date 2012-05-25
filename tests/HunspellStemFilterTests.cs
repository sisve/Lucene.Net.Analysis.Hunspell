using System;
using System.IO;
using Lucene.Net.Analysis.Standard;
using NUnit.Framework;
using LuceneVersion = Lucene.Net.Util.Version;

namespace Lucene.Net.Analysis.Hunspell.Tests {
    [TestFixture]
    public class HunspellStemFilterTests : BaseTokenStreamTestCase {
        private class DutchAnalyzer : Analyzer {
            private readonly HunspellDictionary _dictionary;

            public DutchAnalyzer() {
                _dictionary = ContentHelper.Dictionary("nl_NL");
            }

            public override TokenStream TokenStream(String fieldName, TextReader reader) {
                TokenStream stream = new StandardTokenizer(LuceneVersion.LUCENE_29, reader);
                stream = new LowerCaseFilter(stream);
                stream = new HunspellStemFilter(stream, _dictionary);
                return stream;
            }

            public override TokenStream ReusableTokenStream(string fieldName, TextReader reader) {
                var streams = (SavedStreams) GetPreviousTokenStream();
                if (streams == null) {
                    streams = new SavedStreams();
                    streams.Tokenizer = new StandardTokenizer(LuceneVersion.LUCENE_29, reader);
                    streams.Filter = new HunspellStemFilter(new LowerCaseFilter(streams.Tokenizer), _dictionary);
                    SetPreviousTokenStream(streams);
                } else {
                    streams.Tokenizer.Reset(reader);
                    streams.Filter.Reset();
                }

                return streams.Filter;
            }

            #region Nested type: SavedStreams

            private class SavedStreams {
                public Tokenizer Tokenizer { get; set; }
                public TokenStream Filter { get; set; }
            }

            #endregion
        };

        private readonly DutchAnalyzer _dutchAnalyzer = new DutchAnalyzer();

        [Test]
        public void TestDutch() {
            AssertAnalyzesTo(_dutchAnalyzer, "huizen",
                             new[] {"huizen", "huis"},
                             new[] {1, 0});
            AssertAnalyzesTo(_dutchAnalyzer, "huis",
                             new[] {"huis", "hui"},
                             new[] {1, 0});
            AssertAnalyzesToReuse(_dutchAnalyzer, "huizen huis",
                                  new[] {"huizen", "huis", "huis", "hui"},
                                  new[] {1, 0, 1, 0});
            AssertAnalyzesToReuse(_dutchAnalyzer, "huis huizen",
                                  new[] {"huis", "hui", "huizen", "huis"},
                                  new[] {1, 0, 1, 0});
        }
    }
}