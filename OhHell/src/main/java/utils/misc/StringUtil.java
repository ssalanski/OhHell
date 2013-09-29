package utils.misc;

import java.util.List;

public class StringUtil {

	public static String join(String delim, List<? extends Object> items)
	{
		StringBuilder sb = new StringBuilder();
		for( Object item : items)
		{
			sb.append(delim);
			sb.append(item);
		}
		sb.delete(0, delim.length());
		
		return sb.toString();
	}
	
}
