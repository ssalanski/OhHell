package game.player;

import static org.junit.Assert.assertEquals;
import static org.junit.Assert.assertTrue;
import game.cards.Card;
import game.cards.Suit;

import org.junit.Before;
import org.junit.Test;

public class AggressivePlayerTest extends CardTest {

	private AggressivePlayer sut;
	
	@Before
	public void setup()
	{
		sut = new AggressivePlayer("sut");
	}
	
	@Test
	public void testHand()
	{
		sut.setHand(precannedHand1);
		assertEquals(4, sut.getHand().size());
		assertTrue(sut.getHand().remove(precannedHand1.get(0)));
		assertEquals(3, sut.getHand().size());
	}
	
	@Test
	public void testPlay()
	{
		sut.setHand(precannedHand1);
		assertEquals(4,sut.getHand().size());
		Card playedCard = sut.playCard(precannedBoard1);
		Card expectedCard = new Card(4,Suit.CLUBS);
		assertTrue(playedCard.equals(expectedCard));
		assertEquals(3,sut.getHand().size());
	}
	
}
