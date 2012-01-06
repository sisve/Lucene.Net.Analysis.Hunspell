using System;
using System.Collections.Generic;
using System.Linq;
using Lucene.Net.Analysis.Tokenattributes;

namespace Lucene.Net.Analysis.Hunspell {
    /// <summary>
    ///   TokenFilter that uses hunspell affix rules and words to stem tokens.  Since hunspell supports a
    ///   word having multiple stems, this filter can emit multiple tokens for each consumed token.
    /// </summary>
    public class HunspellStemFilter : TokenFilter {
        private readonly TermAttribute _termAtt;
        private readonly PositionIncrementAttribute _posIncAtt;
        private readonly HunspellStemmer _stemmer;

        private readonly Queue<HunspellStem> _buffer = new Queue<HunspellStem>();
        private State _savedState;

        private readonly Boolean _dedup;

        /// <summary>
        ///   Creates a new HunspellStemFilter that will stem tokens from the given TokenStream using
        ///   affix rules in the provided HunspellDictionary.
        /// </summary>
        /// <param name="input">TokenStream whose tokens will be stemmed.</param>
        /// <param name="dictionary">HunspellDictionary containing the affix rules and words that will be used to stem the tokens.</param>
        /// <param name="dedup">true if only unique terms should be output.</param>
        public HunspellStemFilter(TokenStream input, HunspellDictionary dictionary, Boolean dedup = true)
            : base(input) {
            _posIncAtt = (PositionIncrementAttribute)AddAttribute(typeof(PositionIncrementAttribute));
            _termAtt = (TermAttribute)AddAttribute(typeof(TermAttribute));

            _dedup = dedup;
            _stemmer = new HunspellStemmer(dictionary);
        }

        public override Boolean IncrementToken() {
            if (_buffer.Any()) {
                var nextStem = _buffer.Dequeue();

                RestoreState(_savedState);
                _posIncAtt.SetPositionIncrement(0);
                _termAtt.SetTermBuffer(nextStem.Stem, 0, nextStem.StemLength);
                return true;
            }

            if (!input.IncrementToken())
                return false;

            var newTerms = _dedup
                               ? _stemmer.UniqueStems(_termAtt.Term())
                               : _stemmer.Stem(_termAtt.Term());
            foreach (var newTerm in newTerms)
                _buffer.Enqueue(newTerm);

            if (_buffer.Count == 0)
                // we do not know this word, return it unchanged
                return true;

            var stem = _buffer.Dequeue();
            _termAtt.SetTermBuffer(stem.Stem, 0, stem.StemLength);

            if (_buffer.Count > 0)
                _savedState = CaptureState();

            return true;
        }

        public override void Reset() {
            base.Reset();

            _buffer.Clear();
        }
    }
}
