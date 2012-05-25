using System;
using System.IO;
using Lucene.Net.Analysis.Tokenattributes;
using Lucene.Net.Util;
using NUnit.Framework;

namespace Lucene.Net.Analysis.Hunspell.Tests {
    public abstract class BaseTokenStreamTestCase {
        public static void AssertTokenStreamContents(TokenStream ts, String[] output, Int32[] startOffsets, Int32[] endOffsets, String[] types, Int32[] posIncrements, Int32? finalOffset) {
            Assert.IsNotNull(output);
            var checkClearAtt = (CheckClearAttributesAttribute) ts.AddAttribute(typeof (CheckClearAttributesAttribute));

            Assert.IsTrue(ts.HasAttribute(typeof (TermAttribute)), "has no TermAttribute");
            var termAtt = (TermAttribute) ts.GetAttribute(typeof (TermAttribute));

            OffsetAttribute offsetAtt = null;
            if (startOffsets != null || endOffsets != null || finalOffset != null) {
                Assert.IsTrue(ts.HasAttribute(typeof (OffsetAttribute)), "has no OffsetAttribute");
                offsetAtt = (OffsetAttribute) ts.GetAttribute(typeof (OffsetAttribute));
            }

            TypeAttribute typeAtt = null;
            if (types != null) {
                Assert.IsTrue(ts.HasAttribute(typeof (TypeAttribute)), "has no TypeAttribute");
                typeAtt = (TypeAttribute) ts.GetAttribute(typeof (TypeAttribute));
            }

            PositionIncrementAttribute posIncrAtt = null;
            if (posIncrements != null) {
                Assert.IsTrue(ts.HasAttribute(typeof (PositionIncrementAttribute)), "has no PositionIncrementAttribute");
                posIncrAtt = (PositionIncrementAttribute) ts.GetAttribute(typeof (PositionIncrementAttribute));
            }

            ts.Reset();
            for (Int32 i = 0; i < output.Length; i++) {
                // extra safety to enforce, that the state is not preserved and also assign bogus values
                ts.ClearAttributes();
                termAtt.SetTermBuffer("bogusTerm");
                if (offsetAtt != null) offsetAtt.SetOffset(14584724, 24683243);
                if (typeAtt != null) typeAtt.SetType("bogusType");
                if (posIncrAtt != null) posIncrAtt.SetPositionIncrement(45987657);

                checkClearAtt.GetAndResetClearCalled(); // reset it, because we called clearAttribute() before
                Assert.IsTrue(ts.IncrementToken(), "token " + i + " does not exist");
                Assert.IsTrue(checkClearAtt.GetAndResetClearCalled(), "clearAttributes() was not called correctly in TokenStream chain");

                Assert.AreEqual(output[i], termAtt.Term(), "term " + i);
                if (startOffsets != null)
                    Assert.AreEqual(startOffsets[i], offsetAtt.StartOffset(), "startOffset " + i);
                if (endOffsets != null)
                    Assert.AreEqual(endOffsets[i], offsetAtt.EndOffset(), "endOffset " + i);
                if (types != null)
                    Assert.AreEqual(types[i], typeAtt.Type(), "type " + i);
                if (posIncrements != null)
                    Assert.AreEqual(posIncrements[i], posIncrAtt.GetPositionIncrement(), "posIncrement " + i);
            }
            Assert.IsFalse(ts.IncrementToken(), "end of stream");
            ts.End();
            if (finalOffset.HasValue)
                Assert.AreEqual(finalOffset.Value, offsetAtt.EndOffset(), "finalOffset ");
            ts.Close();
        }

        public static void AssertTokenStreamContents(TokenStream ts, String[] output, Int32[] startOffsets, Int32[] endOffsets, String[] types, Int32[] posIncrements) {
            AssertTokenStreamContents(ts, output, startOffsets, endOffsets, types, posIncrements, null);
        }

        public static void AssertTokenStreamContents(TokenStream ts, String[] output) {
            AssertTokenStreamContents(ts, output, null, null, null, null, null);
        }

        public static void AssertTokenStreamContents(TokenStream ts, String[] output, String[] types) {
            AssertTokenStreamContents(ts, output, null, null, types, null, null);
        }

        public static void AssertTokenStreamContents(TokenStream ts, String[] output, Int32[] posIncrements) {
            AssertTokenStreamContents(ts, output, null, null, null, posIncrements, null);
        }

        public static void AssertTokenStreamContents(TokenStream ts, String[] output, Int32[] startOffsets, Int32[] endOffsets) {
            AssertTokenStreamContents(ts, output, startOffsets, endOffsets, null, null, null);
        }

