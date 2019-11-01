using System.Collections.Generic;
using NUnit.Framework;

namespace Poker.Tests
{
    [TestFixture]
    public class LinqExtensionsTests
    {
        [TestCase(new int[] { }, new int[] { })]
        [TestCase(new[] { 0 },
            new int[] { }, new[] { 0 })]
        [TestCase(new[] { 1, 2 },
            new int[] { }, new[] { 1 }, new[] { 2 }, new[] { 1, 2 })]
        [TestCase(new[] { 1, 2, 3 },
            new int[] { }, new[] { 1 }, new[] { 2 }, new[] { 3 },
            new[] { 1, 2 }, new[] { 2, 3 }, new[] { 1, 3 }, new[] { 1, 2, 3 })]
        public void SubsetsTests(IEnumerable<int> set, params IEnumerable<int>[] expectedSubsets)
            => CollectionAssert.AreEquivalent(expectedSubsets, set.Subsets());

        [TestCase(new[] { 0 }, new[] { 1, 2, 3 },
            new[] { 0, 1 }, new[] { 0, 2 }, new[] { 0, 3 })]
        [TestCase(new[] { 1, 2 }, new[] { 3, 4 },
            new[] { 1, 3 }, new[] { 1, 4 }, new[] { 2, 3 }, new[] { 2, 4 })]
        [TestCase(new[] { 1, 2 }, new[] { 3, 4, 5 },
            new[] { 1, 3 }, new[] { 1, 4 }, new[] { 1, 5 }, new[] { 2, 3 }, new[] { 2, 4 }, new[] { 2, 5 })]
        public void TwoSetsCrossProductTests(IEnumerable<int> firstSet, IEnumerable<int> secondSet, params IEnumerable<int>[] expectedSubsets)
            => CollectionAssert.AreEquivalent(expectedSubsets, new[] { firstSet, secondSet }.CrossProduct());

        [TestCase(new[] { 0 }, new[] { 1, 2 }, new[] { 3, 4, },
            new[] { 0, 1, 3 }, new[] { 0, 1, 4 }, new[] { 0, 2, 3 }, new[] { 0, 2, 4 })]
        public void ThreeSetsCrossProductTests(IEnumerable<int> firstSet, IEnumerable<int> secondSet, IEnumerable<int> thirdSet,
            params IEnumerable<int>[] expectedSets)
            => CollectionAssert.AreEquivalent(expectedSets, new[] { firstSet, secondSet, thirdSet }.CrossProduct());

        [TestCase(new int[] { },
            new int[] { })]
        [TestCase(new[] { 0 },
            new[] { 0 })]
        [TestCase(new[] { 1, 2 },
            new[] { 1, 2 }, new[] { 2, 1 })]
        [TestCase(new[] { 1, 2, 3 },
            new[] { 1, 2, 3 }, new[] { 1, 3, 2 }, new[] { 2, 1, 3 }, new[] { 2, 3, 1 }, new[] { 3, 1, 2 }, new[] { 3, 2, 1 })]
        public void PermutateTests(IEnumerable<int> set, params IEnumerable<int>[] expectedPermutations)
            => CollectionAssert.AreEquivalent(expectedPermutations, set.Permutate());

        [TestCase(new[] { 1, 2, 3 }, 2,
            new[] { 1, 2 }, new[] { 1, 3 }, new[] { 2, 3 })]
        public void CombinationsTests(IEnumerable<int> set, int count, params IEnumerable<int>[] expectedCombinations)
            => CollectionAssert.AreEquivalent(expectedCombinations, set.Combinations(count));

        [TestCase(new[] { 1, 2, 3 }, 2,
            new[] { 1, 2 }, new[] { 1, 3 }, new[] { 2, 1 }, new[] { 2, 3 }, new[] { 3, 1 }, new[] { 3, 2 })]
        public void ArrangementsTests(IEnumerable<int> set, int count, params IEnumerable<int>[] expectedArrangements)
            => CollectionAssert.AreEquivalent(expectedArrangements, set.Arrangements(count));
    }
}
