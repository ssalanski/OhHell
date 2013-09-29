package utils.misc;

import java.util.ArrayList;
import java.util.List;

public class ListUtil {
	
	public static <T> List<T> asList(T... elements)
	{
		List<T> list = new ArrayList<T>(elements.length);
		for( T element : elements)
		{
			list.add(element);
		}
		return list;
	}
	
}
