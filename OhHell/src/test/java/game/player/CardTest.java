package game.player;

import java.util.List;

import utils.misc.ListUtil;
import game.cards.Board;
import game.cards.Card;
import game.cards.Hand;
import game.cards.Suit;

public abstract class CardTest {

	protected Hand precannedHand1 = new Hand(ListUtil.asList(new Card(4,Suit.CLUBS),new Card(7,Suit.DIAMONDS),new Card(13,Suit.SPADES),new Card(2,Suit.HEARTS)));
	
	protected Hand precannedHand2 = new Hand(ListUtil.asList(new Card(1,Suit.CLUBS),new Card(10,Suit.DIAMONDS),new Card(11,Suit.SPADES),new Card(11,Suit.HEARTS)));
	
	protected Board precannedBoard1 = buildBoard(ListUtil.asList(new Card(2,Suit.CLUBS),new Card(6,Suit.DIAMONDS),new Card(12,Suit.SPADES),new Card(4,Suit.HEARTS)));

	private Board buildBoard(List<Card> cards) {
		Board b = new Board();
		b.setTrump(Suit.SPADES);
		for(Card card : cards)
		{
			b.put(card, null);
		}
		return b;
	}
	
}
