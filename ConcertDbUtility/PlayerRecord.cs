using System;
using System.Collections.Generic;
using System.Text;

namespace ConcertDbUtility
{
	/// <summary>
	/// 
	/// </summary>
	class PlayerRecord
	{
		public readonly string name;
		public readonly string siteurl;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <param name="siteurl"></param>
		public PlayerRecord(string name, string siteurl)
		{
			this.name = name;
			this.siteurl = siteurl;
		}
	}
}
