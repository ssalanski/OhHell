package game.score;

public class Status {

	private int bid;
	private int taken;
	
	private int totalScore;
	private int handScore;
	
	public Status()
	{
		bid = 0;
		taken = 0;
		totalScore = 0;
		handScore = 0;
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
	
	
	
}