        public static void AssertTokenStreamContents(TokenStream ts, String[] output, Int32[] startOffsets, Int32[] endOffsets, Int32 finalOffset) {
            AssertTokenStreamContents(ts, output, startOffsets, endOffsets, null, null, finalOffset);
        }

        public static void AssertTokenStreamContents(TokenStream ts, String[] output, Int32[] startOffsets, Int32[] endOffsets, Int32[] posIncrements) {
            AssertTokenStreamContents(ts, output, startOffsets, endOffsets, null, posIncrements, null);
        }

        public static void AssertTokenStreamContents(TokenStream ts, String[] output, Int32[] startOffsets, Int32[] endOffsets, Int32[] posIncrements, Int32 finalOffset) {
            AssertTokenStreamContents(ts, output, startOffsets, endOffsets, null, posIncrements, finalOffset);
        }

        public static void AssertAnalyzesTo(Analyzer a, String input, String[] output, Int32[] startOffsets, Int32[] endOffsets, String[] types, Int32[] posIncrements) {
            AssertTokenStreamContents(a.TokenStream("dummy", new StringReader(input)), output, startOffsets, endOffsets, types, posIncrements, input.Length);
        }

        public static void AssertAnalyzesTo(Analyzer a, String input, String[] output) {
            AssertAnalyzesTo(a, input, output, null, null, null, null);
        }

        public static void AssertAnalyzesTo(Analyzer a, String input, String[] output, String[] types) {
            AssertAnalyzesTo(a, input, output, null, null, types, null);
        }

        public static void AssertAnalyzesTo(Analyzer a, String input, String[] output, Int32[] posIncrements) {
            AssertAnalyzesTo(a, input, output, null, null, null, posIncrements);
        }

        public static void AssertAnalyzesTo(Analyzer a, String input, String[] output, Int32[] startOffsets, Int32[] endOffsets) {
            AssertAnalyzesTo(a, input, output, startOffsets, endOffsets, null, null);
        }

        public static void AssertAnalyzesTo(Analyzer a, String input, String[] output, Int32[] startOffsets, Int32[] endOffsets, Int32[] posIncrements) {
            AssertAnalyzesTo(a, input, output, startOffsets, endOffsets, null, posIncrements);
        }


        public static void AssertAnalyzesToReuse(Analyzer a, String input, String[] output, Int32[] startOffsets, Int32[] endOffsets, String[] types, Int32[] posIncrements) {
            AssertTokenStreamContents(a.ReusableTokenStream("dummy", new StringReader(input)), output, startOffsets, endOffsets, types, posIncrements, input.Length);
        }

        public static void AssertAnalyzesToReuse(Analyzer a, String input, String[] output) {
            AssertAnalyzesToReuse(a, input, output, null, null, null, null);
        }

        public static void AssertAnalyzesToReuse(Analyzer a, String input, String[] output, String[] types) {
            AssertAnalyzesToReuse(a, input, output, null, null, types, null);
        }

        public static void AssertAnalyzesToReuse(Analyzer a, String input, String[] output, Int32[] posIncrements) {
            AssertAnalyzesToReuse(a, input, output, null, null, null, posIncrements);
        }

        public static void AssertAnalyzesToReuse(Analyzer a, String input, String[] output, Int32[] startOffsets, Int32[] endOffsets) {
            AssertAnalyzesToReuse(a, input, output, startOffsets, endOffsets, null, null);
        }

        public static void AssertAnalyzesToReuse(Analyzer a, String input, String[] output, Int32[] startOffsets, Int32[] endOffsets, Int32[] posIncrements) {
            AssertAnalyzesToReuse(a, input, output, startOffsets, endOffsets, null, posIncrements);
        }

        public static void CheckOneTerm(Analyzer a, String input, String expected) {
            AssertAnalyzesTo(a, input, new[] {expected});
        }

        public static void CheckOneTermReuse(Analyzer a, String input, String expected) {
            AssertAnalyzesToReuse(a, input, new[] {expected});
        }

        public interface CheckClearAttributesAttribute : Util.Attribute {
            Boolean GetAndResetClearCalled();
        }

        public class CheckClearAttributesAttributeImpl : AttributeImpl, CheckClearAttributesAttribute {
            private Boolean _clearCalled;

            public Boolean GetAndResetClearCalled() {
                try {
                    return _clearCalled;
                } finally {
                    _clearCalled = false;
                }
            }

            public override void Clear() {
                _clearCalled = true;
            }

            public override bool Equals(object other) {
                return (other is CheckClearAttributesAttributeImpl) &&
                       (((CheckClearAttributesAttributeImpl) other)._clearCalled == _clearCalled);
            }

            public override int GetHashCode() {
                return 76137213 ^ _clearCalled.GetHashCode();
            }

            public override void CopyTo(AttributeImpl target) {
                target.Clear();
            }
        }
    }
}