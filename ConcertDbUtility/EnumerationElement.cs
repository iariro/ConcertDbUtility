using System.Xml;
using NUnit.Framework;

namespace ConcertDbUtility
{
	[TestFixture]
	public class StringEnumeratorTest1
	{
		private const string path =
			@"C:\Documents and Settings\w81515sr\My Documents\Sorcerer My Documents\Private\音楽\コンサート\ConcertSchemaBase.xsd";

		[Test]
		public void Test01()
		{
			XmlDocument document;

			document = new XmlDocument();
			document.Load(path);

			Assert.IsNotNull(document.DocumentElement);
		}

		[Test]
		public void Test02()
		{
			XmlDocument document;
			XmlNamespaceManager manager;

			document = new XmlDocument();
			document.Load(path);

			manager = new XmlNamespaceManager(document.NameTable);
			manager.AddNamespace(
				document.DocumentElement.Prefix,
				document.DocumentElement.NamespaceURI);
			Assert.IsNotNull(
				document.SelectSingleNode
					(document.DocumentElement.Prefix + ":schema", manager));
		}

		[Test]
		public void Test03()
		{
			string prefix;
			XmlDocument document;
			XmlNamespaceManager manager;

			document = new XmlDocument();
			document.Load(path);

			prefix = document.DocumentElement.Prefix;
			manager = new XmlNamespaceManager(document.NameTable);
			manager.AddNamespace(
				prefix,
				document.DocumentElement.NamespaceURI);
			Assert.IsNotNull(
				document.SelectSingleNode(
					prefix + ":schema/" +
					prefix + ":simpleType[@name='hallName']/" +
					prefix + ":restriction", manager));

		}
	}

	/// <summary>
	///
	/// </summary>
	class StringEnumerator1
	{
		/// <summary>
		///
		/// </summary>
		static public void Enumerate
			(XmlDocument document, XmlNode top, string [] values)
		{
			XmlElement enumeration;

			foreach(string s in values)
			{
				if(s.Length > 0)
				{
					// 空文字列ではない。

					enumeration =
						document.CreateElement(
							document.DocumentElement.Prefix,
							"enumeration",
							document.DocumentElement.NamespaceURI);

					enumeration.SetAttribute("value", s);
					top.AppendChild(enumeration);
				}
			}
		}
	}
}
