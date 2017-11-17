using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace ConcertDbUtility
{
	/// <summary>
	/// 
	/// </summary>
	class ConcertCollectionDocument
		: XmlDocument
	{
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public List<Concert> GetConcertCollection()
		{
			List<Concert> ret;

			ret = new List<Concert>();
			foreach(XmlElement element in DocumentElement.ChildNodes)
			{
				ret.Add(new Concert(element));
			}

			return ret;
		}
	}
}
