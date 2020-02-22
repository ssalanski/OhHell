using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scorekeeper : MonoBehaviour
{

    private void Awake()
    {
        int numPlayers = GameObject.Find("OhHellGame").GetComponent<OptionsManager>().playerCount;
        hide();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       // score = currentBid == tricksTakenCount ? 10 + tricksTakenCount * tricksTakenCount : -5 * Math.Abs(currentBid - tricksTakenCount);
        
    }

    public void show()
    {
        gameObject.SetActive(true);
    }

    public void hide()
    {
        gameObject.SetActive(false);
    }

    public void RecordRoundScores()
    {
        
    }
}
