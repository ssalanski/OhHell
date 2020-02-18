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

    public void TakeCard(CardModel playedCard)
    {
        playedCard.gameObject.tag = "in trick";
        playedCard.showing = true;
        
        cards.Add(playedCard.thisCard, playedCard.transform.parent.gameObject);
        Transform sourceHand = playedCard.transform.parent;

        playedCard.transform.parent = gameObject.transform;
        playedCard.transform.localPosition = new Vector3(0, 0, 0);
        playedCard.transform.Translate(Vector3.down, sourceHand);

        if (lead == null)
        {
            lead = playedCard.thisCard.suit;
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
