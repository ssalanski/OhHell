package utils.misc;

import static org.junit.Assert.assertEquals;

import java.util.List;

import org.junit.Test;

public class ListUtilTest {

	@Test
	public void testAsList()
	{
		String s1 = "one";
		String s2 = "two";
		String s3 = "three";
		String s4 = "four";
		List<String> strings = ListUtil.asList(s1,s2,s3,s4);
		assertEquals(4, strings.size());
		assertEquals(s1, strings.get(0));
		assertEquals(s2, strings.get(1));
		assertEquals(s3, strings.get(2));
		assertEquals(s4, strings.get(3));
	}
	
}
