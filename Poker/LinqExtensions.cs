using System.Collections.Generic;
using System.Linq;

namespace Poker
{
    public static class LinqExtensions
    {
        public static IEnumerable<IEnumerable<T>> CrossProduct<T>(this IEnumerable<IEnumerable<T>> sets)
        {
            if (!sets.Any())
            {
                yield return Enumerable.Empty<T>();
                yield break;
            }

            foreach (var element in sets.First())
            {
                foreach (var tail in sets.Skip(1).CrossProduct().ToList())
                {
                    yield return tail.Prepend(element);
                }
            }
        }

        public static IEnumerable<IEnumerable<T>> Subsets<T>(this IEnumerable<T> set)
        {
            if (!set.Any()) return new[] { Enumerable.Empty<T>() };

            var element = set.First();
            var subsets = Subsets(set.Skip(1)).ToList();

            return subsets.Concat(subsets.Select(subset => subset.Prepend(element)));
        }

        public static IEnumerable<IEnumerable<T>> Permutate<T>(this IEnumerable<T> source)
        {
            IEnumerable<T[]> permutate(IEnumerable<T> remainder, IEnumerable<T> prefix) => !remainder.Any()
                ? new[] { prefix.ToArray() }
                : remainder.SelectMany((c, i) => permutate(remainder.Take(i).Concat(remainder.Skip(i + 1)).ToList(), prefix.Append(c)));

            return permutate(source, Enumerable.Empty<T>());
        }

        public static IEnumerable<IEnumerable<T>> Combinations<T>(this IEnumerable<T> source, int count)
        {
            if (count == 0) return new[] { Enumerable.Empty<T>() };

            return source.SelectMany((set, index) => source.Skip(index + 1).Combinations(count - 1).Select(c => c.Prepend(set)));
        }

        public static IEnumerable<IEnumerable<T>> Arrangements<T>(this IEnumerable<T> source, int count) =>
            source.Combinations(count).SelectMany(combination => combination.Permutate().ToList());
    }
}
