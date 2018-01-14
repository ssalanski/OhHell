using UnityEngine;

public enum Suit
{
    Clubs, Diamonds, Hearts, Spades
}

// extensions class to provide a .getAbbr() method on the Suit enum
public static class Extensions
{
    public static string GetAbbr(this Suit suit)
    {
        return suit.ToString().Substring(0, 1);
    }
}

public class Card
{
    public int denom;
    public Suit suit;

    public Card(Suit suit, int denom)
    {
        this.denom = denom;
        this.suit = suit;
        if (denom < 2 || denom > 14)
        {
            throw new UnityException("Can't have a card with denomination " + denom.ToString());
        }
    }

    public string GetName()
    {
        return GetDenomName() + " of " + suit.ToString();
    }

    public string GetAbbr()
    {
        return (denom == 14 ? "A" : denom.ToString()) + suit.GetAbbr();
    }

    private string GetDenomName()
    {
        switch (denom)
        {
            case (14):
                return "Ace";
            case (2):
                return "Two";
            case (3):
                return "Three";
            case (4):
                return "Four";
            case (5):
                return "Five";
            case (6):
                return "Six";
            case (7):
                return "Seven";
            case (8):
                return "Eight";
            case (9):
                return "Nine";
            case (10):
                return "Ten";
            case (11):
                return "Jack";
            case (12):
                return "Queen";
            case (13):
                return "King";
            default:
                return "NOTHING!";
        }
    }
}