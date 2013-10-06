package utils.misc;

import java.util.Collection;

public class StringUtil {

	public static String join(String delim, Collection<? extends Object> items)
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
