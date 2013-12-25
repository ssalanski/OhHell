package utils.strategy;

import game.cards.Board;
import game.cards.Card;
import game.cards.Hand;
import game.cards.Suit;

import java.util.ArrayList;
import java.util.Collection;
import java.util.List;
import java.util.Comparator;

public class CardsUtil {

	public static Card whoWins(Collection<Card> playedCards, Suit lead, Suit trump) {
		Card winner = null;
		for(Card contender : playedCards)
		{
			if(contender.beats(winner,lead,trump))
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

	public static Card lowest(List<Card> legalPlays, Suit trump) {
		if(legalPlays.size()==0)
		{
			throw new IllegalArgumentException("Cant decide lowest of no cards");
		}
		Card low = legalPlays.get(0);
		for(Card card : legalPlays)
		{
			if (score(card, trump) <= score(low, trump))
			{
				low = card;
			}
		}
		return low;
	}
	
	private static int score(Card c, Suit trump)
	{
		if (c==null)
		{
			throw new IllegalArgumentException("Cant score null card");
		}
		return c.getDenom() + (c.getSuit().equals(trump) ? 20 : 0);
	}

	public static List<Card> losingPlays(Board board, Hand hand) {
		Suit trump = board.getTrump();
		Suit lead = board.getLead();
		List<Card> legalPlays = CardsUtil.legalPlays(hand, lead);
		List<Card> losingPlays = new ArrayList<Card>();
		Card currentlyWinning = CardsUtil.whoWins(board.getCards(), lead, trump);
		for ( Card card : legalPlays )
		{
			if ( currentlyWinning.beats(card, lead, trump) )
			{
				losingPlays.add(card);
			}
		}
		return losingPlays;
	}
	
	public static class CardComparator implements Comparator<Card>
	{
		private Suit trump;
		
		public CardComparator(Suit trump)
		{
			this.trump = trump;
		}
		
		@Override
		public int compare(Card c1, Card c2) {
			return c2.getSuit().compareTo(c1.getSuit()) * 100 
					+ (c2.getSuit().equals(trump) ? 1000 : 0) 
					- (c1.getSuit().equals(trump) ? 1000 : 0)
					+ c2.getDenom()
					- c1.getDenom();
		}
	}

	public static CardComparator getCardComparator(Suit trump)
	{
		return new CardComparator(trump);
	}
	
	public static CardComparator getCardComparator()
	{
		return new CardComparator(null);
	}
	
}
