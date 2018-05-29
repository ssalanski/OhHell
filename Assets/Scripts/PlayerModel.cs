using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : MonoBehaviour {

    public string playerName;
    private List<TrickModel> tricksTaken;

    public int tricksTakenCount = 0;

	// Use this for initialization
	void Start () {
        tricksTaken = new List<TrickModel>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void TakeTrick(TrickModel trick)
    {
        tricksTaken.Add(trick);
        tricksTakenCount++;
    }
}
