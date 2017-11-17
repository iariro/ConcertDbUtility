using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace ConcertDbUtility
{
	/// <summary>
	/// 
	/// </summary>
	class KyokumokuElement
	{
		public readonly string composerName;
		public readonly string title;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="node"></param>
		public KyokumokuElement(XmlNode node)
		{
			composerName = node.Attributes["composer"].Value;
			title = node.Attributes["title"].Value;
		}
	}
}
