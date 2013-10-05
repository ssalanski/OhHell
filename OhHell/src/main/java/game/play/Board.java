package game.play;

import java.util.ArrayList;

import utils.misc.StringUtil;

public class Board extends ArrayList<Card>{

	public String toString()
	{
		return "Board: " + StringUtil.join(", ", this);
	}

}
