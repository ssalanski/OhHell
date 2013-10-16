package game.score;

import game.cards.Hand;

public class Status {

	private int bid;
	private int taken;
	
	private int totalScore; // TODO: i dont think i'll actually want to use this field, just use the scorecard to calculate the total score.
	private int handScore;
	
	private Hand hand;
	
	public Status()
	{
		bid = 0;
		taken = 0;
		totalScore = 0;
		handScore = 0;
		hand = null;
	}
	
	public Status(Status previousHand)
	{
		bid = 0;
		taken = 0;
		totalScore = previousHand.totalScore;
		handScore = 0;
		hand = null;
	}

	public int getBid() {
		return bid;
	}

	public void setBid(int bid) {
		this.bid = bid;
	}

	public int getTaken() {
		return taken;
	}

	public void setTaken(int taken) {
		this.taken = taken;
	}

	public int getTotalScore() {
		return totalScore;
	}

	public void setTotalScore(int totalScore) {
		this.totalScore = totalScore;
	}

	public int getHandScore() {
		return handScore;
	}

	public void setHandScore(int handScore) {
		this.handScore = handScore;
	}

	public Hand getHand() {
		return hand;
	}

	public void setHand(Hand hand) {
		this.hand = hand;
	}

	public void calculateScore() {
		handScore = score(bid,taken);
		totalScore += handScore;
	}

	private int score(int bid2, int taken2) {
		if (bid != taken)
		{
			return -5 * Math.abs(bid-taken);
		}
		else
		{
			return 10 + bid*bid;
		}
	}

	public void incrementTricksTaken() {
		this.taken++;
	}
	
	
	
}
