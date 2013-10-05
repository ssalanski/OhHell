package game.play;

import java.util.ArrayList;

import utils.misc.StringUtil;

public class Hand extends ArrayList<Card>{

	public String toString()
	{
		return "Hand: " + StringUtil.join(", ", this);
	}
	
}
