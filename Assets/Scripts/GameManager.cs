using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject handAnchorPrefab;
    public int numOtherPlayers;

    HandModel playerHand;
    List<HandModel> otherHands;

    private const float tableMinor = 3.5f;
    private const float tableMajor = 6.0f;

    private Deck deck;

    void Start()
    {
        deck = new Deck();
        GameObject handAnchor;

        // instantiate the players hand
        handAnchor = Instantiate(handAnchorPrefab, gameObject.transform);
        handAnchor.transform.localPosition += Vector3.down * tableMinor;
        playerHand = handAnchor.GetComponent<HandModel>();

        // instantiate the other players hands, placement/spacing depends on count
        otherHands = new List<HandModel>(numOtherPlayers);
        if (numOtherPlayers == 1)
        {
            handAnchor = Instantiate(handAnchorPrefab, gameObject.transform);
            handAnchor.transform.localPosition += Vector3.up * tableMinor;
            handAnchor.transform.Rotate(new Vector3(0, 0, 180));
            otherHands.Add(handAnchor.GetComponent<HandModel>());
            print(otherHands.Count);
        }
        else if (numOtherPlayers == 2)
        {
            handAnchor = Instantiate(handAnchorPrefab, gameObject.transform);
            handAnchor.transform.localPosition += getEllipsePositionAtAngle(30);
            handAnchor.transform.Rotate(new Vector3(0, 0, 120));
            otherHands.Add(handAnchor.GetComponent<HandModel>());

            handAnchor = Instantiate(handAnchorPrefab, gameObject.transform);
            handAnchor.transform.localPosition += getEllipsePositionAtAngle(150);
            handAnchor.transform.Rotate(new Vector3(0, 0, -120));
            otherHands.Add(handAnchor.GetComponent<HandModel>());
        }
        else
        {
            float playerSpacing = 180f / (numOtherPlayers - 1);
            for (int playerIdx = 0; playerIdx < numOtherPlayers; playerIdx++)
            {
                float playerAngle = playerSpacing * playerIdx;
                handAnchor = Instantiate(handAnchorPrefab, gameObject.transform);
                handAnchor.transform.localPosition += getEllipsePositionAtAngle(playerAngle);
                handAnchor.transform.Rotate(new Vector3(0, 0, 90+playerAngle));
                otherHands.Add(handAnchor.GetComponent<HandModel>());
            }
        }
    }

    private Vector3 getEllipsePositionAtAngle(float angle)
    {
        float theta = angle * Mathf.Deg2Rad;
        float x = tableMajor * tableMinor / Mathf.Sqrt(tableMinor * tableMinor + Mathf.Pow(tableMajor * Mathf.Tan(theta), 2)) * Mathf.Sign(Mathf.Cos(theta));
        float y = tableMinor * Mathf.Sqrt(1 - Mathf.Pow(x / tableMajor, 2));
        return new Vector3(x, y, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerHand.TakeCard(deck.DrawCard());
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            otherHands[0].TakeCard(deck.DrawCard());
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            otherHands[1].TakeCard(deck.DrawCard());
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            otherHands[2].TakeCard(deck.DrawCard());
        }
    }

}
