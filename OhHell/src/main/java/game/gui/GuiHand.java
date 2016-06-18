package game.gui;

import java.awt.Point;
import java.util.List;
import java.util.stream.Collectors;

import game.cards.Hand;

public class GuiHand {

	private static final int MIN_SPACING = 14;
	
	private Hand hand;
	
	private List<GuiCard> guiCards;
	
	private Point center = new Point(100, 200);
	
	public GuiHand(Hand hand) {
		this.hand = hand;
		this.guiCards = hand.stream().map(c->new GuiCard(c, null)).collect(Collectors.toList());
	}
	
	public void arrange() {
		
	}
	
	
	
}
