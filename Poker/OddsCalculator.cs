using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Poker
{
    public class OddsCalculator
    {
        public IDictionary<Hand, decimal> Determine(
            IEnumerable<Hand> hands,
            IEnumerable<Card<Rank>> drawnCommunityCards,
            IEnumerable<Card<Rank>> floppedCards)
        {
            if (hands.Count() < 2)
                throw new ArgumentException("Unexpected number of hands. Expecting hole cards of at least 2 players.", nameof(hands));

            if (hands.Any(hand => hand.Count != 2))
                throw new ArgumentException("Unexpected amount of cards in hand. Expecting hole hands of exactly 2 cards.", nameof(hands));

            if (!new[] { 0, 3, 4 }.Contains(drawnCommunityCards.Count()))
                throw new ArgumentException("Unexpected number of community cards. Expecting 0, 3, or 4.", nameof(drawnCommunityCards));

            var cards = CreateCards().Except(hands.SelectMany(hand => hand)).Except(drawnCommunityCards).Except(floppedCards).ToList();
            var unknownCommunityCards = 5 - drawnCommunityCards.Count();

            var points = new ConcurrentDictionary<Hand, decimal>();

            Parallel.ForEach(cards.Combinations(unknownCommunityCards), nextCommunityCards =>
            {
                var playerHandsWithCommunityCards = hands.ToDictionary(
                    hand => hand,
                    hand => new Hand(hand.Concat(drawnCommunityCards).Concat(nextCommunityCards)));

                var bestHand = playerHandsWithCommunityCards.Select(hand => hand.Value.Strength).Max();

                var winners = playerHandsWithCommunityCards.Where(pair => pair.Value.Strength.Equals(bestHand)).ToList();

                if (winners.Count() == 1)
                {
                    points.AddOrUpdate(winners.Single().Key, 1, (_, current) => current + 1);
                }
                else
                {
                    var gain = 1m / winners.Count;

                    foreach (var winner in winners)
                    {
                        points.AddOrUpdate(winner.Key, gain, (_, current) => current + gain);
                    }
                }
            });

            var total = points.Values.Sum();
            return points.ToDictionary(pair => pair.Key, pair => pair.Value / total);
        }

        private static IEnumerable<Card<Rank>> CreateCards()
        {
            foreach (var rank in new[] { Rank.Two, Rank.Three, Rank.Four, Rank.Five, Rank.Six, Rank.Seven, Rank.Eight, Rank.Nine, Rank.Ten, Rank.Jack, Rank.Queen, Rank.King, Rank.Ace })
            {
                foreach (var suit in new[] { Suit.Clubs, Suit.Diamonds, Suit.Hearts, Suit.Spades })
                {
                    yield return new Card<Rank>(rank, suit);
                }
            }
        }
    }
}