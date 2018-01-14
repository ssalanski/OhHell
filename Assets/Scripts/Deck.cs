using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class Deck
{
    private Stack<Card> deck;

    public Deck(bool shuffled)
    {
        List<Card> cards = new List<Card>(52);
        foreach (Suit s in Enum.GetValues(typeof(Suit)))
        {
            for (int d = 2; d <= 14; d++)
            {
                cards.Add(new Card(s, d));
            }
        }
        if (shuffled)
        {
            Card temp;
            int j;
            for (int i = 0; i < cards.Count; i++)
            {
                j = Random.Range(0, i);
                temp = cards[i];
                cards[i] = cards[j];
                cards[j] = temp;
            }
        }
        else
        {
            cards.Reverse();
        }
        deck = new Stack<Card>(cards);
    }

    public Card DrawCard()
    {
        return deck.Pop();
    }
}
