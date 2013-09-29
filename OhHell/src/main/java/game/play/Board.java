package game.play;

import java.util.ArrayList;
import java.util.List;

import utils.misc.ListUtil;
import utils.misc.StringUtil;

public class Board {

	private List<Card> board;
	
	public Board()
	{
		board = new ArrayList<Card>();
	}
	
	public Board(Card... cards)
	{
		board = ListUtil.asList(cards);
	}
	
	public String toString()
	{
		return StringUtil.join(", ", board);
	}
	
}
