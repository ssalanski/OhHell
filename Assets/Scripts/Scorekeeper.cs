using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scorekeeper : MonoBehaviour
{
    public GameObject playerNameTextPrefab;
    public GameObject scoreRowPanelPrefab;
    public GameObject scoreEntryTextPrefab;
    public GameObject totalScoreTextPrefab;

    private Dictionary<PlayerModel, Text> players;

    private GameObject header;
    private GameObject footer;
    private GameObject scoreTable;

    private void Awake()
    {
        int numPlayers = GameObject.Find("OhHellGame").GetComponent<OptionsManager>().playerCount;
        scoreTable = GameObject.Find("ScoreTable").gameObject;
        Hide();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Initialize(List<PlayerModel> players)
    {
        this.players = new Dictionary<PlayerModel, Text>(players.Count);

        header = GameObject.Find("PlayerNamesHeader").gameObject;
        footer = GameObject.Find("FinalScoreFooter").gameObject;
        foreach (PlayerModel player in players)
        {
            GameObject playerName = Instantiate(playerNameTextPrefab, header.transform);
            playerName.GetComponent<Text>().text = player.playerName;
            GameObject playerTotalScore = Instantiate(totalScoreTextPrefab, footer.transform);
            Text scoreText = playerTotalScore.GetComponent<Text>();
            scoreText.text = "0";
            this.players.Add(player, scoreText);
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void RecordRoundScores()
    {
        GameObject scoreRow = Instantiate(scoreRowPanelPrefab, scoreTable.transform);
        footer.transform.SetAsLastSibling();
        foreach (PlayerModel player in this.players.Keys)
        {
            int score = player.currentBid == player.tricksTakenCount ? 10 + player.tricksTakenCount * player.tricksTakenCount : -5 * Math.Abs(player.currentBid - player.tricksTakenCount);
            GameObject scoreEntry = Instantiate(scoreEntryTextPrefab, scoreRow.transform);
            scoreEntry.GetComponent<Text>().text = String.Format("{0}/{1}  {2}", player.tricksTakenCount, player.currentBid, score);
            Text scoreText = players[player];
            scoreText.lineSpacing = scoreText.lineSpacing + score; // definitely cheating by using this unrelated/unused lineSpacing field to store the score
            scoreText.text = "" + scoreText.lineSpacing;           // just seems like a waste to attach a whole other component just to store a single integer
        }
    }
}
