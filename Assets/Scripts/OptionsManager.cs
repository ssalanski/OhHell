using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour {

    public Text playerCountLabel;
    public Text cardCountLabel;

    public int cardCount { get; set; }
    public float cardCountF
    {
        set
        {
            cardCount = (int)value;
            cardCountLabel.text = string.Format("Card Count: {0}", cardCount);
        }
    }

    public int playerCount { get; set; }
    public float playerCountF
    {
        set
        {
            playerCount = (int)value;
            playerCountLabel.text = string.Format("Player Count: {0}", playerCount);
        }
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    void Start ()
    {
        playerCountF = 4;
        cardCountF = 5;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

}
