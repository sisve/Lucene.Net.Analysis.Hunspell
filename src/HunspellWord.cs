using System;
using System.Linq;

namespace Lucene.Net.Analysis.Hunspell {
    public class HunspellWord {
        private readonly Char[] _flags;

        /// <summary>
        ///   Creates a new HunspellWord with no associated flags.
        /// </summary>
        public HunspellWord() : this(new Char[0]) {
        }

        /// <summary>
        ///   Constructs a new HunspellWord with the given flags.
        /// </summary>
        /// <param name="flags">Flags to associate with the word.</param>
        public HunspellWord(Char[] flags) {
            if (flags == null) 
                throw new ArgumentNullException("flags");

            _flags = flags;
        }

        /// <summary>
        ///   Checks whether the word has the given flag associated with it.
        /// </summary>
        /// <param name="flag">Flag to check whether it is associated with the word.</param>
        /// <returns><c>true</c> if the flag is associated, <c>false</c> otherwise</returns>
        public Boolean HasFlag(Char flag) {
            return _flags.Contains(flag);
        }
    }
}
