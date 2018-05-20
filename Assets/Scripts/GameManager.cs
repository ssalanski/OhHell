using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject handAnchorPrefab;
    public int numOtherPlayers;

    HandModel playerHand;
    List<HandModel> otherHands;

    void Start()
    {
        GameObject handAnchor;
        handAnchor = Instantiate(handAnchorPrefab, gameObject.transform);
        handAnchor.transform.localPosition += Vector3.down * 3.5f;
        playerHand = handAnchor.GetComponent<HandModel>();

        otherHands = new List<HandModel>(numOtherPlayers);
        //float otherPlayerSpacing = numOtherPlayers == 1 ? 0 : numOtherPlayers == 2 ? 120 : 180f / (numOtherPlayers - 1);
        if (numOtherPlayers == 1)
        {
            handAnchor = Instantiate(handAnchorPrefab, gameObject.transform);
            handAnchor.transform.localPosition += Vector3.up * 3.5f;
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
        float a = 6f;
        float b = 3.5f;
        float theta = angle * Mathf.Deg2Rad;
        float x = a * b / Mathf.Sqrt(b * b + Mathf.Pow(a * Mathf.Tan(theta), 2)) * Mathf.Sign(Mathf.Cos(theta));
        float y = b * Mathf.Sqrt(1 - Mathf.Pow(x / a, 2));
        return new Vector3(x, y, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerHand.TakeCard(new Card(Suit.Diamonds, 1));
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            otherHands[0].TakeCard(new Card(Suit.Diamonds, 1));
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            otherHands[1].TakeCard(new Card(Suit.Diamonds, 1));
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            otherHands[2].TakeCard(new Card(Suit.Diamonds, 1));
        }
    }

}
