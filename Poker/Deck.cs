using System;
using System.Collections.Generic;
using System.Linq;

namespace Poker
{
    public interface IDeck
    {
        public int Count { get; }

        public bool IsEmpty { get { return Count == 0; } }

        public bool CanDraw { get { return Count > 0; } }

        public Card<Rank> Draw();

        public IEnumerable<Card<Rank>> Draw(int amount) => amount > 0
            ? Enumerable.Range(0, amount).Select(_ => Draw()).ToList()
            : throw new ArgumentOutOfRangeException(nameof(amount));

        public void Burn() => Draw();
    }

    public sealed class Deck : IDeck
    {
        private static readonly Random _random = new Random();

        private readonly Queue<Card<Rank>> _cards;

        public Deck(int jokers = 0)
        {
            if (jokers < 0) throw new ArgumentOutOfRangeException(nameof(jokers));

            static IEnumerable<Card<Rank>> createCards()
            {
                foreach (var rank in new[] { Rank.Two, Rank.Three, Rank.Four, Rank.Five, Rank.Six, Rank.Seven, Rank.Eight, Rank.Nine, Rank.Ten, Rank.Jack, Rank.Queen, Rank.King, Rank.Ace })
                {
                    foreach (var suit in new[] { Suit.Clubs, Suit.Diamonds, Suit.Hearts, Suit.Spades })
                    {
                        yield return new Card<Rank>(rank, suit);
                    }
                }
            }

            _cards = new Queue<Card<Rank>>(Enumerable
                .Repeat(new Card<Rank>(Rank.Joker, Suit.None), jokers)
                .Union(createCards())
                .OrderBy(_ => _random.Next()));
        }

        public int Count => _cards.Count;

        public Card<Rank> Draw() => _cards.Any() ? _cards.Dequeue() : throw new InvalidOperationException("No cards left.");
    }
}
