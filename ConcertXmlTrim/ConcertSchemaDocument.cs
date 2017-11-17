using System.Collections.Generic;
using System.Xml;

namespace ConcertXmlTrim
{
	/// <summary>
	/// コンサートスキーマXML。
	/// </summary>
	class ConcertSchemaDocument
		: XmlDocument
	{
		private readonly XmlNamespaceManager manager;

		/// <summary>
		/// 指定のxsdファイルをロード。
		/// </summary>
		/// <param name="path">xsdファイルパス</param>
		public ConcertSchemaDocument(string path)
		{
			Load(path);

			manager = new XmlNamespaceManager(NameTable);
			manager.AddNamespace("xsd", "http://www.w3.org/2001/XMLSchema");
		}

		/// <summary>
		/// ホール名一覧を取得。
		/// </summary>
		/// <returns>ホール名一覧</returns>
		public string[] GetHalls()
		{
			XmlNode node =
				SelectSingleNode(
					"/xsd:schema/xsd:simpleType[@name='hallName']/xsd:restriction",
					manager);

			List<string> halls = new List<string>();

			foreach (XmlNode child in node.ChildNodes)
			{
				halls.Add(child.Attributes["value"].Value);
			}

			return halls.ToArray();
		}

		/// <summary>
		/// 演奏者名一覧を取得。
		/// </summary>
		/// <returns>演奏者名一覧</returns>
		public string[] GetPlayerNames()
		{
			XmlNode node =
				SelectSingleNode(
					"/xsd:schema/xsd:simpleType[@name='playerName']/xsd:restriction",
					manager);

			List<string> playerNames = new List<string>();

			foreach (XmlNode child in node.ChildNodes)
			{
				playerNames.Add(child.Attributes["value"].Value);
			}

			return playerNames.ToArray();
		}

		/// <summary>
		/// パート名一覧を取得。
		/// </summary>
		/// <returns>パート名一覧</returns>
		public string[] GetPartNames()
		{
			XmlNode node =
				SelectSingleNode(
					"/xsd:schema/xsd:simpleType[@name='partName']/xsd:restriction",
					manager);

			List<string> partNames = new List<string>();

			foreach (XmlNode child in node.ChildNodes)
			{
				partNames.Add(child.Attributes["value"].Value);
			}

			return partNames.ToArray();
		}

		/// <summary>
		/// 作曲家名一覧を取得。
		/// </summary>
		/// <returns>作曲家名一覧</returns>
		public string[] GetComposerNames()
		{
			XmlNode node =
				SelectSingleNode(
					"/xsd:schema/xsd:simpleType[@name='composerName']/xsd:restriction",
					manager);

			List<string> composerNames = new List<string>();

			foreach (XmlNode child in node.ChildNodes)
			{
				composerNames.Add(child.Attributes["value"].Value);
			}

			return composerNames.ToArray();
		}
	}
}
