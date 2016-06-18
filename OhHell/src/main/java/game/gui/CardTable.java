package game.gui;

import java.io.IOException;
import java.net.URL;

import javax.swing.JFrame;
import javax.swing.JPanel;

import game.cards.Deck;

public class CardTable {
	
	JFrame boardFrame = new JFrame("Board");
	
	public CardTable() throws IOException {
		boardFrame.setSize(600, 400);
		boardFrame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
		
		
		
		JPanel tablePanel = new JPanel();
		tablePanel.setSize(600, 400);
		
		Deck d = new Deck();
		
		d.stream().map(c->new GuiCard(c, null)).forEach(tablePanel::add);
		
		boardFrame.add(tablePanel);

		tablePanel.setVisible(true);
		boardFrame.setVisible(true);
		
		
		
		
	}
	
	public static void main(String[] args) throws IOException {
		

		URL x = CardTable.class.getClass().getResource("/images/cnh4.PNG");
		System.out.println(x);
		System.out.println(x.toString() + x.toString() + x.getPath());
		
		CardTable board = new CardTable();
		
		
		
		board.boardFrame.setTitle("aahhhhhh");
	}
	
}
