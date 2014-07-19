package networking;

import game.cards.Card;
import game.cards.Suit;

import java.io.Serializable;
/*
 * This class defines the different type of messages that will be exchanged between the
 * Clients and the Server. 
 * When talking from a Java Client to a Java Server a lot easier to pass Java objects, no 
 * need to count bytes or to wait for a line feed at the end of the frame
 */
public class GameMessage implements Serializable {

	protected static final long serialVersionUID = 1112122200L;

	// The different types of message sent by the Client
	public static enum MessageType
	{
		MESSAGE, // arbitrary text, display to console or something
		LOGOUT,  // client elects to dropout
		SCORES,  // client requests score information
		BID,     // client announces bid
		PLAY;    // client announces card to play
	}
	private MessageType type;
	private String message;
	
	// constructor
	GameMessage(MessageType type, String message) {
		this.type = type;
		this.message = message;
	}
	
	// getters
	MessageType getType() {
		return type;
	}
	String getMessage() {
		return message;
	}
	Card getCard() {
		if ( type == MessageType.PLAY ) {
			// cards encoded as suit-character + number value. ie H2, C12, S7...
			int denom = Integer.parseInt(message.substring(1));
			Suit suit = Suit.valueOf(message.substring(0,1));
			return new Card(denom, suit);
		}
		return null;	
	}
	Integer getBid() {
		if ( type == MessageType.BID ) {
			return Integer.parseInt(message);
		}
		return null;
	}
}

