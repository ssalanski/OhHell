package game.play;

import game.score.ScoreCard;

import java.util.List;

public class GameManager {

	private ScoreCard scoreCard;
	private Deck deck;
	private int handNumber;
	private List<Player> players;
	
	public GameManager(int numPlayers)
	{
		scoreCard = new ScoreCard(numPlayers);
	
		for (handNumber = 1; handNumber<=13; handNumber++)
		{
			playHand();
		}
		
		
	}
	
	private void playHand() {
		
		dealCards();
		bidRound();
		for(int i=0;i<cardsThisHand();i++)
		{
			playTrick();
		}
		
	}

	private void playTrick() {
		// TODO Auto-generated method stub
		
	}

	private void bidRound() {
		// TODO Auto-generated method stub
		
	}

	private void dealCards() {
		for (Player player : players)
		{
			deck.drawCards(cardsThisHand());
		}
	}

	private int cardsThisHand()
	{
		return Math.abs(8-handNumber);
	}
	
}
