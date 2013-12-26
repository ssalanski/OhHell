package game;

import game.cards.Board;
import game.cards.Card;
import game.cards.Deck;
import game.cards.Hand;
import game.player.HumanPlayer;
import game.player.Player;
import game.score.HandRecord;
import game.score.ScoreCard;
import game.score.Status;

import java.util.List;

import utils.misc.StringUtil;

public class GameManager {

	private ScoreCard scoreCard;
	private Deck deck;
	private int handNumber;
	private HandRecord thisHand;
	private List<Player> players;
	private Player inLead;
	private Board board;
	
	public GameManager()
	{
		
		
	}

	public void play(List<Player> players)
	{
		this.players = players;
		scoreCard = new ScoreCard(players.size());
		
		for (handNumber = 1; handNumber<=13; handNumber++)
		{
			inLead = players.get((handNumber-1)%players.size());
			deck = new Deck();
			deck.shuffle();
			board = new Board();
			playHand();
		}
	}
	
	private void playHand() {
		
		dealCards();
		System.out.println(thisHand.getBoard().getTrump() + " are trump.");
		
		for(Player player: players)
		{
			if( player instanceof HumanPlayer)
			{
				System.out.println(player.getName()+"'s hand: " + player.getHand());
			}
		}
		
		bidRound();
		for (Player player : players)
		{
			System.out.println(player.getName() + " bid " + player.getStatus().getBid());
		}
		
		
		for(int i=0;i<cardsThisHand();i++)
		{
			System.out.println("its trick number " + (i+1)+", and "+inLead.getName()+" is in the lead.");
			for (Player player : players)
			{
				System.out.println("[bid/tricks] " + describeTrickStatus(player));
			}
			inLead = playTrick();
			inLead.getStatus().incrementTricksTaken();
			System.out.println(inLead.getName() + " won that trick");
		}
		thisHand.assessScores();
		scoreCard.recordHand(thisHand);

		for (Player player : players)
		{
			System.out.println("[result]" + describeHandResults(player));
		}
		
		for(Player player: players)
		{
			player.setStatus(new Status(player.getStatus())); // maybe dont do this in the playHand method, but right after calling it...
		}
	}

	private String describeHandResults(Player player) {
		String status;
		int remaining = player.getStatus().getBid() - player.getStatus().getTaken();
		if (remaining == 0)
			status = " made their bid of " + player.getStatus().getBid();
		else if (remaining > 0)
			status = " was " + remaining + " below their bid of " + player.getStatus().getBid();
		else
			status = " was " + (-remaining) + " over their bid of " + player.getStatus().getBid();
		String score = " scoring " + player.getStatus().getHandScore() + " points, totaling " + player.getStatus().getTotalScore();
		return player.getName() + status + score;
	}

	private String describeTrickStatus(Player player)
	{
		String status;
		int remaining = player.getStatus().getBid() - player.getStatus().getTaken();
		if (remaining == 0)
			status = " has made their bid of " + player.getStatus().getBid();
		else if (remaining > 0)
			status = " needs " + remaining + " more tricks to make their bid of " + player.getStatus().getBid();
		else
			status = " has taken " + (-remaining) + " more than their bid of " + player.getStatus().getBid();
		return player.getName() + status;
	}

	/*
	 * returns the player that won the trick
	 */
	private Player playTrick() {
		board.clear();
		int leadOffset = players.indexOf(inLead);
		Player theirTurn = inLead;
		Card playedCard;
		for(int turnIndex = 0; turnIndex<players.size(); turnIndex++)
		{
			describeBoard();
			theirTurn = players.get((turnIndex+leadOffset)%players.size());
			System.out.println("its turn number " + (turnIndex+1)+": "+theirTurn.getName());
			playedCard = theirTurn.playCard(board);
			System.out.println(theirTurn.getName() + " played the " + playedCard.toString());
			board.put(playedCard,theirTurn);
		}
		
		return board.determineWinner();
		
		
	}

	private void bidRound() {
		int tricksRemaining = cardsThisHand();
		int leadOffset = players.indexOf(inLead);
		Player theirTurn = inLead;
		for(int turnIndex = 0; turnIndex<players.size(); turnIndex++)
		{
			theirTurn = players.get((turnIndex+leadOffset)%players.size());
			tricksRemaining -= theirTurn.bid(cardsThisHand(),tricksRemaining,turnIndex==players.size()-1);
		}
	}

	private void dealCards() {
		thisHand = new HandRecord(players);
		thisHand.setBoard(board);
		System.out.println("Dealing "+cardsThisHand()+" cards to everyone");
		for (Player player : players)
		{
			player.setHand(new Hand(deck.drawCards(cardsThisHand())));
		}
		Card trump = deck.drawCard();
		thisHand.getBoard().setTrump(trump.getSuit());
		for (Player player : players)
		{
			player.getHand().organize(trump.getSuit());
		}
	}

	/*
	 * reads the handNumberField and returns the number of cards in the given hand.
	 * perhaps allow this to be overridden for different variations on the rules. (along with scoring)
	 */
	private int cardsThisHand()
	{
		return Math.abs(7-handNumber)+1;
	}
	
	public String listPlayers()
	{
		return StringUtil.join(", ",players);
	}
	
	public String displayBoard()
	{
		return StringUtil.join(", ",board.getCards());
	}
	
	public String describeBoard()
	{
		String description = board.getTrump() + " is trump\n";
		String temp = "";
		for(Card card : board.getCards())
		{
			if (board.get(card).equals(inLead))
			{
				description += inLead.getName() + " led the " + card.toString() + "\n";
			}
			else {
				temp += board.get(card).getName() + " played the " + card + "\n";
			}
		}
		description += temp;
		if(	board.getCards().size()==0 )
		{
			description += inLead.getName() + " is in the lead.";
		}
		else {
			int remaining = players.size() - board.getCards().size();
			if(remaining == 0)
			{
				description += board.determineWinner().getName() + " won that trick.";
			}
			else
			{
				description += remaining + " players still to play";
			}
		}
		return description;
	}
	
}
