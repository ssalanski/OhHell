using UnityEngine;

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
