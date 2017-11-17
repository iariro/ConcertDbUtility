using System.Collections.Generic;
using System.Text;

namespace ConcertXmlTrim
{
	/// <summary>
	/// 全角→半角変換。
	/// </summary>
	class ZenkakuHankakuConverter
	{
		static private readonly Dictionary<char, char> table =
			new ZenkakuHankakuTable();

		/// <summary>
		/// 全角→半角変換。
		/// </summary>
		/// <param name="zenkaku">全角文字列</param>
		/// <returns>半角文字列</returns>
		static public string ConvertZenkakuToHankaku(string zenkaku)
		{
			if (zenkaku == null)
			{
				// nullが指定された。

				return null;
			}

			StringBuilder builder = new StringBuilder();

			for (int i = 0; i < zenkaku.Length; i++)
			{
				if (table.ContainsKey(zenkaku[i]))
				{
					// テーブル分の変換対象。

					builder.Append(table[zenkaku[i]]);
				}
				else
				{
					// テーブル分の変換対象以外。

					if (zenkaku[i] >= '０' && zenkaku[i] <= '９')
					{
						// 全角数字。

						builder.Append((char)('0' + zenkaku[i] - '０'));
					}
					else
					{
						// それ以外。

						builder.Append(zenkaku[i]);
					}
				}
			}

			return builder.ToString();
		}
	}
}
