using System.Collections.Generic;
using Random = UnityEngine.Random;

public enum Suit { Clubs, Diamonds, Hearts, Spades }

[System.Serializable]
public class Card
{
    public static Suit[] Suits = { Suit.Clubs, Suit.Diamonds, Suit.Hearts, Suit.Spades };
    public int denom;
    public Suit suit;

    public Card(Suit s, int d)
    {
        denom = d;
        suit = s;
    }

    public bool Beats(Card other, Suit lead, Suit trump)
    {
        if (other == null)
        {
            return true;
        }
        if (other.suit == trump)
        {
            if (this.suit == trump)
            {
                return this.denom > other.denom;
            }
            else
            {
                return false;
            }
        }
        else
        {
            if (this.suit == trump)
            {
                return true;
            }
            else
            {
                if (other.suit == lead)
                {
                    if (this.suit == lead)
                    {
                        return this.denom > other.denom;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    if (this.suit == lead)
                    {
                        return true;
                    }
                    else
                    {
                        return this.denom > other.denom;
                    }
                }
            }
        }
    }

}

public class Deck
{

    private Stack<Card> deck;

    public Deck(bool shuffled = true)
    {
        List<Card> cards = new List<Card>(52);
        foreach (Suit s in Card.Suits)
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

    internal Card DrawCard()
    {
        return deck.Pop();
    }
}
