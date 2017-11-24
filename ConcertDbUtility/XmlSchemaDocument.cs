using System.Xml;
using NUnit.Framework;

namespace ConcertDbUtility
{
	[TestFixture]
	public class XmlSchemaDocumentTest
	{
		private const string path =
			@"C:\Documents and Settings\w81515sr\My Documents\Sorcerer My Documents\Private\音楽\コンサート\ConcertSchemaBase.xsd";

		[Test]
		public void Test01()
		{
			XmlSchemaDocument document;

			document = new XmlSchemaDocument();
			document.Load(path);

			document.Enumerate(
				new string []
				{
					"schema",
					"simpleType[@name='hallName']",
					"restriction"
				},
				new string []
				{
					"a",
					"b",
					"c"
				});

			document.Save(System.Console.Out);
		}
	}

	/// <summary>
	///
	/// </summary>
	class XmlSchemaDocument
		: XmlDocument
	{
		private string uri, prefix;
		private XmlNamespaceManager manager;

		/// <summary>
		///
		/// </summary>
		/// <param name="filename"></param>
		public override void Load(string filename)
		{
			base.Load(filename);

			uri = DocumentElement.NamespaceURI;
			prefix = DocumentElement.Prefix;

			manager = new XmlNamespaceManager(NameTable);
			manager.AddNamespace(prefix, uri);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="nest"></param>
		/// <param name="values"></param>
		public void Enumerate(string [] nest, string [] values)
		{
			string path;
			XmlNode top;
			XmlElement enumeration;

			// パスの組み立て。
			path = string.Empty;
			foreach(string s in nest)
			{
				path += "/" + prefix + ":" + s;
			}

			top = SelectSingleNode(path, manager);

			foreach(string s in values)
			{
				if(s.Length > 0)
				{
					// 空文字列ではない。

					enumeration = CreateElement(prefix, "enumeration", uri);
					enumeration.SetAttribute("value", s);
					top.AppendChild(enumeration);
				}
			}
		}
	}
}
