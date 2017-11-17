using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ConcertXmlTrim
{
	/// <summary>
	/// コンサート情報。
	/// </summary>
	class ConcertInformation
	{
		public int index;
		public string name;
		public string date;
		public string hall;
		public string ryoukin;
		public List<KeyAndValue> composerNameAndTitles;
		public List<KeyAndValue> partAndPlayers;
		public string kaijou;
		public string kaien;

		/// <summary>
		/// 日付。半角数字による。
		/// </summary>
		public string Date
		{
			get { return ZenkakuHankakuConverter.ConvertZenkakuToHankaku(date); }
		}

		/// <summary>
		/// 開場時間。半角数字による。無指定の場合は12:00。なお無指定の場合、開
		/// 演時間の30分前とする。
		/// </summary>
		public string Kaijou
		{
			get
			{
				if (kaijou != null)
				{
					// 開場時間指定あり。

					return ZenkakuHankakuConverter.ConvertZenkakuToHankaku(kaijou);
				}
				else
				{
					// 開場時間指定なし。

					if (this.kaien != null)
					{
						// 開演時間の指定あり。

						string kaien = Kaien;

						int hour = int.Parse(kaien.Substring(0, 2));
						int minute = int.Parse(kaien.Substring(3, 2));

						if (minute >= 30)
						{
							// 繰り下がりなし。

							minute -= 30;
						}
						else
						{
							// 繰り下がりあり。

							minute += 30;
							hour--;
						}

						return string.Format("{0:d2}:{1:d2}", hour, minute);
					}
					else
					{
						// 開演時間の指定なし。

						return "12:00";
					}
				}
			}
		}

		/// <summary>
		/// 開演時間。半角数字による。無指定の場合12:00。
		/// </summary>
		public string Kaien
		{
			get
			{
				if (kaien != null)
				{
					// 開演時間の指定あり。

					return ZenkakuHankakuConverter.ConvertZenkakuToHankaku(kaien);
				}
				else
				{
					// 開演時間の指定なし。

					return "12:00";
				}
			}
		}

		/// <summary>
		/// メンバーの初期化。
		/// </summary>
		/// <param name="index">加工対象XML中のノードインデックス</param>
		public ConcertInformation(int index)
		{
			this.index = index;
			this.composerNameAndTitles = new List<KeyAndValue>();
			this.partAndPlayers = new List<KeyAndValue>();
		}

		/// <summary>
		/// kyoku要素を作曲家属性のみで追加。
		/// </summary>
		/// <param name="composer">作曲家名</param>
		public void AddComposer(string composer)
		{
			composerNameAndTitles.Add(new KeyAndValue(composer, null));
		}

		/// <summary>
		/// 最新のkyoku要素のtitle属性をセット。
		/// </summary>
		/// <param name="title">曲タイトル</param>
		public void SetTitle(string title)
		{
			title = Regex.Replace(title, " *[oO]p. *[0-9]*", string.Empty);
			title = Regex.Replace(title, "[ 　]*作品 *[0-9]*", string.Empty);
			title = Regex.Replace(title, "[ 　]など", string.Empty);
			title = Regex.Replace(title, "[ 　]等", string.Empty);

			composerNameAndTitles[composerNameAndTitles.Count - 1].value = title;
		}

		/// <summary>
		/// player要素をパート属性のみで追加。
		/// </summary>
		/// <param name="part">パート名</param>
		public void AddPart(string part)
		{
			partAndPlayers.Add(new KeyAndValue(part, null));
		}

		/// <summary>
		/// 最新のplayer要素のplayer属性をセット。
		/// </summary>
		/// <param name="player">演奏者名</param>
		public void SetPlayer(string player)
		{
			string part = partAndPlayers[partAndPlayers.Count - 1].key;

			if (player.IndexOf(" ") < 0 && part != "管弦楽")
			{
				// 管弦楽以外でスペースなし。

				player = player.Insert(player.Length / 2, " ");
			}

			partAndPlayers[partAndPlayers.Count - 1].value = player;
		}

		/// <summary>
		/// 「管弦楽」「指揮」枠を確保。
		/// </summary>
		public void SecureRequiredPart()
		{
			foreach (string part in new string[] { "管弦楽", "指揮" })
			{
				bool find = false;

				foreach (KeyAndValue kv in partAndPlayers)
				{
					if (kv.key == part)
					{
						find = true;
						break;
					}
				}

				if (!find)
				{
					partAndPlayers.Add(new KeyAndValue(part, null));
				}
			}
		}

		/// <summary>
		/// パートをキーにソート。順番はスキーマファイル登場順とする。
		/// </summary>
		/// <param name="partNames">パートマスター</param>
		public void SortPlayerByPart(string [] partNames)
		{
			partAndPlayers.Sort(
				delegate(KeyAndValue x, KeyAndValue y)
				{
					int xindex = Array.IndexOf(partNames, x.key);
					int yindex = Array.IndexOf(partNames, y.key);

					return xindex.CompareTo(yindex);
				});
		}
	}
}
