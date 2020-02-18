using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // prefabs linked in from unity editor
    public GameObject playerPrefab;
    public GameObject trickPrefab;
    public GameObject cardPrefab;

    // game setting value
    public int numPlayers;
    public int cardCount;

    // containers that change throughout the game
    private HandModel humanPlayerHand;
    private List<GameObject> allPlayers;
    private TrickModel currentTrick;
    private GameObject leader;
    private GameObject trumpCard;
    private Deck deck;

    // actual constants
    private const float tableMinor = 3.5f;
    private const float tableMajor = 6.0f;

    private void Awake()
    {
        numPlayers = GameObject.Find("OhHellGame").GetComponent<OptionsManager>().playerCount;
        cardCount = GameObject.Find("OhHellGame").GetComponent<OptionsManager>().cardCount;
    }

    void Start()
    {
        SetTable(numPlayers);
        StartCoroutine(PlayGame());
    }

    private void SetTable(int numberOfPlayers)
    {
        GameObject player;

        // instantiate the (human) players hand
        player = Instantiate(playerPrefab, gameObject.transform);
        player.transform.localPosition += Vector3.down * tableMinor;
        humanPlayerHand = player.GetComponent<HandModel>();
        player.GetComponent<PlayerModel>().playerInfo.anchor = TextAnchor.UpperCenter;
        player.GetComponent<PlayerModel>().playerInfo.transform.Rotate(new Vector3(0, 0, 180));

        // instantiate the other players hands, placement/spacing depends on count
        allPlayers = new List<GameObject>(numberOfPlayers);
        allPlayers.Add(player);
        if (numPlayers == 2)
        {
            player = Instantiate(playerPrefab, gameObject.transform);
            player.transform.localPosition += Vector3.up * tableMinor;
            player.transform.Rotate(new Vector3(0, 0, 180));
            allPlayers.Add(player);
        }
        else if (numPlayers == 3)
        {
            player = Instantiate(playerPrefab, gameObject.transform);
            player.transform.localPosition += getEllipsePositionAtAngle(30);
            player.transform.Rotate(new Vector3(0, 0, -120));
            allPlayers.Add(player);

            player = Instantiate(playerPrefab, gameObject.transform);
            player.transform.localPosition += getEllipsePositionAtAngle(150);
            player.transform.Rotate(new Vector3(0, 0, 120));
            allPlayers.Add(player);
        }
        else
        {
            float playerSpacing = 180f / (numPlayers - 2);
            for (int playerIdx = 0; playerIdx < numPlayers - 1; playerIdx++)
            {
                float playerAngle = playerSpacing * playerIdx;
                player = Instantiate(playerPrefab, gameObject.transform);
                player.transform.localPosition += getEllipsePositionAtAngle(playerAngle);
                player.transform.Rotate(new Vector3(0, 0, 270 - playerAngle));
                allPlayers.Add(player);
            }
        }

    }

    private IEnumerator PlayGame()
    {
        int[] cardCounts = Enumerable.Range(2 - cardCount, 2 * cardCount - 1).ToArray();
        for (int handNumber = 0; handNumber < cardCounts.Length; handNumber++)
        {
            int handCardCount = handNumber < cardCount ? 2 - cardCounts[handNumber] : cardCounts[handNumber];
            yield return PlayRound(handCardCount, handNumber % numPlayers);
        }
    }

    private void DealNewHand(int numberOfCards)
    {
        deck = new Deck();

        for (int cardNumber = 0; cardNumber < numberOfCards; cardNumber++)
        {
            foreach (GameObject player in allPlayers)
            {
                player.GetComponent<HandModel>().TakeCard(deck.DrawCard());
            }
        }

        trumpCard = Instantiate(cardPrefab, gameObject.transform);
        trumpCard.transform.localPosition = new Vector3(6.7f, -3.86f, -0.1f);
        trumpCard.GetComponent<CardModel>().SetCard(deck.DrawCard());
        trumpCard.GetComponent<CardModel>().showing = true;
    }

    private IEnumerator PlayRound(int cardCount, int dealerOffset)
    {
        DealNewHand(cardCount);
        leader = allPlayers[dealerOffset];
        BidRound(cardCount, dealerOffset);
        for (int cardNumber = 0; cardNumber < cardCount; cardNumber++)
        {
            GameObject trickAnchor = Instantiate(trickPrefab, gameObject.transform);
            currentTrick = trickAnchor.GetComponent<TrickModel>();
            foreach (GameObject player in allPlayers)
            {
                player.GetComponent<HandModel>().SetCurrentTrick(currentTrick);
            }
            yield return PlayTrick(allPlayers.IndexOf(leader));
            currentTrick.gameObject.SetActive(false);  // not destroying it because this info may be useful later
        }
        Debug.Log("played all cards in this round");
        foreach (GameObject player in allPlayers)
        {
            player.GetComponent<PlayerModel>().UpdateScore();
        }
    }

    private void BidRound(int cardCount, int dealerOffset)
    {
        int bidTotal = 0;
        // start bidIndex at 1, player to the left of the dealer starts bidding
        for (int bidIndex = 1; bidIndex < numPlayers; bidIndex++)
        {
            int playerIndex = (bidIndex + dealerOffset) % numPlayers;
            bidTotal += allPlayers[playerIndex].GetComponentInParent<PlayerModel>().MakeBid(bidTotal, cardCount);
        }
        // now have the dealer bid, passing restricted=true
        leader.GetComponentInParent<PlayerModel>().MakeBid(bidTotal, cardCount, true);
    }

    private IEnumerator PlayTrick(int leadOffset)
    {
        for (int turnIndex = 0; turnIndex < numPlayers; turnIndex++)
        {
            int playerIndex = (turnIndex + leadOffset) % numPlayers;
            PlayerModel currentPlayer = allPlayers[playerIndex].GetComponent<PlayerModel>();
            currentPlayer.SetTurnFlag(true); // (currentTrick, trumpCard.GetComponent<CardModel>().thisCard.suit);
            yield return new WaitUntil(() => !currentPlayer.IsYourTurn());
        }
        GameObject winner = currentTrick.GetWinner(trumpCard.GetComponent<CardModel>().thisCard.suit);
        Debug.Log(allPlayers.IndexOf(winner) + " won that trick");
        winner.GetComponentInParent<PlayerModel>().TakeTrick(currentTrick);
        leader = winner;
        yield return new WaitForSeconds(2);
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
