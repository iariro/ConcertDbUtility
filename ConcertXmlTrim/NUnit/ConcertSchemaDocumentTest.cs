using NUnit.Framework;

namespace ConcertXmlTrim.NUnit
{
	[TestFixture]
	public class ConcertSchemaDocumentTest
	{
		private const string path =
			@"..\..\..\..\..\..\..\Nefertiti の文書\Private\m\c2\ConcertSchema.xsd";

		[Test]
		public void GetHalls()
		{
			ConcertSchemaDocument document = new ConcertSchemaDocument(path);

			string [] halls = document.GetHalls();

			Assert.AreEqual(193, halls.Length);
		}

		[Test]
		public void GetPlayerNames()
		{
			ConcertSchemaDocument document = new ConcertSchemaDocument(path);

			string [] playerNames = document.GetPlayerNames();

			Assert.AreEqual(1518, playerNames.Length);
		}

		[Test]
		public void GetPartNames()
		{
			ConcertSchemaDocument document = new ConcertSchemaDocument(path);

			string [] partNames = document.GetPartNames();

			Assert.AreEqual(42, partNames.Length);
		}

		[Test]
		public void GetComposerNames()
		{
			ConcertSchemaDocument document = new ConcertSchemaDocument(path);

			string [] composerNames = document.GetComposerNames();

			Assert.AreEqual(307, composerNames.Length);
		}
	}
}
