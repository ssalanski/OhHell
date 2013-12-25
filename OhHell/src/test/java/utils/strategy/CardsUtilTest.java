package utils.strategy;

import static org.junit.Assert.assertEquals;
import game.cards.Card;
import game.cards.Suit;

import java.util.Collections;
import java.util.Comparator;
import java.util.List;

import org.junit.Before;
import org.junit.Test;

import utils.misc.ListUtil;

public class CardsUtilTest {

	public Card sixOfHearts, fourOfSpades, twoOfSpades, fourOfClubs, jackOfHearts, threeOfDiamonds;
	
	@Before
	public void defineCards()
	{
		sixOfHearts = new Card(6-1, Suit.HEARTS);
		fourOfClubs = new Card(4-1, Suit.CLUBS);
		fourOfSpades = new Card(4-1, Suit.SPADES);
		jackOfHearts = new Card(11-1, Suit.HEARTS);
		threeOfDiamonds = new Card(3-1, Suit.DIAMONDS);
		twoOfSpades = new Card(2-1, Suit.SPADES);
	}
	
	@Test
	public void testComparatorNoTrump()
	{
		Comparator<Card> comparator = CardsUtil.getCardComparator();
		
		List<Card> cards = ListUtil.asList(
				sixOfHearts,
				fourOfSpades,
				twoOfSpades,
				fourOfClubs,
				jackOfHearts,
				threeOfDiamonds);

		System.out.println(cards);
		
		Collections.sort(cards, comparator);

		System.out.println(cards);
		
		assertEquals(cards.get(0),twoOfSpades);
		assertEquals(cards.get(1),fourOfSpades);
		assertEquals(cards.get(2),sixOfHearts);
		assertEquals(cards.get(3),jackOfHearts);
		assertEquals(cards.get(4),fourOfClubs);
		assertEquals(cards.get(5),threeOfDiamonds);
		
	}
	
	
	
}
