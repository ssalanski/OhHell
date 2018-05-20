using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    HandModel playerHand;
    public GameObject handAnchorPrefab;

    void Start()
    {
        GameObject handAnchor;
        handAnchor = Instantiate(handAnchorPrefab, gameObject.transform);
        playerHand = handAnchor.GetComponent<HandModel>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerHand.TakeCard(new Card(Suit.Diamonds, 1));
        }
    }

}
