using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PlayListGenerator
{

    public class NumericPrefixSortComparer : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            if (string.IsNullOrEmpty(x))
                throw new ArgumentException($"Argument {nameof(x)} is null or empty");
            if (string.IsNullOrEmpty(y))
                throw new ArgumentException($"Argument {nameof(y)} is null or empty");

            const string RegexDecimalPattern = @"^\d*";
            string prefixStrX = Regex.Match(x, RegexDecimalPattern).Value;
            string prefixStrY = Regex.Match(y, RegexDecimalPattern).Value;
            bool hasPrefixX = !string.IsNullOrEmpty(prefixStrX);
            bool hasPrefixY = !string.IsNullOrEmpty(prefixStrY);

            if (!hasPrefixX && !hasPrefixY)
                return string.Compare(x, y, true);

            if (hasPrefixX && !hasPrefixY)
                return -1;

            if (!hasPrefixX && hasPrefixY)
                return 1;

            Debug.Assert(hasPrefixX && hasPrefixY);

            int prefixX = Convert.ToInt32(prefixStrX);
            int prefixY = Convert.ToInt32(prefixStrY);

            if (prefixX < prefixY)
                return -1;
            if (prefixX > prefixY)
                return 1;

            Debug.Assert(prefixX == prefixY);
            return string.Compare(x, y, true);
        }
    }
}

