package game;

import game.play.Board;
import game.play.Card;
import game.play.Player;

public class WildcardPlayer extends Player {

	public WildcardPlayer(String name) {
		super(name);
		// TODO Auto-generated constructor stub
	}

	@Override
	public int bid(int tricksThisHand, int tricksRemaining, boolean restricted) {
		int bidRequest = (int) Math.floor(Math.random()*(tricksThisHand+1));
		if(restricted && bidRequest==tricksRemaining)
		{
			// adjust bid if we're restricted. add one, unless we were trying to bid the max, then go down one.
			return bidRequest + (bidRequest==tricksThisHand ? -1 : 1); 
		}
		else
		{
			return bidRequest;
		}
	}

	@Override
	public Card playCard(Board board) {
		// TODO Auto-generated method stub
		return null;
	}

}
