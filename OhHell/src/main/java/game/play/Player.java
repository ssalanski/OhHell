package game.play;

import game.score.Status;

public abstract class Player {

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

	// should decide how much the player wants to bid for this hand,
	// given the current bid situation. Must adhere to OH rules
	// regarding bid restrictions when you are the dealer (restricted=true)
	public abstract int bid(int tricksThisHand, int tricksRemaining, boolean restricted);

	// should choose the card to play from the players hand
	// must ensure the play is legal considering the current board
	// must remove the card from the players hand as it returns it
	public abstract Card playCard(Board board);
	
	public String toString()
	{
		return "Player: " + name;
	}
	
}
