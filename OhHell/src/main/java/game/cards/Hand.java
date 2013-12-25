package game.cards;

import java.util.ArrayList;
import java.util.Collections;
import java.util.List;

import utils.misc.StringUtil;
import utils.strategy.CardsUtil;

public class Hand extends ArrayList<Card>{

	public Hand(List<Card> cards) {
		super(cards);
	}

	public void organize(Suit trump)
	{
		Collections.sort(this, CardsUtil.getCardComparator(trump));
	}
	
	public String toString()
	{
		return "Hand: " + StringUtil.join(", ", this);
	}
	
}
