using System.Collections.Generic;
using UnityEngine;

public class HandModel : MonoBehaviour
{
    public List<CardModel> cards;
    public GameObject cardPrefab;

    // Use this for initialization
    void Start()
    {
        cards = new List<CardModel>();

        GameObject newCard = Instantiate(cardPrefab,gameObject.transform);
        newCard.GetComponent<SpriteRenderer>().sortingLayerName = "Hand";
        CardModel cm = newCard.GetComponent<CardModel>();
        cm.showing = true;
        cm.SetCard(new Card(Suit.Diamonds, 8));

        cards.Add(cm);

    }
}
