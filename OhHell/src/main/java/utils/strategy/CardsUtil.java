package utils.strategy;

import game.play.Card;
import game.play.Hand;
import game.play.Suit;

import java.util.ArrayList;
import java.util.Collection;
import java.util.Iterator;
import java.util.List;

public class CardsUtil {

	public static Card whoWins(Collection<Card> keySet, Suit lead, Suit trump) {
		Iterator<Card> cards = keySet.iterator();
		Card winner = cards.next();
		Card contender = null;
		while(cards.hasNext())
		{
			contender = cards.next();
			if(!winner.beats(contender,lead,trump))
			{
				winner = contender;
			}
		}
		return winner;
	}
	
	public static List<Card> legalPlays(Hand hand, Suit lead)
	{
		if (lead == null) // then you are in the lead, all cards legal.
		{
			return hand;
		}
		List<Card> legal = new ArrayList<Card>();
		for (Card card : hand)
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
			return hand;
		}
	}
	
}
