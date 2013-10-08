package game.play;

import game.score.Status;

public class Player {

	private String name;
	private Status status;
	
	public Player(String name)
	{
		this.name = name;
		this.status = new Status();
	}

	public String getName() {
		return name;
	}

	public void setName(String name) {
		this.name = name;
	}

	public Status getStatus() {
		return status;
	}

	public void setStatus(Status status) {
		this.status = status;
	}

	public void setHand(Hand hand) {
		status.setHand(hand);
	}
	
	public Hand getHand() {
		return status.getHand();
	}

	public int bid(int tricksRemaining, boolean restricted) {
		return 0; // FIXME: actually bid!
		//TODO: maybe extend this class to AI player and human player, one calls a thinking method, the other looks for stdin
	}

	public Card playCard(Suit lead) {
		return status.getHand().legalPlays(lead).get(0); // FIXME: actually play!
		//TODO: extend this class to AI player and human player, one calls a thinking method, the other looks for std in
	}
	
	public String toString()
	{
		return "Player: " + name;
	}
	
}
