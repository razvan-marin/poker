namespace Poker
{
    public enum Suit : byte
    {
        None = 0,
        Clubs,
        Diamonds,
        Hearts,
        Spades,
    }

    public static class SuitExtensions
    {
        public static string Symbol(this Suit suit) => suit switch
        {
            Suit.Clubs => "♣",
            Suit.Diamonds => "♢",
            Suit.Hearts => "♤",
            Suit.Spades => "♠",
            _ => "?",
        };
    }
}
