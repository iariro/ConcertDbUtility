using System.Collections.Generic;

namespace ConcertXmlTrim
{
	/// <summary>
	/// 
	/// </summary>
	class ZenkakuHankakuTable
		: Dictionary<char, char>
	{
		/// <summary>
		/// 
		/// </summary>
		public ZenkakuHankakuTable()
		{
			Add('：', ':');
		}
	}
}
