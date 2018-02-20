using System;
using System.Collections.Generic;
using System.Text;

namespace ConcertDbUtility
{
	/// <summary>
	/// 作曲家情報
	/// </summary>
	class ComposerRecord
	{
		readonly public string name;
        readonly public int namevalue;

		/// <summary>
		/// 指定の値をメンバーに割り当てる
		/// </summary>
		/// <param name="name">作曲家名</param>
        /// <param name="namevalue">知名度</param>
        public ComposerRecord(string name, int namevalue)
		{
			this.name = name;
            this.namevalue = namevalue;
		}
	}
}
