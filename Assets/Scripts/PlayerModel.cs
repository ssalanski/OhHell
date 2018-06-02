﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : MonoBehaviour
{

    public string playerName;
    private List<TrickModel> tricksTaken;

    public int currentBid = 0;
    public int tricksTakenCount = 0;
    public int score = 0;

    public TextMesh playerInfo;

    private void Awake()
    {
        GameObject textAnchor = new GameObject();
        playerInfo = textAnchor.AddComponent<TextMesh>();
        textAnchor.transform.SetParent(gameObject.transform);
        textAnchor.transform.localPosition = new Vector3(0, -0.5f, -2);
        textAnchor.transform.localRotation = textAnchor.transform.parent.localRotation;
        textAnchor.transform.localRotation = Quaternion.identity;
        textAnchor.transform.Rotate(new Vector3(0, 0, 180));

        playerInfo.text = "Bid: ? Taken: -";
        playerInfo.anchor = TextAnchor.LowerCenter;
        playerInfo.alignment = TextAlignment.Center;
        playerInfo.characterSize = 0.1f;
        playerInfo.fontSize = 40;
    }

    // Use this for initialization
    void Start()
    {
        tricksTaken = new List<TrickModel>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeTrick(TrickModel trick)
    {
        tricksTaken.Add(trick);
        tricksTakenCount++;
        UpdatePlayerInfoText();
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
        SetBid(bid);
        return bid;
    }

    internal void SetBid(int bid)
    {
        currentBid = bid;
        UpdatePlayerInfoText();
    }

    private void UpdatePlayerInfoText(bool includeScore=false)
    {
        if (includeScore)
        {
            playerInfo.text = String.Format("Bid: {0}, Taken: {1}\nScore:{2}", currentBid, tricksTakenCount, score);
            playerInfo.transform.localPosition = new Vector3(0, 0, -2);
        }
        else
        {
            playerInfo.text = String.Format("Bid: {0}, Taken: {1}", currentBid, tricksTakenCount);
        }
    }

    internal void UpdateScore()
    {
        score = currentBid == tricksTakenCount ? 10 + tricksTakenCount * tricksTakenCount : -5 * Math.Abs(currentBid - tricksTakenCount);
        UpdatePlayerInfoText(true);
    }
}
