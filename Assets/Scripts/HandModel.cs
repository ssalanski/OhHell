using System.Collections.Generic;
using UnityEngine;

public class HandModel : MonoBehaviour
{
    public List<CardModel> cards;
    public GameObject cardPrefab;

    // Use this for initialization
    void Start()
    {
        cards = new List<CardModel>();
    }
}
