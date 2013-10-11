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
		return "" + getDenomName() + " of " + suit.getAbbr();
	}

	private String getDenomName() {
		switch(denom)
		{
		case(13):
			return "Ace";
		case(1):
			return "Two";
		case(2):
			return "Three";
		case(3):
			return "Four";
		case(4):
			return "Five";
		case(5):
			return "Six";
		case(6):
			return "Seven";
		case(7):
			return "Eight";
		case(8):
			return "Nine";
		case(9):
			return "Ten";
		case(10):
			return "Jack";
		case(11):
			return "Queen";
		case(12):
			return "King";
		default:
			System.err.println("Card with no denomination!");
			return "NOTHING!";
		}
	}

	/*
	 * given another card, and knowledge of the suit of the lead, and the current trump,
	 * this method returns true if this card beats the other one, and false if it doesnt.
	 * TODO: UNIT TESTS!!!
	 */
	public boolean beats(Card other, Suit lead, Suit trump) {
		if ( other == null ) // no contender (ie, you're the first card being considered)
		{
			return true;
		}
		if (other.getSuit().equals(trump))
		{
			if ( this.getSuit().equals(trump))
			{
				return this.denom > other.getDenom();
			}
			else
			{
				return false;
			}
		}
		else
		{
			if ( this.getSuit().equals(trump))
			{
				return true;
			}
			else
			{
				if ( other.getSuit().equals(lead))
				{
					if ( this.getSuit().equals(lead))
					{
						return this.denom > other.getDenom();
					}
					else
					{
						return false;
					}
				}
				else
				{
					if ( this.getSuit().equals(lead))
					{
						return true;
					}
					else
					{
						return this.denom > other.getDenom();
					}
				}
			}
		}
		
	}
	
	
}
