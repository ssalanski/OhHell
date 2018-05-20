using UnityEngine;

public class CardModel : MonoBehaviour
{

    public Sprite[] cardFaces;
    public Sprite cardBack;

    public Card thisCard;
    public bool showing;

    public bool selected;

    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        SetSprite();
    }

    private void Update()
    {

    }

    internal void SetCard(Card c)
    {
        thisCard = c;
        SetSprite();
    }

    public void SetSuit(Suit s)
    {
        thisCard.suit = s;
        SetSprite();
    }

    public void SetDenom(int d)
    {
        thisCard.denom = d;
        SetSprite();
    }

    public void HideCard()
    {
        showing = false;
        SetSprite();
    }

    public void ShowCard()
    {
        showing = true;
        SetSprite();
    }

    private void SetSprite()
    {
        if (showing)
        {
            int spriteIndex = ((int)thisCard.suit) * 13 + thisCard.denom - 1;
            System.Console.WriteLine(spriteIndex);
            spriteRenderer.sprite = cardFaces[spriteIndex];
        }
        else
        {
            spriteRenderer.sprite = cardBack;
        }
    }

    internal void SetSelected(bool s)
    {
        selected = s;
        Vector3 pos = transform.localPosition;
        pos.y = s ? 0.5f : 0.0f;
        transform.localPosition = pos;
    }

}
