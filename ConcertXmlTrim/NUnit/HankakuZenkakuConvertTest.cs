using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace ConcertXmlTrim.NUnit
{
	[TestFixture]
	public class HankakuZenkakuConvertTest
	{
		[Test]
		public void Test1()
		{
			string zenkaku = "１２：３４";

			Assert.AreEqual("12:34", ZenkakuHankakuConverter.ConvertZenkakuToHankaku(zenkaku));
		}
	}
}
