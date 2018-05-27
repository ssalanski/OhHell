using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // prefabs linked in from unity editor
    public GameObject handAnchorPrefab;
    public GameObject trickAnchorPrefab;
    public GameObject cardPrefab;

    // game setting value
    public int numOtherPlayers;

    // containers that change throughout the game
    private HandModel playerHand;
    private List<HandModel> allHands;
    private TrickModel currentTrick;
    private GameObject trumpCard;
    private Deck deck;

    // actual constants
    private const float tableMinor = 3.5f;
    private const float tableMajor = 6.0f;

    private void DealNewHand(int numberOfCards)
    {
        deck = new Deck();
        GameObject handAnchor;

        // instantiate the players hand
        handAnchor = Instantiate(handAnchorPrefab, gameObject.transform);
        handAnchor.transform.localPosition += Vector3.down * tableMinor;
        playerHand = handAnchor.GetComponent<HandModel>();

        // instantiate the other players hands, placement/spacing depends on count
        allHands = new List<HandModel>(numOtherPlayers);
        allHands.Add(playerHand);
        if (numOtherPlayers == 1)
        {
            handAnchor = Instantiate(handAnchorPrefab, gameObject.transform);
            handAnchor.transform.localPosition += Vector3.up * tableMinor;
            handAnchor.transform.Rotate(new Vector3(0, 0, 180));
            allHands.Add(handAnchor.GetComponent<HandModel>());
        }
        else if (numOtherPlayers == 2)
        {
            handAnchor = Instantiate(handAnchorPrefab, gameObject.transform);
            handAnchor.transform.localPosition += getEllipsePositionAtAngle(30);
            handAnchor.transform.Rotate(new Vector3(0, 0, -120));
            allHands.Add(handAnchor.GetComponent<HandModel>());

            handAnchor = Instantiate(handAnchorPrefab, gameObject.transform);
            handAnchor.transform.localPosition += getEllipsePositionAtAngle(150);
            handAnchor.transform.Rotate(new Vector3(0, 0, 120));
            allHands.Add(handAnchor.GetComponent<HandModel>());
        }
        else
        {
            float playerSpacing = 180f / (numOtherPlayers - 1);
            for (int playerIdx = 0; playerIdx < numOtherPlayers; playerIdx++)
            {
                float playerAngle = playerSpacing * playerIdx;
                handAnchor = Instantiate(handAnchorPrefab, gameObject.transform);
                handAnchor.transform.localPosition += getEllipsePositionAtAngle(playerAngle);
                handAnchor.transform.Rotate(new Vector3(0, 0, 270 - playerAngle));
                allHands.Add(handAnchor.GetComponent<HandModel>());
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

        GameObject trickAnchor = Instantiate(trickAnchorPrefab, gameObject.transform);
        currentTrick = trickAnchor.GetComponent<TrickModel>();

    }

    private IEnumerator TurnTaker()
    {
        foreach (HandModel hm in allHands)
        {
            // have this hand take its turn
            hm.TakeTurn(currentTrick, trumpCard.GetComponent<CardModel>().thisCard.suit);
            // wait until its done with its turn
            yield return new WaitUntil(() => !hm.yourTurn);
        }
        HandModel winner = currentTrick.GetWinner(trumpCard.GetComponent<CardModel>().thisCard.suit);
        Debug.Log(allHands.IndexOf(winner) + " won that trick");

    }

    void Start()
    {
        DealNewHand(7);
        StartCoroutine(TurnTaker());
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
