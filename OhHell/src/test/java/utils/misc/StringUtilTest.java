package utils.misc;

import static org.junit.Assert.assertEquals;

import java.util.List;

import org.junit.Test;

public class StringUtilTest {

	@Test
	public void testJoin()
	{
		String myDelim = ",";
		List<String> myStrings = ListUtil.asList("one","two","three","four");
		assertEquals(StringUtil.join(myDelim, myStrings),"one,two,three,four");
	}
	
}
