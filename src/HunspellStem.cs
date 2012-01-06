using System;
using System.Collections.Generic;

namespace Lucene.Net.Analysis.Hunspell {
    public class HunspellStem {
        private readonly List<HunspellAffix> _prefixes = new List<HunspellAffix>();
        private readonly List<HunspellAffix> _suffixes = new List<HunspellAffix>();
        private readonly String _stem;

        /// <summary>
        ///   the actual word stem itself.
        /// </summary>
        public String Stem {
            get { return _stem; }
        }

        /// <summary>
        ///   The stem length.
        /// </summary>
        public Int32 StemLength {
            get { return _stem.Length; }
        }

        /// <summary>
        ///   The list of prefixes used to generate the stem.
        /// </summary>
        public IEnumerable<HunspellAffix> Prefixes {
            get { return _prefixes; }
        }

        /// <summary>
        ///   The list of suffixes used to generate the stem.
        /// </summary>
        public IEnumerable<HunspellAffix> Suffixes {
            get { return _suffixes; }
        }

        /// <summary>
        ///   Creates a new Stem wrapping the given word stem.
        /// </summary>
        public HunspellStem(String stem) {
            if (stem == null) throw new ArgumentNullException("stem");

            _stem = stem;
        }

        /// <summary>
        ///   Adds a prefix to the list of prefixes used to generate this stem. Because it is 
        ///   assumed that prefixes are added depth first, the prefix is added to the front of 
        ///   the list.
        /// </summary>
        /// <param name="prefix">Prefix to add to the list of prefixes for this stem.</param>
        public void AddPrefix(HunspellAffix prefix) {
            _prefixes.Insert(0, prefix);
        }

        /// <summary>
        ///   Adds a suffix to the list of suffixes used to generate this stem. Because it
        ///   is assumed that suffixes are added depth first, the suffix is added to the end
        ///   of the list.
        /// </summary>
        /// <param name="suffix">Suffix to add to the list of suffixes for this stem.</param>
        public void AddSuffix(HunspellAffix suffix) {
            _suffixes.Add(suffix);
        }

        public String GetStemString() {
            return _stem;
        }
    }
}