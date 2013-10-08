package game.play;

import java.util.HashMap;
import java.util.Iterator;

import utils.misc.StringUtil;

public class Board extends HashMap<Card,Player>{

	private Card lead;
	
	public String toString()
	{
		return "Board: " + StringUtil.join(", ", this.values());
	}
	
	public Player put(Card card, Player player)
	{
		if (this.size()==0) // then this is the first card played, aka the lead. keep track of that
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
	 * out of the cards played on this board, determine the winning player by finding the winning card
	 * and looking up who played it.
	 */
	public Player determineWinner(Suit trump) {
		
		Iterator<Card> cards = this.keySet().iterator();
		Card winner = cards.next();
		Card contender = null;
		while(cards.hasNext())
		{
			contender = cards.next();
			if(!winner.beats(contender,trump,lead.getSuit()))
			{
				winner = contender;
			}
		}
		
		return this.get(winner);
		
	}

}
