using System;
using System.Collections.Generic;
using System.Text;

namespace ConcertDbUtility
{
	/// <summary>
	/// 
	/// </summary>
	class KyokumokuRecord
	{
		public readonly int concertId;
		public readonly int composerId;
		public readonly string title;

		/// <summary>
		///
		/// </summary>
		/// <param name="concertId"></param>
		/// <param name="composerId"></param>
		/// <param name="title"></param>
		public KyokumokuRecord(int concertId, int composerId, string title)
		{
			this.concertId = concertId;
			this.composerId = composerId;
			this.title = title;
		}
	}
}
