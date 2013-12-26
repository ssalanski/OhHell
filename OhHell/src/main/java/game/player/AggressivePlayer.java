package game.player;

import game.cards.Board;
import game.cards.Card;
import game.cards.Suit;

import java.util.ArrayList;
import java.util.List;

import utils.strategy.CardsUtil;

public class AggressivePlayer extends Player {

	public AggressivePlayer(String name) {
		super(name);
		// TODO Auto-generated constructor stub
	}

	@Override
	public int bid(int tricksThisHand, int tricksRemaining, boolean restricted) {
		int bidRequest = 0;
		if (restricted && tricksRemaining==tricksThisHand)
		{
			bidRequest = tricksThisHand-1;
		}
		else
		{
			bidRequest = tricksThisHand;
		}
		this.getStatus().setBid(bidRequest); //TODO: should this be done in the manager?
		return bidRequest;
	}

	@Override
	public Card playCard(Board board) {
		Suit trump = board.getTrump();
		Suit lead = board.getLead();
		List<Card> legalPlays = CardsUtil.legalPlays(this.getHand(), lead);
		List<Card> winningPlays = new ArrayList<Card>();
		Card currentlyWinning = CardsUtil.whoWins(board.getCards(), lead, trump);
		for ( Card card : legalPlays )
		{
			if ( card.beats(currentlyWinning, lead, trump) )
			{
				winningPlays.add(card);
			}
		}
		Card choice;
		if (winningPlays.size()==0)
		{
			choice = CardsUtil.lowest(legalPlays, trump);
		}
		else
		{
			choice = CardsUtil.whoWins(winningPlays, lead, trump);
		}
		if(!this.getHand().remove(choice))
		{
			throw new IllegalStateException("Tried to play/remove a card("+choice+") that wasnt in your hand");
		}
		return choice;
	}

}
