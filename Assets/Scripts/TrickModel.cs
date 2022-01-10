using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrickModel : MonoBehaviour
{
    public GameObject cardPrefab;

    public Dictionary<CardModel, PlayerModel> cards;
    public Suit trumpSuit;
    public Suit? lead; // is using nullable '?' the best practice?
    public CardModel winning = null;

    const float SLIDE_TIME = 1; // one second
    bool sliding = false;
    float startTime;
    Vector3 startPosition;
    Vector3 endPosition;
    Action onComplete;

    // Use this for initialization
    void Awake()
    {
        cards = new Dictionary<CardModel, PlayerModel>();
    }

    public void TakeCard(CardModel playedCard)
    {
        playedCard.gameObject.tag = "in trick";
        playedCard.showing = true;

        cards.Add(playedCard, playedCard.GetComponentInParent<PlayerModel>());

        // snap transform to look at source hand, to get the vector representing one unit towards that player
        Transform sourceHand = playedCard.transform.parent;
        gameObject.transform.LookAt(sourceHand);
        Vector3 playedCardDestination = gameObject.transform.TransformVector(Vector3.forward);
        gameObject.transform.rotation = Quaternion.identity; // snap transform back

        playedCard.transform.parent = gameObject.transform;
        playedCard.SlideToPosition(playedCardDestination, () => sourceHand.gameObject.GetComponentInParent<PlayerModel>().SetTurnFlag(false));

        if (lead == null)
        {
            lead = playedCard.thisCard.suit;
        }

        foreach (CardModel cm in cards.Keys)
        {
            if (cm.thisCard.Beats(winning?.thisCard, lead.Value, trumpSuit))
            {
                winning = cm;
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (sliding)
        {
            float progress = (Time.time - startTime) / SLIDE_TIME;
            if (progress >= 1)
            {
                transform.localPosition = endPosition;
                sliding = false;
                foreach( CardModel cm in cards.Keys)
                {
                    cm.HideCard();
                }
                onComplete();
            }
            else
            {
                transform.localPosition = Vector3.Lerp(startPosition, endPosition, progress);
                foreach (Transform child in transform)
                {
                    child.localRotation = Quaternion.Lerp(child.localRotation, Quaternion.identity, progress * .1f);
                    child.localPosition = Vector3.Lerp(child.localPosition, Vector3.zero, progress * .1f);
                    child.localScale = Vector3.Lerp(transform.localScale, new Vector3(1, 1, 1), progress * 0.1f);
                }
            }
        }
    }

    internal PlayerModel GetWinner(Suit trumpSuit)
    {
        return cards[winning];
    }

    internal void HilightWinner()
    {
        winning.gameObject.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
    }

    internal void SlideToPlayer(int trickSlot)
    {
        startTime = Time.time;
        startPosition = transform.localPosition;
        sliding = true;
        endPosition = Vector3.left * (2 + trickSlot * 0.25f);
        onComplete = delegate () { }; // TODO: implement similar wait-for-slide-completion before proceeding with next trick
    }

    internal void SetTrump(Suit suit)
    {
        trumpSuit = suit;
    }
}
