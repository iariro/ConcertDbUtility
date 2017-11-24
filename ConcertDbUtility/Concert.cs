using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace ConcertDbUtility
{
	/// <summary>
	/// コンサート情報
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
		/// XML要素からコンサート情報を読み込み
        /// 2017/11/24 : 新スキーマに対応
		/// </summary>
		/// <param name="element">concert要素</param>
		public Concert(XmlElement element)
		{
			name = element.Attributes["name"].Value;

			date = DateTime.Parse
				(element.Attributes["date"].Value);
			kaijou = DateTime.Parse(
				element.Attributes["date"].Value + " " +
				element.Attributes["kaijou"].Value);
			kaien = DateTime.Parse(
				element.Attributes["date"].Value + " " +
				element.Attributes["kaien"].Value);

            if (element.Attributes["hall"] != null)
            {
                // 属性あり

                hallName = element.Attributes["hall"].Value;
            }
            else
            {
                // 属性なし

                XmlNode hallNode = element.SelectSingleNode("hall");
                if (hallNode != null)
                {
                    // hall要素あり

                    hallName = hallNode.Attributes["name"].Value;
                }
            }

            if (element.Attributes["ryoukin"] != null)
            {
                // 属性あり

                ryoukin = element.Attributes["ryoukin"].Value;
            }
            else
            {
                // 属性なし

                XmlNode ryoukinNode = element.SelectSingleNode("ryoukin");
                if (ryoukinNode != null)
                {
                    // ryoukin要素あり

                    ryoukin = ryoukinNode.Attributes["value"].Value;
                }
            }

			kyokuCollection = new List<KyokumokuElement>();
            XmlNode kyokuCollectionNode = element.SelectSingleNode("kyokuCollection");
			if(kyokuCollectionNode != null)
			{
                // kyokuCollection要素あり

                foreach(XmlNode kyoku in kyokuCollectionNode.ChildNodes)
				{
					if(kyoku.NodeType == XmlNodeType.Element)
					{
                        // 要素である

                        kyokuCollection.Add(new KyokumokuElement(kyoku));
					}
				}
			}

			playerCollection = new List<Player>();
            XmlNode playerCollectionNode = element.SelectSingleNode("playerCollection");
			if(playerCollectionNode != null)
			{
                // playerCollection要素あり

                foreach (XmlNode player in playerCollectionNode.ChildNodes)
                {
                    if (player != null && player.NodeType == XmlNodeType.Element)
                    {
                        // 要素である

                        playerCollection.Add(new Player(player));
                    }
                    else
                    {
                        // 要素ではない

                        System.Diagnostics.Debug.WriteLine(name);
                    }
                }
			}
		}

		/// <summary>
		/// デバッグ用の文字列化
		/// </summary>
		/// <returns>全内容文字列</returns>
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
