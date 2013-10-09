package game.score;

import game.play.Board;
import game.play.Player;

import java.util.HashMap;
import java.util.List;
import java.util.Map;

public class HandRecord {

	private Map<Player,Status> playerStats;
	private Board board;
	
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

	public void assessScores() {
		for(Status s : playerStats.values())
		{
			s.calculateScore();
		}
	}

	public Board getBoard() {
		return board;
	}

	public void setBoard(Board board) {
		this.board = board;
	}
	
}
