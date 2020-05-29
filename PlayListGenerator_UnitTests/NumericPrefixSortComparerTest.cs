using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PlayListGenerator;

namespace PlayListGenerator_UnitTests
{
    [TestClass]
    public class NumericPrefixSortComparerTest
    {
        [TestMethod]
        public void ShouldSortCorrectly()
        {
            var listInput = new List<string>()
            {
                "foo",
                "16 - foo",
                "6 - bar",
                "1 - bar",
                "7 - bar",
                "bar",
            };

            var listExpected = new List<string>()
            {
                "1 - bar",
                "6 - bar",
                "7 - bar",
                "16 - foo",
                "bar",
                "foo",
            };

            // 1st test (simple test)
            listInput.Sort(new NumericPrefixSortComparer());
            Assert.IsTrue(Enumerable.SequenceEqual<string>(listInput, listExpected));

            // 2nd test : test all permutations
            var permutations = listInput.Permute();
            foreach (var input in permutations)
            {
                var inputList = new List<string>(input);
                inputList.Sort(new NumericPrefixSortComparer());
                Assert.IsTrue(Enumerable.SequenceEqual<string>(inputList, listExpected));
            }

            // 3rd test
            listInput = new List<string>()
            {
                "1.Course Overview",
                "10.Read - only and Immutable Collections",
                "11.Collection Interfaces",
                "2.Arrays, Lists, and Collection Equality",
                "3.Collection Performance",
                "4.Inside Dictionaries and Sorted Dictionaries",
                "5.High - performance Modifications with Linked Lists",
                "6.Stacks",
                "7.Queues",
                "8.Concurrency and Concurrent Collections",
                "9.Merging Data with HashSets and SortedSets",
            };

            listExpected = new List<string>()
            {
                "1.Course Overview",
                "2.Arrays, Lists, and Collection Equality",
                "3.Collection Performance",
                "4.Inside Dictionaries and Sorted Dictionaries",
                "5.High - performance Modifications with Linked Lists",
                "6.Stacks",
                "7.Queues",
                "8.Concurrency and Concurrent Collections",
                "9.Merging Data with HashSets and SortedSets",
                "10.Read - only and Immutable Collections",
                "11.Collection Interfaces",
            };

            listInput.Sort(new NumericPrefixSortComparer());
            Assert.IsTrue(Enumerable.SequenceEqual<string>(listInput, listExpected));

        }
    }
}
