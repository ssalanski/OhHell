package game.cards;

import game.player.Player;

import java.util.HashMap;
import java.util.Set;

import utils.misc.StringUtil;
import utils.strategy.CardsUtil;

public class Board extends HashMap<Card, Player> {

	private Suit lead;
	private Suit trump;

	public String toString() {
		return "Board: " + StringUtil.join(", ", this.values());
	}

	public Player put(Card card, Player player) {
		if (this.size() == 0) // then this is the first card played, aka the lead. keep track of that
		{
			setLead(card.getSuit());
		}
		return super.put(card, player);
	}
	
	public void clear()
	{
		super.clear();
		this.lead = null;
	}

	public Suit getLead() {
		return lead;
	}

	public void setLead(Suit lead) {
		this.lead = lead;
	}
	
	public Set<Card> getCards()
	{
		return this.keySet();
	}

	/*
	 * out of the cards played on this board, determine the winning player by
	 * finding the winning card and looking up who played it.
	 */
	public Player determineWinner() {
		Card winner = CardsUtil.whoWins(this.keySet(), trump, lead);
		return this.get(winner);
	}

	public void setTrump(Suit trump) {
		this.trump = trump;
	}

	public Suit getTrump() {
		return trump;
	}

}
