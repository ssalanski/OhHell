
using System.Collections.Generic;

public class AIPlayerModel : PlayerModel
{
    public override int MakeBid(int bidTotal, int cardCount, bool restricted = false)
    {
        // MAX BIDDING
        int bid = cardCount;
        // re-bid if we're the dealer and our chosen bid isnt allowed
        while (restricted && bid + bidTotal == cardCount)
        {
            bid = UnityEngine.Random.Range(0, cardCount + 1);
        }
        SetBid(bid);
        return bid;
    }

    public override void SetTurnFlag(bool isYourTurn)
    {
        base.SetTurnFlag(isYourTurn);
        if(IsYourTurn())
        {
            CardModel chosen = ChooseCard(gameObject.GetComponentInParent<HandModel>().getCards());
            gameObject.GetComponentInParent<HandModel>().PlayCard(chosen);
        }
    }

    public CardModel ChooseCard(List<CardModel> cardsInHand)
    {
        // wow such sophisticated AI
        return cardsInHand[0];
    }
}
