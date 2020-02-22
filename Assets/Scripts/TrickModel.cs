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
    void Awake()
    {
        cards = new Dictionary<Card, GameObject>();
    }

    public void TakeCard(CardModel playedCard)
    {
        playedCard.gameObject.tag = "in trick";
        playedCard.showing = true;
        
        cards.Add(playedCard.thisCard, playedCard.transform.parent.gameObject);

        // snap transform to look at source hand, to get the vector representing one unit towards that player
        Transform sourceHand = playedCard.transform.parent;
        gameObject.transform.LookAt(sourceHand);
        Vector3 playedCardDestination = gameObject.transform.TransformVector(Vector3.forward);
        gameObject.transform.rotation = Quaternion.identity; // snap transform back

        playedCard.transform.parent = gameObject.transform;
        playedCard.SlideToPosition(playedCardDestination);

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
