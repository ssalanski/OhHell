
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
}
