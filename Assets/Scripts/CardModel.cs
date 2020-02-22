using System;
using UnityEngine;

public class CardModel : MonoBehaviour
{

    public Sprite[] cardFaces;
    public Sprite cardBack;

    public Card thisCard;
    public bool showing;

    const float SLIDE_TIME = 1; // one second
    bool sliding = false;
    float startTime;
    Vector3 startPosition;
    Vector3 endPosition;
    Action onComplete;

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
        if (sliding)
        {
            float progress = (Time.time - startTime) / SLIDE_TIME;
            if (progress >= 1)
            {
                transform.position = endPosition;
                sliding = false;
                onComplete();
            }
            else
            {
                transform.position = Vector3.Lerp(startPosition, endPosition, progress);
            }
        }
    }

    private void OnMouseDown()
    {
        if (gameObject.CompareTag("in hand"))
        {
            gameObject.GetComponentInParent<HandModel>().ClickCard(this);
        }
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
            spriteIndex = spriteIndex % 13 == 0 ? spriteIndex - 13 : spriteIndex;
            spriteRenderer.sprite = cardFaces[spriteIndex];
        }
        else
        {
            spriteRenderer.sprite = cardBack;
        }
    }
    
    internal void SlideToPosition(Vector3 playedCardDestination, Action a)
    {
        startTime = Time.time;
        startPosition = transform.position;
        sliding = true;
        endPosition = playedCardDestination;
        onComplete = a;
    }

    public static Comparison<CardModel> basicComparison = (c1, c2) => (int)c1.thisCard.suit * 20 + c1.thisCard.denom - (int)c2.thisCard.suit * 20 - c2.thisCard.denom;

}

