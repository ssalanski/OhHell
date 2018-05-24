using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrickModel : MonoBehaviour
{
    
    Dictionary<HandModel, CardModel> cards;
    public GameObject cardPrefab;

    // Use this for initialization
    void Start()
    {
        cards = new Dictionary<HandModel, CardModel>();
    }

    public void TakeCard(GameObject playedCard)
    {
        playedCard.tag = "in trick";
        CardModel cardModel = playedCard.GetComponent<CardModel>();
        cardModel.showing = true;

        cards.Add(cardModel.GetComponentInParent<HandModel>(), cardModel);
        Transform sourceHand = cardModel.transform.parent;

        cardModel.transform.parent = gameObject.transform;
        cardModel.transform.localPosition = new Vector3(0, 0, 0);
        cardModel.transform.Translate(Vector3.down, sourceHand);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
