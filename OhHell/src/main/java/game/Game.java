package game;

import game.play.GameManager;
import game.play.Player;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.util.ArrayList;
import java.util.List;

import utils.misc.StringUtil;

public class Game {

	private static BufferedReader br;
	
	public static void main(String[] args) throws IOException {
		
		System.out.println("Welcome!");
		
		br = new BufferedReader(new InputStreamReader(System.in));
		
		GameManager manager = new GameManager();
		
		List<Player> players = choosePlayers();
		
		System.out.println(StringUtil.join(", ", players));
		
		manager.play(players);
		
		
		
	}
	
	public static List<Player> choosePlayers() throws IOException
	{
		List<Player> players = new ArrayList<Player>();
		
		do
		{
			players.add(newPlayer());
			System.out.println("Game consists of " + players.size() + " players now.");
		} while ( youWouldLikeToAddAnotherPlayer() );
		
		return players;
	}

	private static Player newPlayer() throws IOException {
		Player newGuy = null;
		
		do {
			System.out.println("Who will play!??\n\t1)Human\n\tComputer");
			String input = br.readLine();
			if( input.contains("1") )
			{
				newGuy = newHuman();
			}
			else if ( input.contains("2") )
			{
				newGuy = newAI();
			}
			else
			{
				System.out.println("Please type a number from the listed options.");
			}
		} while ( newGuy == null );
		
		return newGuy;
	}

	private static Player newAI() throws IOException{
		// TODO: make Player an abstract class and extend it with human and AI classes
		System.out.println("no ai coded yet, just pick human, sorry");
		return null;
	}

	private static Player newHuman() throws IOException {
		// TODO: make this return a 'human' extension of human
		System.out.println("What is this humans name?");
		
		String input = br.readLine();
		Player newGuy = new Player(input);
		
		return newGuy;
	}

	private static boolean youWouldLikeToAddAnotherPlayer() throws IOException {
		System.out.println("Would you like to add another player?");
		
		String input = br.readLine();
		
		if (input.toLowerCase().equals("yes"))
		{
			return true;
		}
		else
		{
			System.out.println("I'll take that as a no");
			return false;
		}
		
	}

}
