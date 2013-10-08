package game.play;

import java.util.ArrayList;
import java.util.List;

public class Deck extends ArrayList<Card>{

	public Deck()
	{
		super(buildCleanDeck());
	}

	private static List<Card> buildCleanDeck() {
		List<Card> deck = new ArrayList<Card>(52);
		for(Suit suit : Suit.allSuits())
		{
			for(int denom = 1; denom <= 13; denom++)
			{
				deck.add(new Card(denom, suit));
			}
		}
		return deck;
	}
	
	public void shuffle()
	{
		Card temp;
		int j;
		for(int i=0;i<this.size();i++)
		{
			j = (int)(Math.random()*i);
			temp = this.get(i);
			this.set(i, this.get(j));
			this.set(j, temp);
		}
	}
	
	public Card peekCard()
	{
		return this.peekCard(0);
	}
	
	public Card peekCard(int index)
	{
		return this.get(index);
	}
	
	public Card drawCard()
	{
		return this.drawCard(0);
	}
	
	public List<Card> drawCards(int count)
	{
		List<Card> cards = new ArrayList<Card>(count);
		for(int i = 0;i<count;i++)
		{
			cards.add(drawCard());
		}
		return cards;
	}
	
	public Card drawCard(int index)
	{
		if (this.size()==0)
		{
			System.err.println("Your deck is empty, yet you tried to draw a card! WHATS WRONG WITH YOU!?!?!????");
			System.exit(1);
		}
		return this.remove(index);
	}
	
}
