using System;
using System.Windows.Forms;

namespace ConcertDbUtility
{
	/// <summary>
	/// 改行ありでテキスト追加可能なテキストボックス
	/// </summary>
	class LineTextBox
		: TextBox
	{
		/// <summary>
		/// 改行ありでテキスト追加
		/// </summary>
        /// <param name="value">追加する文字列</param>
		public void AddText(string value)
		{
			base.Text += value + Environment.NewLine;
		}
	}
}
