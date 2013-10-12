package game;

import game.play.GameManager;
import game.play.Player;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.util.ArrayList;
import java.util.List;

import utils.misc.ListUtil;
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
			System.out.println("Who will play!??\n\t1)Human\n\t2)Computer");
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
		System.out.println("What is this AI's name?");
		String name = br.readLine();

		System.out.println("Which kind of AI will "+name+" be?");
		System.out.println(StringUtil.join("\n", ListUtil.asList("\t1) Agressive","\t2) Passive","\t3) Intelligent","\t4) WILDCARD!!")));
		Player newGuy = null;

		do
		{
			String input = br.readLine();
			if( input.contains("1") )
			{
				newGuy = new AggressivePlayer(name);
			}
			else if ( input.contains("2") )
			{
				newGuy = new PassivePlayer(name);
			}
			else if ( input.contains("3") )
			{
				//newGuy = new IntelligentPlayer(name);
				System.out.println("No algorithm for this yet, feel free to contribute!\n(also, pick again)");
			}
			else if ( input.contains("4") )
			{
				newGuy = new WildcardPlayer(name);
			}
			else
			{
				System.out.println("Please type a number from the listed options.");
			}
		} while ( newGuy == null );

		return newGuy;
	}

	private static Player newHuman() throws IOException {
		System.out.println("What is this humans name?");
		
		String input = br.readLine();
		Player newGuy = new HumanPlayer(input);
		
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
