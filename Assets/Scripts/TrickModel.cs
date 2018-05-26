using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrickModel : MonoBehaviour
{

    Dictionary<Card, HandModel> cards;
    public GameObject cardPrefab;
    public Suit? lead; // is using nullable '?' the best practice?

    // Use this for initialization
    void Start()
    {
        cards = new Dictionary<Card, HandModel>();
    }

    public void TakeCard(GameObject playedCard)
    {
        playedCard.tag = "in trick";
        CardModel cardModel = playedCard.GetComponent<CardModel>();
        cardModel.showing = true;

        cards.Add(cardModel.thisCard, cardModel.GetComponentInParent<HandModel>());
        Transform sourceHand = cardModel.transform.parent;

        cardModel.transform.parent = gameObject.transform;
        cardModel.transform.localPosition = new Vector3(0, 0, 0);
        cardModel.transform.Translate(Vector3.down, sourceHand);

        if (lead == null)
        {
            lead = cardModel.thisCard.suit;
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    internal HandModel GetWinner(Suit trumpSuit)
    {
        Card winning = null;
        foreach (Card cm in cards.Keys)
        {
            if (cm.Beats(winning, lead.Value, trumpSuit))
            {
                winning = cm;
            }
        }
        return cards[winning];
    }
}
