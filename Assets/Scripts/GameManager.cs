using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // prefabs linked in from unity editor
    public GameObject playerPrefab;
    public GameObject aiPlayerPrefab;
    public GameObject trickPrefab;
    public GameObject cardPrefab;

    private Scorekeeper scorekeeper;

    // game setting value
    public int numPlayers;
    public int cardCount;

    // containers that change throughout the game
    private List<PlayerModel> allPlayers;
    private TrickModel currentTrick;
    private PlayerModel leader;
    private GameObject trumpCard;
    private Deck deck;

    private bool gameOver = false;

    // actual constants
    private const float tableMinor = 3.5f;
    private const float tableMajor = 6.0f;

    private void Awake()
    {
        numPlayers = GameObject.Find("OhHellGame").GetComponent<OptionsManager>().playerCount;
        cardCount = GameObject.Find("OhHellGame").GetComponent<OptionsManager>().cardCount;
        scorekeeper = GameObject.Find("ScorecardPanel").GetComponent<Scorekeeper>();
        SetTable();
    }

    void Start()
    {
        StartCoroutine(PlayGame());
    }

    private void SetTable()
    {
        allPlayers = new List<PlayerModel>(numPlayers);

        GameObject playerObject;
        PlayerModel playerModel;

        // instantiate the (human) players hand
        playerObject = Instantiate(playerPrefab, gameObject.transform);
        playerObject.transform.localPosition += Vector3.down * tableMinor;
        playerModel = playerObject.GetComponent<PlayerModel>();
        playerModel.playerInfo.anchor = TextAnchor.UpperCenter;
        playerModel.playerInfo.transform.Rotate(new Vector3(0, 0, 180));
        playerModel.GetHand().setFaceUp(true);
        playerModel.playerName = "You";
        allPlayers.Add(playerModel);

        // instantiate the other players hands, placement/spacing depends on count
        if (numPlayers == 2)
        {
            playerObject = Instantiate(aiPlayerPrefab, gameObject.transform);
            playerObject.transform.localPosition += Vector3.up * tableMinor;
            playerObject.transform.Rotate(new Vector3(0, 0, 180));
            playerModel = playerObject.GetComponent<PlayerModel>();
            playerModel.playerName = "Them";
            allPlayers.Add(playerModel);
        }
        else if (numPlayers == 3)
        {
            playerObject = Instantiate(aiPlayerPrefab, gameObject.transform);
            playerObject.transform.localPosition += getEllipsePositionAtAngle(30);
            playerObject.transform.Rotate(new Vector3(0, 0, -120));
            playerModel = playerObject.GetComponent<PlayerModel>();
            playerModel.playerName = "Dumb";
            allPlayers.Add(playerModel);

            playerObject = Instantiate(aiPlayerPrefab, gameObject.transform);
            playerObject.transform.localPosition += getEllipsePositionAtAngle(150);
            playerObject.transform.Rotate(new Vector3(0, 0, 120));
            playerModel = playerObject.GetComponent<PlayerModel>();
            playerModel.playerName = "Dumber";
            allPlayers.Add(playerModel);
        }
        else
        {
            float playerSpacing = 180f / (numPlayers - 2);
            for (int playerIdx = 0; playerIdx < numPlayers - 1; playerIdx++)
            {
                float playerAngle = playerSpacing * playerIdx;
                playerObject = Instantiate(aiPlayerPrefab, gameObject.transform);
                playerObject.transform.localPosition += getEllipsePositionAtAngle(playerAngle);
                playerObject.transform.Rotate(new Vector3(0, 0, 270 - playerAngle));
                playerModel = playerObject.GetComponent<PlayerModel>();
                playerModel.playerName = "AI " + (playerIdx + 1);
                allPlayers.Add(playerModel);
            }
        }
        scorekeeper.Initialize(allPlayers);
    }

    private IEnumerator PlayGame()
    {
        int[] cardCounts = Enumerable.Range(2 - cardCount, 2 * cardCount - 1).ToArray();
        for (int handNumber = 0; handNumber < cardCounts.Length; handNumber++)
        {
            int handCardCount = handNumber < cardCount ? 2 - cardCounts[handNumber] : cardCounts[handNumber];
            yield return PlayRound(handCardCount, handNumber % numPlayers);
        }
        gameOver = true;
        scorekeeper.Show(true);
    }

    private void DealNewHand(int numberOfCards)
    {
        deck = new Deck();

        for (int cardNumber = 0; cardNumber < numberOfCards; cardNumber++)
        {
            foreach (PlayerModel player in allPlayers)
            {
                player.GetHand().TakeCard(deck.DrawCard());
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
            currentTrick.SetTrump(trumpCard.GetComponent<CardModel>().thisCard.suit);
            foreach (PlayerModel player in allPlayers)
            {
                player.SetCurrentTrick(currentTrick);
                player.GetHand().SetCurrentTrick(currentTrick);
            }
            yield return PlayTrick(allPlayers.IndexOf(leader));
        }

        scorekeeper.RecordRoundScores();
        scorekeeper.Show(true);
        yield return new WaitUntil(() => !scorekeeper.gameObject.activeInHierarchy);
        foreach (PlayerModel player in allPlayers)
        {
            player.Reset();
        }
        GameObject.Destroy(trumpCard);
    }

    private void BidRound(int cardCount, int dealerOffset)
    {
        int bidTotal = 0;
        // start bidIndex at 1, player to the left of the dealer starts bidding
        for (int bidIndex = 1; bidIndex < numPlayers; bidIndex++)
        {
            int playerIndex = (bidIndex + dealerOffset) % numPlayers;
            bidTotal += allPlayers[playerIndex].MakeBid(bidTotal, cardCount);
        }
        // now have the dealer bid, passing restricted=true
        leader.GetComponentInParent<PlayerModel>().MakeBid(bidTotal, cardCount, true);
    }

    private IEnumerator PlayTrick(int leadOffset)
    {
        for (int turnIndex = 0; turnIndex < numPlayers; turnIndex++)
        {
            int playerIndex = (turnIndex + leadOffset) % numPlayers;
            PlayerModel currentPlayer = allPlayers[playerIndex];
            yield return currentPlayer.TakeTurn();
        }
        PlayerModel winner = currentTrick.GetWinner(trumpCard.GetComponent<CardModel>().thisCard.suit);
        currentTrick.HilightWinner();
        yield return new WaitForSeconds(2);
        winner.TakeTrick(currentTrick);
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
        if (!gameOver)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                scorekeeper.Show(false);
            }
            else if (Input.GetKeyUp(KeyCode.Tab))
            {
                scorekeeper.Hide();
            }
        }
    }

}
