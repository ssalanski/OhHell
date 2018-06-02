using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrickModel : MonoBehaviour
{
    public GameObject cardPrefab;
    
    public Dictionary<Card, GameObject> cards;
    public Suit? lead; // is using nullable '?' the best practice?

    // Use this for initialization
    void Start()
    {
        cards = new Dictionary<Card, GameObject>();
    }

    public void TakeCard(GameObject playedCard)
    {
        playedCard.tag = "in trick";
        CardModel cardModel = playedCard.GetComponent<CardModel>();
        cardModel.showing = true;

        cards.Add(cardModel.thisCard, cardModel.transform.parent.gameObject);
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

    internal GameObject GetWinner(Suit trumpSuit)
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
