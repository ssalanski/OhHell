package game.gui;

import java.awt.Dimension;
import java.awt.Image;
import java.awt.Point;
import java.awt.event.MouseAdapter;
import java.awt.event.MouseEvent;
import java.awt.image.BufferedImage;
import java.io.File;
import java.net.URL;

import javax.imageio.ImageIO;
import javax.swing.ImageIcon;
import javax.swing.JLabel;

import game.cards.Card;
import game.player.Player;

public class GuiCard extends JLabel {

	private static final Dimension CARD_DIMENSION = new Dimension(72,96);
	private static final ImageIcon FACE_DOWN_IMAGE = loadImage("/images/classic-cards/b1fv.png");

	public Card card;
	public Player owner;
	public boolean faceUp;

	private ImageIcon imageIcon;
	
	private CardClickListener cardClickListener;

	public GuiCard(Card card, Player owner) {
		this.card = card;
		this.owner = owner;
		this.imageIcon = loadImage(card);

		this.faceUp = false; // by default
		this.setIcon(FACE_DOWN_IMAGE);
		
		this.cardClickListener = new CardClickListener(this);
		
		this.setVisible(true);
		
	}
	
	public class CardClickListener extends MouseAdapter {

		private GuiCard myCard;
		
		public CardClickListener(GuiCard c) {
			myCard = c;
			myCard.addMouseListener(this);
		}
		
		@Override
		public void mouseClicked(MouseEvent e) {
			// TODO Auto-generated method stub
			switch( e.getButton() ) {
				case MouseEvent.BUTTON1:
					myCard.turnFaceUp();
					break;
				case MouseEvent.BUTTON2:
					myCard.turnFaceDown();
					break;
				case MouseEvent.BUTTON3:
					myCard.flip();
					break;
				default:
					System.out.println(e.getButton() + " " + myCard.card);
					System.out.println(e.getSource());
					break;
			}
			
			Point p = myCard.getLocation();
			
			p.translate((int)(Math.random()*10-5), (int)(Math.random()*10-5));
			
			myCard.setLocation(p);
			
		}

		
	}
	
	public void turnFaceUp() {
		this.faceUp = true;
		this.setIcon(this.imageIcon);
	}
	
	
	
	public void flip() {
		this.faceUp = !this.faceUp;
		this.setIcon(this.faceUp ? this.imageIcon : FACE_DOWN_IMAGE);
	}



	public void turnFaceDown() {
		this.faceUp = false;
		this.setIcon(FACE_DOWN_IMAGE);
	}



	private ImageIcon loadImage(Card card2) {
		return loadImage("/images/classic-cards/" + card2.getSuit().getAbbr() + card2.getDenom() + ".png");
	}

	private static ImageIcon loadImage(String string) {
		Image img = null;
		
		try {
			URL url = GuiCard.class.getClass().getResource(string);
			//URL url = GuiCard.class.getResource(string);
			File imgFile = new File(url.getFile());
			BufferedImage buffImg = ImageIO.read(imgFile);
			img = buffImg.getScaledInstance(CARD_DIMENSION.width, CARD_DIMENSION.height, Image.SCALE_SMOOTH);
		} catch (Exception e) {
			System.out.println(string);
			// TODO Auto-generated catch block
			e.printStackTrace();
		}

		return new ImageIcon(img);
	}

}
