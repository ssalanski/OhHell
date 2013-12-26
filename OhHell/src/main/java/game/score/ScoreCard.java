package game.score;

import java.util.ArrayList;
import java.util.List;

public class ScoreCard {

	private List<HandRecord> scoreTable;
	
	public ScoreCard(int numPlayers)
	{
		scoreTable = new ArrayList<HandRecord>();
	}
	
	public void recordHand(HandRecord hand)
	{
		scoreTable.add(hand);
	}
	
	public HandRecord getHandRecord(int hand)
	{
		return scoreTable.get(hand);
	}
	
}
