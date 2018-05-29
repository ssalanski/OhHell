using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : MonoBehaviour {

    public string playerName;
    private List<TrickModel> tricksTaken;

    public int currentBid;
    public int tricksTakenCount = 0;

	// Use this for initialization
	void Start () {
        tricksTaken = new List<TrickModel>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void TakeTrick(TrickModel trick)
    {
        tricksTaken.Add(trick);
        tricksTakenCount++;
    }

    internal int MakeBid(int bidTotal, int cardCount, bool restricted = false)
    {
        // random bidding, even for player (for now)
        int bid = UnityEngine.Random.Range(0, cardCount + 1);
        // re-bid if we're the dealer and our chosen bid isnt allowed
        while (restricted && bid + bidTotal == cardCount)
        {
            bid = UnityEngine.Random.Range(0, cardCount + 1);
        }
        currentBid = bid;
        return bid;
    }
}
