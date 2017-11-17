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
		public readonly bool active;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <param name="siteurl"></param>
		/// <param name="active"></param>
		public PlayerRecord(string name, string siteurl, bool active)
		{
			this.name = name;
			this.siteurl = siteurl;
			this.active = active;
		}
	}
}
