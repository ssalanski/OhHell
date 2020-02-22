using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandModel : MonoBehaviour
{
    List<CardModel> cards;
    public GameObject cardPrefab;

    private TrickModel currentTrick;
    private CardModel selectedCardModel;
    private bool isFaceUp = false;

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
        GameObject uiCard = Instantiate(cardPrefab, gameObject.transform);
        uiCard.GetComponent<SpriteRenderer>().sortingLayerName = "Hand";
        uiCard.tag = "in hand";
        CardModel cm1 = uiCard.GetComponent<CardModel>();
        cm1.showing = isFaceUp;
        cm1.SetCard(c);

        cards.Add(cm1);
        OrganizeCards();
    }

    internal List<CardModel> getCards()
    {
        return cards;
    }

    internal void PlayCard(CardModel cardModel)
    {
        if (GetComponentInParent<PlayerModel>().IsYourTurn())
        {
            selectedCardModel = null;
            cards.Remove(cardModel);
            cardModel.ShowCard();
            currentTrick.TakeCard(cardModel);
            OrganizeCards();
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
            // enforce following suit
            if (cards.Find(c => c.thisCard.suit == currentTrick.lead) != null && clickedCardModel.thisCard.suit != currentTrick.lead)
            {
                // if you have at least one card in the lead suit, then dont allow selecting a card that cant be played
                return;
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
    }

    internal void OrganizeCards()
    {
        cards.Sort(CardModel.basicComparison);
        // spread out cards
        for (int i = 0; i < cards.Count; i++)
        {
            cards[i].transform.localPosition = new Vector3(((cards.Count - 1) * (-0.5f) + i) * 0.35f, 0, -0.1f * i - 0.5f);
            cards[i].GetComponent<SpriteRenderer>().sortingOrder = i;
        }
    }

    internal void SetCurrentTrick(TrickModel newCurrentTrick)
    {
        currentTrick = newCurrentTrick;
    }

    internal void setFaceUp(bool v)
    {
        isFaceUp = true;
    }
}
