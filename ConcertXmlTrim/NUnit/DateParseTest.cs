using System.Text.RegularExpressions;
using NUnit.Framework;

namespace ConcertXmlTrim.NUnit
{
	[TestFixture]
	public class DateParseTest
	{
		[Test]
		public void Test1()
		{
			string line = "2013年4月27日（土）";

			if (Regex.IsMatch(line, "[0-9]{4}年[0-9]*月[0-9]*日"))
			{
				Assert.AreEqual(
					"2013/4/27",
					Regex.Replace(line, ".*([0-9]{4})年([0-9]*)月([0-9]*)日.*", "$1/$2/$3"));
			}
			else
			{
				Assert.Fail();
			}
		}
	}
}
