package game;

import utils.misc.StringUtil;
import utils.strategy.CardsUtil;
import utils.ui.UserUtil;
import game.play.Board;
import game.play.Card;
import game.play.Player;

public class HumanPlayer extends Player {

	public HumanPlayer(String name) {
		super(name);
	}

	@Override
	public int bid(int tricksThisHand, int tricksRemaining, boolean restricted) {
		System.out.println("Hello, "+this.getName()+". There are " + tricksThisHand + " tricks to take this hand, " + (tricksThisHand - tricksRemaining) + " have already been bid on.");
		if(restricted)
		{
			System.out.println("You are the dealer, and therefore cannot bid " + tricksRemaining);
		}
		Integer bidRequest = null;
		do
		{
			try
			{
				bidRequest = Integer.parseInt(UserUtil.ask("How many would you like to bid?"));
			}
			catch (NumberFormatException e)
			{
				System.out.println("was that a number?");
				bidRequest = -1;
			}
		} while ( !acceptable(bidRequest, tricksThisHand, tricksRemaining, restricted));
		this.getStatus().setBid(bidRequest);
		return bidRequest;
	}

	private boolean acceptable(Integer bidRequest, int tricksThisHand, int tricksRemaining, boolean restricted) {
		if (bidRequest == null)
		{
			return false;
		}
		if (bidRequest < 0) // dont bid negative
		{
			return false;
		}
		if (bidRequest > tricksThisHand) // dont bid more than the total number of tricks
		{
			return false;
		}
		if (restricted && bidRequest==tricksRemaining) // you're the dealer, cant make total bid = number of tricks.
		{
			return false;
		}
		return true;
	}

	@Override
	public Card playCard(Board board) {
		System.out.println(board.getTrump().getName() + " are trump. You have bid " + this.getStatus().getBid() + " and taken " + this.getStatus().getTaken() + " so far.");
		if(board.getLead()==null)
			{
				System.out.println("You are in the lead.");
			}
		else
		{
			System.out.println(board.getLead().name() + " were lead.");
		}
		System.out.println("Your hand: " + StringUtil.join(",",this.getHand()));
		Card yourPlay = UserUtil.choose("Which card will you play?", CardsUtil.legalPlays(this.getHand(), board.getLead()));
		this.getHand().remove(yourPlay);
		return yourPlay;
	}

}
