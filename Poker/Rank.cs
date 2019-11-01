using System.Linq;

namespace Poker
{
    public enum Rank : byte
    {
        Joker = 0,
        // 1 is reserved for Ace
        Two = 2,
        Three = 3,
        Four = 4,
        Five = 5,
        Six = 6,
        Seven = 7,
        Eight = 8,
        Nine = 9,
        Ten = 10,
        Jack = 11,
        Queen = 12,
        King = 13,
        Ace = 14,
    }

    public static class RankExtensions
    {
        public static string Symbol(this Rank rank) => (byte)rank switch
        {
            var value when value < 10 => value.ToString(),
            _ => rank.ToString().Substring(0, 1),
        };
    }
}
