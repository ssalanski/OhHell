package game.play;

import java.util.ArrayList;
import java.util.List;

import utils.misc.StringUtil;

public class Hand extends ArrayList<Card>{

	public Hand(List<Card> cards) {
		super(cards);
	}

	public String toString()
	{
		return "Hand: " + StringUtil.join(", ", this);
	}
	
}
