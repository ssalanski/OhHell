﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // prefabs linked in from unity editor
    public GameObject playerPrefab;
    public GameObject trickPrefab;
    public GameObject cardPrefab;

    // game setting value
    public int numOtherPlayers;

    // containers that change throughout the game
    private HandModel humanPlayerHand;
    private List<HandModel> allPlayers;
    private TrickModel currentTrick;
    private HandModel leader;
    private GameObject trumpCard;
    private Deck deck;

    // actual constants
    private const float tableMinor = 3.5f;
    private const float tableMajor = 6.0f;

    private void DealNewHand(int numberOfCards)
    {
        deck = new Deck();
        GameObject player;

        // instantiate the (human) players hand
        player = Instantiate(playerPrefab, gameObject.transform);
        player.transform.localPosition += Vector3.down * tableMinor;
        humanPlayerHand = player.GetComponent<HandModel>();

        // instantiate the other players hands, placement/spacing depends on count
        allPlayers = new List<HandModel>(numberOfPlayers);
        allPlayers.Add(humanPlayerHand);
        if (numOtherPlayers == 1)
        {
            player = Instantiate(playerPrefab, gameObject.transform);
            player.transform.localPosition += Vector3.up * tableMinor;
            player.transform.Rotate(new Vector3(0, 0, 180));
            allPlayers.Add(player.GetComponent<HandModel>());
        }
        else if (numOtherPlayers == 2)
        {
            player = Instantiate(playerPrefab, gameObject.transform);
            player.transform.localPosition += getEllipsePositionAtAngle(30);
            player.transform.Rotate(new Vector3(0, 0, -120));
            allPlayers.Add(player.GetComponent<HandModel>());

            player = Instantiate(playerPrefab, gameObject.transform);
            player.transform.localPosition += getEllipsePositionAtAngle(150);
            player.transform.Rotate(new Vector3(0, 0, 120));
            allPlayers.Add(player.GetComponent<HandModel>());
        }
        else
        {
            float playerSpacing = 180f / (numOtherPlayers - 1);
            for (int playerIdx = 0; playerIdx < numOtherPlayers; playerIdx++)
            {
                float playerAngle = playerSpacing * playerIdx;
                player = Instantiate(playerPrefab, gameObject.transform);
                player.transform.localPosition += getEllipsePositionAtAngle(playerAngle);
                player.transform.Rotate(new Vector3(0, 0, 270 - playerAngle));
                allPlayers.Add(player.GetComponent<HandModel>());
            }
        }

        for (int cardNumber = 0; cardNumber < numberOfCards; cardNumber++)
        {
            foreach (HandModel hm in allHands)
            {
                hm.TakeCard(deck.DrawCard());
            }
        }

        trumpCard = Instantiate(cardPrefab, gameObject.transform);
        trumpCard.transform.localPosition = new Vector3(6.7f, -3.86f, -0.1f);
        trumpCard.GetComponent<CardModel>().SetCard(deck.DrawCard());
        trumpCard.GetComponent<CardModel>().showing = true;

    }

    private IEnumerator PlayTrick(int leadOffset)
    {
        for(int turnIndex = 0; turnIndex < numOtherPlayers+1; turnIndex++ )
        {
            int playerIndex = (turnIndex + leadOffset) % (numOtherPlayers + 1);
            allPlayers[playerIndex].SetTurnFlag(true); // (currentTrick, trumpCard.GetComponent<CardModel>().thisCard.suit);
            yield return new WaitUntil(() => !allPlayers[playerIndex].IsYourTurn());
        }
        HandModel winner = currentTrick.GetWinner(trumpCard.GetComponent<CardModel>().thisCard.suit);
        Debug.Log(allPlayers.IndexOf(winner) + " won that trick");
        leader = winner;
        yield return new WaitForSeconds(2);
    }

    private IEnumerator PlayRound(int dealerOffset)
    {
        DealNewHand(7);
        leader = allPlayers[0];
        for (int cardNumber = 0; cardNumber < 7; cardNumber++)
        {
            GameObject trickAnchor = Instantiate(trickPrefab, gameObject.transform);
            currentTrick = trickAnchor.GetComponent<TrickModel>();
            foreach (HandModel hm in allPlayers)
            {
                hm.SetCurrentTrick(currentTrick);
            }
            yield return PlayTrick(allPlayers.IndexOf(leader));
            currentTrick.gameObject.SetActive(false);  // not destroying it because this info may be useful later
        }
        Debug.Log("played all cards in this round");
    }

    void Start()
    {
        StartCoroutine(PlayRound(0));
    }

    private Vector3 getEllipsePositionAtAngle(float angle)
    {
        float theta = angle * Mathf.Deg2Rad;
        float x = tableMajor * tableMinor / Mathf.Sqrt(tableMinor * tableMinor + Mathf.Pow(tableMajor * Mathf.Tan(theta), 2)) * Mathf.Sign(-Mathf.Cos(theta));
        float y = tableMinor * Mathf.Sqrt(1 - Mathf.Pow(x / tableMajor, 2));
        return new Vector3(x, y, 0);
    }

    // Update is called once per frame
    void Update()
    {
    }

}
