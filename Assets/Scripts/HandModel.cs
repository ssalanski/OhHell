using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandModel : MonoBehaviour
{
    List<CardModel> cards;
    public GameObject cardPrefab;
    
    void Start()
    {
        cards = new List<CardModel>();
    }

    public void TakeCard(Card c)
    {
        GameObject uiCard;
        uiCard = Instantiate(cardPrefab, gameObject.transform);
        uiCard.GetComponent<SpriteRenderer>().sortingLayerName = "Hand";
        CardModel cm1 = uiCard.GetComponent<CardModel>();
        cm1.showing = true;
        cm1.SetCard(c);

        cards.Add(cm1);
        OrganizeCards();
    }

    internal void PlayCard(CardModel cardModel)
    {
        cards.Remove(cardModel);
        Destroy(cardModel.gameObject);
    }

    internal void SelectCard(CardModel selectedCardModel)
    {
        foreach (CardModel cm in cards)
        {
            cm.SetSelected(cm.Equals(selectedCardModel));
        }
    }

    internal void OrganizeCards()
    {
        cards.Sort(CardModel.basicComparison);
        // spread out cards
        for (int i = 0; i < cards.Count; i++)
        {
            cards[i].transform.localPosition = new Vector3(((cards.Count - 1) * (-0.5f) + i) * 0.35f, 0, -0.1f * i);
            cards[i].GetComponent<SpriteRenderer>().sortingOrder = i;
        }
    }
}
