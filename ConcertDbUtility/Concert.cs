using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace ConcertDbUtility
{
	/// <summary>
	///
	/// </summary>
	class Concert
	{
		readonly public string name;
		readonly public DateTime date;
		readonly public DateTime kaijou;
		readonly public DateTime kaien;
		readonly public string hallName;
		readonly public List<KyokumokuElement> kyokuCollection;
		readonly public List<Player> playerCollection;
		readonly public string ryoukin;

		/// <summary>
		///
		/// </summary>
		/// <param name="element"></param>
		public Concert(XmlElement element)
		{
			XmlNode hallNode;
			XmlNode kyokuCollectionNode;
			XmlNode playerCollectionNode;
			XmlNode ryoukinNode;

			name = element.Attributes["name"].Value;

			date = DateTime.Parse
				(element.Attributes["date"].Value);
			kaijou = DateTime.Parse(
				element.Attributes["date"].Value + " " +
				element.Attributes["kaijou"].Value);
			kaien = DateTime.Parse(
				element.Attributes["date"].Value + " " +
				element.Attributes["kaien"].Value);

			hallNode = element.SelectSingleNode("hall");
			if(hallNode != null)
			{
				hallName = hallNode.Attributes["name"].Value;
			}

			kyokuCollection = new List<KyokumokuElement>();
			kyokuCollectionNode = element.SelectSingleNode("kyokuCollection");
			if(kyokuCollectionNode != null)
			{
				foreach(XmlNode kyoku in kyokuCollectionNode.ChildNodes)
				{
					if(kyoku.NodeType == XmlNodeType.Element)
					{
						kyokuCollection.Add(new KyokumokuElement(kyoku));
					}
				}
			}

			playerCollection = new List<Player>();
			playerCollectionNode = element.SelectSingleNode("playerCollection");
			if(playerCollectionNode != null)
			{
				foreach(XmlNode player in playerCollectionNode.ChildNodes)
				{
					if(player.NodeType == XmlNodeType.Element)
					{
						if(player != null)
						{
							playerCollection.Add(new Player(player));
						}
						else
						{
							System.Diagnostics.Debug.WriteLine(name);
						}
					}
				}
			}

			ryoukinNode = element.SelectSingleNode("ryoukin");
			if(ryoukinNode != null)
			{
				ryoukin = ryoukinNode.Attributes["value"].Value;
			}
		}

		/// <summary>
		///
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			string ret;

			ret =
				name + " " +
				date.ToShortDateString() + " " +
				kaijou.ToShortTimeString() + " " +
				kaien.ToShortTimeString() + " " +
				hallName + " " +
				ryoukin + "\n";

			foreach(KyokumokuElement kyoku in kyokuCollection)
			{
				ret += "\t" + kyoku.composerName + " " + kyoku.title + "\n";
			}

			foreach(Player player in playerCollection)
			{
				ret += "\t" + player.playerName + " " + player.partName + "\n";
			}

			return ret;
		}
	}
}
