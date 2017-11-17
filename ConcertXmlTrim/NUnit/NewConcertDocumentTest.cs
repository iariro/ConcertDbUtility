using System;
using System.IO;
using System.Text;
using System.Xml;
using NUnit.Framework;

namespace ConcertXmlTrim.NUnit
{
	[TestFixture]
	public class NewConcertDocumentTest
	{
		private const string xmlPath =
			@"..\..\..\..\..\..\..\Nefertiti の文書\Private\m\c2\NewConcert.xml";
		private const string xsdPath =
			@"..\..\..\..\..\..\..\Nefertiti の文書\Private\m\c2\ConcertSchema.xsd";

		private const string newConcertDocumentTemplate1 =
				"<c:concertCollection xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:c=\"concert\" xsi:schemaLocation=\"concert Concert.xsd\">\r\n" +
				"\r\n" +
				"{0}\r\n" +
				"\r\n" +
				"	<concert name=\"\" date=\"2013/1/1\" kaijou=\"12:00\" kaien=\"12:00\">\r\n" +
				"		<hall name=\"\" />\r\n" +
				"		<kyokuCollection>\r\n" +
				"			<kyoku composer=\"\" title=\"\"/>\r\n" +
				"			<kyoku composer=\"\" title=\"\"/>\r\n" +
				"			<kyoku composer=\"\" title=\"\"/>\r\n" +
				"		</kyokuCollection>\r\n" +
				"		<playerCollection>\r\n" +
				"			<player name=\"\" part=\"管弦楽\" />\r\n" +
				"			<player name=\"\" part=\"指揮\" />\r\n" +
				"			<player name=\"\" part=\"\" />\r\n" +
				"		</playerCollection>\r\n" +
				"		<ryoukin value=\"\" />\r\n" +
				"	</concert>\r\n" +
				"\r\n" +
				"</c:concertCollection>";

		//[Test]
		public void Test1()
		{
			Stream stream = new FileStream(xmlPath, FileMode.Open);
			NewConcertDocument document = new NewConcertDocument(stream);
			stream.Close();

			ConcertSchemaDocument xsdDocument = new ConcertSchemaDocument(xsdPath);

			string [] halls = xsdDocument.GetHalls();
			string [] composers = xsdDocument.GetComposerNames();
			string [] parts = xsdDocument.GetPartNames();
			string [] players = xsdDocument.GetPlayerNames();

			document.Trim(halls, composers, parts, players, null);

			document.Save("out.xml");
		}

		[Test]
		public void Test2()
		{
			string xml =
				string.Format(
					newConcertDocumentTemplate1,
					"第153回定期演奏会\r\n" +
					"日時 	2013年5月25日（土）開場 18:00 開演 18：30\r\n" +
					"場所 	カノラホール\r\n" +
					"入場料 	未定\r\n" +
					"指揮 	濱 一\r\n" +
					"ヴァイオリン 	会田 莉凡\r\n" +
					"プログラム\r\n" +
					"\r\n" +
					"第153回定期演奏会\r\n" +
					"\r\n" +
					"\r\n" +
					"\r\n" +
					"ブラームス/大学祝典序曲\r\n" +
					"\r\n" +
					"\r\n" +
					"ブルッフ/ヴァイオリンコンチェルト\r\n" +
					"\r\n" +
					"\r\n" +
					"ショスタコーヴィッチ/交響曲第5番 革命\r\n" +
					"諏訪交響楽団\r\n");

			Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(xml));
			NewConcertDocument document = new NewConcertDocument(stream);
			stream.Close();

			ConcertSchemaDocument xsdDocument = new ConcertSchemaDocument(xsdPath);

			string[] halls = xsdDocument.GetHalls();
			string[] composers = xsdDocument.GetComposerNames();
			string[] parts = xsdDocument.GetPartNames();
			string[] players = xsdDocument.GetPlayerNames();

			document.Trim(halls, composers, parts, players, null);

			Assert.IsTrue(document.OuterXml.IndexOf("kaijou=\"18:00\"") >= 0);
			Assert.IsTrue(document.OuterXml.IndexOf("kaien=\"18:30\"") >= 0);
		}

		[Test]
		public void Test3()
		{
			string xml =
				string.Format(
					newConcertDocumentTemplate1,
					"第19回演奏会\r\n" +
					"\r\n" +
					"　日時：2013年8月18日（Sun）　14時00分開演 (13時30分開場)\r\n" +
					"　場所：三鷹市芸術文化センター・風のホール　→　アクセスマップ\r\n" +
					"　（ＪＲ中央線三鷹駅南口４・５番バスのりばから３つ目「八幡前・芸術文化センター」 下車すぐ。　または徒歩約15分。）\r\n" +
					"\r\n" +
					"\r\n" +
					"　指揮 ： 征矢健之介\r\n" +
					"\r\n" +
					"　曲目 ： コントラバスアンサンブル「コンバース」\r\n" +
					"　　　　　　J. ブラームス／セレナード第１番 ニ長調 op.11\r\n" +
					"　　　　　　R. シューマン／交響曲第４番 ニ短調 op.120\r\n" +
					"\r\n" +
					"\r\n" +
					"　チケット料金：無料\r\n" +
					"Ensemble MUSIKQUELLCHEN\r\n");

			Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(xml));
			NewConcertDocument document = new NewConcertDocument(stream);
			stream.Close();

			ConcertSchemaDocument xsdDocument = new ConcertSchemaDocument(xsdPath);

			string[] halls = xsdDocument.GetHalls();
			string[] composers = xsdDocument.GetComposerNames();
			string[] parts = xsdDocument.GetPartNames();
			string[] players = xsdDocument.GetPlayerNames();

			document.Trim(halls, composers, parts, players, null);

			Assert.IsTrue(document.OuterXml.IndexOf("kaijou=\"13:30\"") >= 0);
			Assert.IsTrue(document.OuterXml.IndexOf("kaien=\"14:00\"") >= 0);
		}

		[Test]
		public void Test4()
		{
			string xml =
				string.Format(
					newConcertDocumentTemplate1,
					"第35回定期演奏会\r\n" +
					"\r\n" +
					"2013年8月4日（日）14:00開演13:30開場　すみだトリフォニーホール\r\n" +
					"\r\n" +
					"指揮／新田 ユリ\r\n" +
					"新田ユリ氏 	\r\n" +
					"\r\n" +
					" \r\n" +
					"\r\n" +
					"ヴァーグナー／楽劇「トリスタンとイゾルデ」前奏曲と愛の死\r\n" +
					"\r\n" +
					"マーラー／交響曲第5番嬰ハ短調\r\n" +
					"日立フィルハーモニー管弦楽団\r\n");

			Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(xml));
			NewConcertDocument document = new NewConcertDocument(stream);
			stream.Close();

			ConcertSchemaDocument xsdDocument = new ConcertSchemaDocument(xsdPath);

			string[] halls = xsdDocument.GetHalls();
			string[] composers = xsdDocument.GetComposerNames();
			string[] parts = xsdDocument.GetPartNames();
			string[] players = xsdDocument.GetPlayerNames();

			document.Trim(halls, composers, parts, players, null);

			System.Diagnostics.Debug.WriteLine(document.OuterXml);
			Assert.IsTrue(document.OuterXml.IndexOf("kaijou=\"13:30\"") >= 0);
			Assert.IsTrue(document.OuterXml.IndexOf("kaien=\"14:00\"") >= 0);
		}
	}
}
