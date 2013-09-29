package utils.misc;

import java.util.List;

public class StringUtil {

	public static String join(String delim, List<String> substrings)
	{
		StringBuilder sb = new StringBuilder();
		for( String substring : substrings)
		{
			sb.append(delim);
			sb.append(substring);
		}
		sb.delete(0, delim.length());
		
		return sb.toString();
	}
	
}
