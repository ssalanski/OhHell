
using System.Collections.Generic;
using UnityEngine;

public class AIPlayerModel : PlayerModel
{
    public override int MakeBid(int bidTotal, int cardCount, bool restricted = false)
    {
        // always bidding 1
        int bid = 1;
        // unless we're the dealer and our chosen bid isnt allowed
        if (restricted && bid + bidTotal == cardCount)
        {
            bid = 0;
        }
        SetBid(bid);
        return bid;
    }

    public override void SetTurnFlag(bool isYourTurn)
    {
        base.SetTurnFlag(isYourTurn);
        if (IsYourTurn())
        {
            CardModel chosen = ChooseCard(GetHand().getCards(), currentTrick);
            GetHand().PlayCard(chosen);
        }
    }

    public CardModel ChooseCard(List<CardModel> cardsInHand, TrickModel currentTrick)
    {
        CardModel[] legalPlays;
        if (cardsInHand.Exists(c => c.thisCard.suit == currentTrick.lead))
        {
            legalPlays = cardsInHand.FindAll(cm => cm.thisCard.suit == currentTrick.lead).ToArray();
        }
        else
        {
            legalPlays = cardsInHand.ToArray();
        }

        return legalPlays[Random.Range(0, legalPlays.Length)];
    }
}
