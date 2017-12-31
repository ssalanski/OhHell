using System.Collections;
using System.Collections.Generic;
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

    public Card(int denom, Suit suit)
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

public class CardModel : MonoBehaviour
{

    public Sprite[] clubs_faces;
    public Sprite[] diamonds_faces;
    public Sprite[] hearts_faces;
    public Sprite[] spades_faces;
    public Sprite cardback;

    public bool showing;

    public Card card;

    private Sprite display;

    private void UpdateSprite()
    {
        if (showing)
        {
            int index = (card.denom - 1) % 13;
            switch (card.suit)
            {
                case Suit.Clubs:
                    display = clubs_faces[index];
                    break;
                case Suit.Diamonds:
                    display = diamonds_faces[index];
                    break;
                case Suit.Hearts:
                    display = hearts_faces[index];
                    break;
                case Suit.Spades:
                    display = spades_faces[index];
                    break;
            }
        }
        else
        {
            display = cardback;
        }
    }
    
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
