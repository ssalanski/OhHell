using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour {

    public Text playerCountLabel;
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
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

}
