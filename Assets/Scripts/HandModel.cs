﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandModel : MonoBehaviour
{
    List<CardModel> cards;
    public GameObject cardPrefab;

    private CardModel selectedCardModel;
    public bool yourTurn = false;

    private void Awake()
    {
        cards = new List<CardModel>();
        selectedCardModel = null;
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
        if (yourTurn)
        {
            selectedCardModel = null;
            cards.Remove(cardModel);
            TrickModel trick = GetComponentInParent<GameManager>().GetComponentInChildren<TrickModel>();
            trick.TakeCard(cardModel.gameObject);
            OrganizeCards();
            Debug.Log("its no longer your turn! " + gameObject.GetHashCode());
            yourTurn = false;
        }
        else
        {
            Debug.Log("its not your turn!");
        }
    }

    internal void ClickCard(CardModel clickedCardModel)
    {
        if (clickedCardModel == selectedCardModel)
        {
            PlayCard(selectedCardModel);
        }
        else
        {
            selectedCardModel = clickedCardModel;
            foreach (CardModel cm in cards)
            {
                Vector3 pos = cm.transform.localPosition;
                pos.y = cm.Equals(selectedCardModel) ? 0.5f : 0.0f;
                cm.transform.localPosition = pos;
            }
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

    internal void TakeTurn(TrickModel currentTrick, Suit suit)
    {
        // implement lead following enforcement later
        Debug.Log("its your turn now! " + gameObject.GetHashCode());
        yourTurn = true;
    }
}
