using System;
using System.Collections.Generic;
using System.Linq;

namespace Poker
{
    public struct HandStrength : IComparable<HandStrength>
    {
        public readonly HandRank HandRank { get; }

        public readonly IReadOnlyList<byte> Kickers { get; }

        public HandStrength(in HandRank handRank, params byte[] kickers) => (HandRank, Kickers) = (handRank, kickers.ToList());

        public HandStrength(Hand<byte> hand)
        {
            if (hand.Count != 5)
            {
                (HandRank, Kickers) = (HandRank.None, new List<byte>());
                return;
            }

            var isStraight = hand.Zip(hand.Skip(1)).All(pair => pair.First.Rank - pair.Second.Rank == 1);
            var isFlush = hand.Select(card => card.Suit).Distinct().Count() == 1;

            var groups = hand
                .GroupBy(card => card.Rank)
                .Select(group => (Count: group.Count(), Rank: group.Key))
                .OrderByDescending(group => group.Count)
                .ThenByDescending(group => group.Rank)
                .ToList();

            var counts = new
            {
                First = groups.Select(group => group.Count).First(),
                Second = groups.Select(group => group.Count).Skip(1).FirstOrDefault(),
                Third = groups.Select(group => group.Count).Skip(2).FirstOrDefault(),
                Fourth = groups.Select(group => group.Count).Skip(3).FirstOrDefault(),
                Fifth = groups.Select(group => group.Count).Skip(4).FirstOrDefault(),
            };

            var ranks = groups.Select(rank => rank.Rank).ToList();

            (HandRank, Kickers) = (isStraight, isFlush, counts) switch
            {
                (_, _, { First: 5 }) => (HandRank.FiveOfAKind, ranks),
                (true, true, _) => (HandRank.StraightFlush, ranks.Take(1).ToList()),
                (true, false, _) => (HandRank.Straight, ranks.Take(1).ToList()),
                (false, true, _) => (HandRank.Flush, ranks),
                (_, _, { First: 4 }) => (HandRank.FourOfAKind, ranks),
                (_, _, { First: 3, Second: 2 }) => (HandRank.FullHouse, ranks),
                (_, _, { First: 3 }) => (HandRank.ThreeOfAKind, ranks),
                (_, _, { First: 2, Second: 2 }) => (HandRank.TwoPair, ranks),
                (_, _, { First: 2 }) => (HandRank.OnePair, ranks),
                (_, _, _) => (HandRank.HighCard, ranks),
            };
        }

        public readonly int CompareTo(HandStrength other) => HandRank.CompareTo(other.HandRank) switch
        {
            0 => Kickers.Zip(other.Kickers).Select(pair => pair.First.CompareTo(pair.Second)).FirstOrDefault(result => result != 0),
            var result => result,
        };

        public readonly override string ToString() => HandRank switch
        {
            HandRank.None => $"Not evaluated",
            HandRank.FiveOfAKind => $"{HandRank} of {Kickers.First()}",
            HandRank.StraightFlush when Kickers.First() == (byte)Rank.Ace => $"Royal {HandRank}",
            HandRank.StraightFlush => $"{HandRank} {Kickers.First()} high",
            HandRank.Straight => $"{HandRank} {Kickers.First()} high",
            _ => $"{HandRank} of {Kickers.First()} and {string.Join(", ", Kickers.Skip(1))}",
        };
    }
}
