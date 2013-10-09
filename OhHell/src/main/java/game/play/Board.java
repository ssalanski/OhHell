package game.play;

import java.util.HashMap;

import utils.misc.StringUtil;
import utils.strategy.CardsUtil;

public class Board extends HashMap<Card, Player> {

	private Card lead;
	private Suit trump;

	public String toString() {
		return "Board: " + StringUtil.join(", ", this.values());
	}

	public Player put(Card card, Player player) {
		if (this.size() == 0) // then this is the first card played, aka the lead. keep track of that
		{
			setLead(card);
		}
		return super.put(card, player);
	}

	public Card getLead() {
		return lead;
	}

	public void setLead(Card lead) {
		this.lead = lead;
	}

	/*
	 * out of the cards played on this board, determine the winning player by
	 * finding the winning card and looking up who played it.
	 */
	public Player determineWinner() {
		Card winner = CardsUtil.whoWins(this.keySet(), trump, lead.getSuit());
		return this.get(winner);
	}

	public void setTrump(Suit trump) {
		this.trump = trump;
	}

	public Suit getTrump() {
		return trump;
	}

}
