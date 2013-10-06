package game.play;

import java.util.ArrayList;
import java.util.List;

import utils.misc.StringUtil;

public class Hand extends ArrayList<Card>{

	public String toString()
	{
		return "Hand: " + StringUtil.join(", ", this);
	}
	
	public List<Card> legalPlays(Suit lead)
	{
		if (lead == null) // then you are in the lead, all cards legal.
		{
			return this;
		}
		ArrayList<Card> legal = new ArrayList<Card>();
		for (Card card : this)
		{
			if (card.getSuit().equals(lead))
			{
				legal.add(card);
			}
		}
		if (legal.size()>0) // then you the suit that was led, must follow suit
		{
			return legal;
		}
		else // then you are void in the suit led, so play whatever.
		{
			return this;
		}
	}
	
}
