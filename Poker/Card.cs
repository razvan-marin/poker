using System;
using System.Collections.Generic;

namespace Poker
{
    public struct Card<TRank> : IEquatable<Card<TRank>>, IComparable<Card<TRank>> where TRank : struct
    {
        public readonly TRank Rank { get; }

        public readonly Suit Suit { get; }

        public Card(in TRank rank, in Suit suit) => (Rank, Suit) = (rank, suit);

        public readonly bool Equals(Card<TRank> other) => Rank.Equals(other.Rank) && Suit == other.Suit;

        public readonly int CompareTo(Card<TRank> other) => Comparer<TRank>.Default.Compare(Rank, other.Rank);

        public readonly override bool Equals(object? obj) => obj is Card<TRank> ? Equals((Card<TRank>)obj) : false;

        public readonly override int GetHashCode() => Rank.GetHashCode() * (byte.MaxValue - Suit).GetHashCode();

        public readonly override string ToString() => $"{(Rank is Rank ? ((Rank)(object)Rank).Symbol() : Rank.ToString())} {Suit.Symbol()}";

        public static Card<Rank> Parse(string input)
        {
            input = input.Trim().ToUpperInvariant();

            return input.Length switch
            {
                1 when input == "J" => new Card<Rank>(Poker.Rank.Joker, Suit.None),
                2 => new Card<Rank>(
                    input[0] switch
                    {
                        '2' => Poker.Rank.Two,
                        '3' => Poker.Rank.Three,
                        '4' => Poker.Rank.Four,
                        '5' => Poker.Rank.Five,
                        '6' => Poker.Rank.Six,
                        '7' => Poker.Rank.Seven,
                        '8' => Poker.Rank.Eight,
                        '9' => Poker.Rank.Nine,
                        'T' => Poker.Rank.Ten,
                        'J' => Poker.Rank.Jack,
                        'Q' => Poker.Rank.Queen,
                        'K' => Poker.Rank.King,
                        'A' => Poker.Rank.Ace,
                        _ => throw new ArgumentException("Could not understand rank symbol.", nameof(input)),
                    },
                    input[1] switch
                    {
                        'C' => Suit.Clubs,
                        'D' => Suit.Diamonds,
                        'H' => Suit.Hearts,
                        'S' => Suit.Spades,
                        '♣' => Suit.Clubs,
                        '♢' => Suit.Diamonds,
                        '♤' => Suit.Hearts,
                        '♠' => Suit.Spades,
                        _ => throw new ArgumentException("Could not understand suit symbol.", nameof(input)),
                    }),
                _ => throw new ArgumentException("Input was longer than expected.", nameof(input)),
            };
        }
    }
}
