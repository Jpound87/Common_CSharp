using System;
using System.Collections.Generic;
using System.Linq;

namespace Common
{
    public class FMIndexer
    {
        #region Identity
        public const String ClassName = nameof(FMIndexer);
        #endregion

        #region Readonly
        private readonly string text;
        private readonly int[] suffixArray;
        private readonly int[] count;
        private readonly Dictionary<char, int> charToRank;
        private readonly int[] cumulativeCount;
        private readonly int[] firstOccurrence;
        #endregion

        #region Constructor
        public FMIndexer(string text)
        {
            this.text = text;

            // Step 1: Construct the suffix array
            suffixArray = Enumerable.Range(0, text.Length)
                .OrderBy(i => text.Substring(i))
                .ToArray();

            // Step 2: Construct the Burrows-Wheeler Transform (BWT) of the text
            var bwt = new char[text.Length];
            for (int i = 0; i < text.Length; i++)
            {
                bwt[i] = (suffixArray[i] == 0) ? '$' : text[suffixArray[i] - 1];
            }

            // Step 3: Construct the rank and C arrays
            charToRank = bwt.Distinct().OrderBy(c => c).Select((c, i) => new { c, i }).ToDictionary(x => x.c, x => x.i);
            count = new int[charToRank.Count];
            foreach (var c in bwt)
            {
                count[charToRank[c]]++;
            }
            cumulativeCount = count
                .Aggregate(new List<int> { 0 }, (acc, c) => { acc.Add(acc.Last() + c); return acc; })
                .ToArray();

            // Step 4: Construct the first occurrence array
            firstOccurrence = new int[charToRank.Count];
            int first = 0;
            foreach (var c in charToRank.Keys.OrderBy(c => c))
            {
                firstOccurrence[charToRank[c]] = first;
                first += count[charToRank[c]];
            }
        }
        #endregion /Constructor

        #region Search
        public IEnumerable<int> Search(string pattern)
        {
            int top = 0;
            int bottom = text.Length - 1;
            for (int i = pattern.Length - 1; i >= 0; i--)
            {
                char c = pattern[i];
                int rank = charToRank[c];
                top = cumulativeCount[rank] + firstOccurrence[rank];
                bottom = cumulativeCount[rank] + firstOccurrence[rank + 1] - 1;
                if (top > bottom)
                {
                    break;
                }
            }

            if (top > bottom)
            {
                yield break;
            }

            for (int i = top; i <= bottom; i++)
            {
                yield return suffixArray[i];
            }
        }
        #endregion /Search
    }
}
