using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace ConcertDbUtility
{
	/// <summary>
	/// 
	/// </summary>
	class Player
	{
		public readonly string playerName;
		public readonly string partName;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="node"></param>
		public Player(XmlNode node)
		{
			playerName = node.Attributes["name"].Value;
			partName = node.Attributes["part"].Value;
		}
	}
}
