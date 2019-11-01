# .NET Core Poker implementation using C# 8 features

## Main Features
* can evaluate 5+ card hands (even with joker cards) and compare their strenghts
* contains an implementation to compute winning odds based on hole cards, drawn community card, and flopped cards
* contains a basic implementation of a shuffled deck that you can draw cards from
* can parse cards or whole hands from strings

## Remarks
Algorithms are mathematically sound but most likely not optimal.
Inspired by an older F# algorithm I once saw, this was written in a day to try out the powerful and expressive new features of C# 8 (mainly the new `switch` expression that I've highly abused).
All algorithms are meant to follow the intuition one has by understanding the game's mechanics. LINQ was used a lot too.

## Sample Usage

```csharp
void PlayGame(int players)
{
    // create a deck (already shuffled and meant to be consumed only once)
    IDeck deck = new Deck();

    // draw two hole cards for each player
    var holeCards = Enumerable.Range(0, players).Select(_ => new Hand(deck.Draw(2))).ToList();

    // draw the flop
    var flop = deck.Draw(3);

    // burn a card if you want
    deck.Burn();

    // draw the turn
    var turn = deck.Draw();

    // burn another card
    deck.Burn();

    // and draw the river
    var river = deck.Draw();

    // put all five community cards together
    var communityCards = flop.Append(turn).Append(river);

    // inspect this in debugger and you can see the strength of each player's hand
    var hands = holeCards.ToDictionary(hand => hand, cards => new Hand(cards.Concat(communityCards)));
}
```
