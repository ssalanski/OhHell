using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandModel : MonoBehaviour
{
    List<CardModel> cards;
    public GameObject cardPrefab;

    private void Awake()
    {
        cards = new List<CardModel>();
    }

    void Start()
    {
        
    }

    public void TakeCard(Card c)
    {
        GameObject uiCard;
        uiCard = Instantiate(cardPrefab, gameObject.transform);
        uiCard.GetComponent<SpriteRenderer>().sortingLayerName = "Hand";
        uiCard.tag = "in hand";
        CardModel cm1 = uiCard.GetComponent<CardModel>();
        cm1.showing = true;
        cm1.SetCard(c);

        cards.Add(cm1);
        OrganizeCards();
    }

    internal void PlayCard(CardModel cardModel)
    {
        cards.Remove(cardModel);
        TrickModel trick = GetComponentInParent<GameManager>().GetComponentInChildren<TrickModel>();
        trick.TakeCard(cardModel.gameObject);
        OrganizeCards();
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
