using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Poker
{
    public class Hand<TRank> : IEnumerable<Card<TRank>> where TRank : struct
    {
        private readonly IReadOnlyList<Card<TRank>> _cards;

        public Hand(IEnumerable<Card<TRank>> cards) => _cards = cards.OrderByDescending(card => card).ToList();

        public Hand(params Card<TRank>[] cards) : this(cards.AsEnumerable()) { }

        public int Count => _cards.Count;

        public override string ToString() => string.Join(" | ", _cards);

        public IEnumerator<Card<TRank>> GetEnumerator() => _cards.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _cards.GetEnumerator();
    }

    public sealed class Hand : Hand<Rank>, IEquatable<Hand>
    {
        public HandStrength Strength { get; }

        public Hand(IEnumerable<Card<Rank>> cards) : base(cards)
        {
            static IEnumerable<Card<byte>> evaluate(Card<Rank> card)
            {
                var ranks = card.Rank switch
                {
                    Rank.Joker => new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 },
                    Rank.Ace => new byte[] { 1, 14 },
                    var other => new[] { (byte)other },
                };

                var suits = card.Suit switch
                {
                    Suit.None => new[] { Suit.Clubs, Suit.Diamonds, Suit.Hearts, Suit.Spades },
                    var other => new[] { other },
                };

                foreach (var rank in ranks)
                {
                    foreach (var suit in suits)
                    {
                        yield return new Card<byte>(rank, suit);
                    }
                }
            }

            Strength = Count switch
            {
                var count when count < 5 => new HandStrength(HandRank.None),
                5 => this
                    .Select(card => evaluate(card).ToList())
                    .CrossProduct()
                    .Select(cards => new HandStrength(new Hand<byte>(cards)))
                    .Max(),
                var count => this
                    .Select(card => evaluate(card).ToList())
                    .CrossProduct()
                    .SelectMany(cards => cards.Subsets())
                    .Select(cards => new Hand<byte>(cards))
                    .Where(cards => cards.Count == 5)
                    .Select(hand => new HandStrength(hand))
                    .Max(),
            };
        }

        public override string ToString() => Strength.HandRank switch
        {
            HandRank.None => base.ToString(),
            _ => $"{base.ToString()} : {Strength}"
        };

        public bool Equals(Hand other) => Enumerable.SequenceEqual(this, other);

        public override bool Equals(object? obj) => obj is Hand ? Equals((Hand)obj) : false;

        public override int GetHashCode() => this.Aggregate(0, (hashCode, card) => hashCode * 10 + card.GetHashCode());

        public static Hand Parse(string input) => new Hand(input
            .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(card => Card<Rank>.Parse(card)));
    }
}
