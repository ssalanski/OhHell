package game.play;

import java.util.ArrayList;
import java.util.List;

import utils.misc.ListUtil;
import utils.misc.StringUtil;

public class Hand {

	private List<Card> hand;
	
	public Hand()
	{
		hand = new ArrayList<Card>();
	}
	
	public Hand(Card... cards)
	{
		hand = ListUtil.asList(cards);
	}
	
	public String toString()
	{
		return StringUtil.join(", ", hand);
	}
	
}
