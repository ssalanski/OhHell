package game.score;

import game.play.Player;
import game.play.Suit;

import java.util.HashMap;
import java.util.List;
import java.util.Map;

public class HandRecord {

	private Map<Player,Status> playerStats;
	private Suit trump;
	
	public HandRecord(List<Player> players)
	{
		playerStats = new HashMap<Player,Status>(players.size());
		for (Player player : players)
		{
			playerStats.put(player,new Status());
		}
	}
	
	public Status getStatusOf(int player)
	{
		return playerStats.get(player);
	}

	public Suit getTrump() {
		return trump;
	}

	public void setTrump(Suit trump) {
		this.trump = trump;
	}

	public void assessScores() {
		for(Status s : playerStats.values())
		{
			s.calculateScore();
		}
	}
	
}
