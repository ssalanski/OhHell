package game.score;

import java.util.ArrayList;
import java.util.List;

public class HandRecord {

	private List<Status> playerStats;
	
	public HandRecord(int numPlayers)
	{
		playerStats = new ArrayList<Status>(numPlayers);
	}
	
	public Status getStatusOf(int player)
	{
		return playerStats.get(player);
	}
	
}
