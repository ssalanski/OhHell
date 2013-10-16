package game.player;

import java.util.ArrayList;
import java.util.List;

import utils.strategy.CardsUtil;
import game.cards.Board;
import game.cards.Card;
import game.cards.Suit;

public class PassivePlayer extends Player {

	public PassivePlayer(String name) {
		super(name);
		// TODO Auto-generated constructor stub
	}

	@Override
	public int bid(int tricksThisHand, int tricksRemaining, boolean restricted) {
		// TODO Auto-generated method stub
		return 0;
	}

	@Override
	public Card playCard(Board board) {
		Suit trump = board.getTrump();
		Suit lead = board.getLead();
		Card choice;
		if(lead==null) // then you are in the lead
		{
			choice = CardsUtil.lowest(this.getHand(), trump); // lead low.
		}
		else
		{
			List<Card> losingPlays = CardsUtil.losingPlays(board, this.getHand());
			if (losingPlays.size()==0) // you're going to take the trick no matter what
			{
				choice = CardsUtil.whoWins(CardsUtil.legalPlays(this.getHand(), lead), lead, trump); // might as well take it high
				// TODO: wait.. what if there are still more people to play? maybe play low anyway.
			}
			else
			{
				choice = CardsUtil.whoWins(losingPlays, lead, trump); // play strongest card that still loses
				// TODO: better definition of 'strongest'
			}
		}
		assert this.getHand().remove(choice);
		return choice;
	}

}
