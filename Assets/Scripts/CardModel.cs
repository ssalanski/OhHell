using UnityEngine;

public class CardModel : MonoBehaviour
{

    public Sprite[] clubs_faces;
    public Sprite[] diamonds_faces;
    public Sprite[] hearts_faces;
    public Sprite[] spades_faces;
    public Sprite cardback;

    public Card card;
    public bool showing;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetCard(Card c)
    {
        card = c;
        UpdateSprite();
    }

    public void Hide()
    {
        showing = false;
        UpdateSprite();
    }

    public void Show()
    {
        showing = true;
        UpdateSprite();
    }

    private void UpdateSprite()
    {
        if (showing)
        {
            int index = (card.denom - 1) % 13;
            switch (card.suit)
            {
                case Suit.Clubs:
                    spriteRenderer.sprite = clubs_faces[index];
                    break;
                case Suit.Diamonds:
                    spriteRenderer.sprite = diamonds_faces[index];
                    break;
                case Suit.Hearts:
                    spriteRenderer.sprite = hearts_faces[index];
                    break;
                case Suit.Spades:
                    spriteRenderer.sprite = spades_faces[index];
                    break;
            }
        }
        else
        {
            spriteRenderer.sprite = cardback;
        }
    }
    
}
