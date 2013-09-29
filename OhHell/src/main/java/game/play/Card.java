package game.play;

import static org.junit.Assert.assertTrue;

public class Card {

	private Suit suit;
	private int denom;

	public Card(int denom, Suit suit)
	{
		assertTrue( denom > 0 && denom <= 13 );
		this.denom = denom;
		this.suit = suit;
	}

	public Suit getSuit() {
		return suit;
	}

	public void setSuit(Suit suit) {
		this.suit = suit;
	}

	public int getDenom() {
		return denom;
	}

	public void setDenom(int denom) {
		assertTrue( denom > 0 && denom <= 13 );
		this.denom = denom;
	}
	
	public String toString()
	{
		return "" + denom + suit.suitAbbr;
	}
	
	
}
