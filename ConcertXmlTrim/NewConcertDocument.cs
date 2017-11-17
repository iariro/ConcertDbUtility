using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;

namespace ConcertXmlTrim
{
	/// <summary>
	/// NewConcert.xmlドキュメント。
	/// </summary>
	class NewConcertDocument
		: XmlDocument
	{
		static private readonly string [][] composerSurrogates =
			{
				new string [] { "ベートーベン", "ベートーヴェン" },
				new string [] { "ショスタコーヴィッチ", "ショスタコーヴィチ" },
				new string [] { "ショスタコービッチ", "ショスタコーヴィチ" },
				new string [] { "ヴァーグナー", "ワーグナー" },
				new string [] { "サン＝サーンス", "サン=サーンス" }
			};

		/// <summary>
		/// 指定のxmlファイルをロード。
		/// </summary>
		/// <param name="path">xmlファイルパス</param>
		public NewConcertDocument(Stream stream)
		{
			PreserveWhitespace = true;
			Load(stream);
		}

		/// <summary>
		/// XMLを自動加工。
		/// </summary>
		/// <param name="halls">ホール名一覧</param>
		/// <param name="composers">作曲家名一覧</param>
		/// <param name="partNames">パート名一覧</param>
		/// <param name="playerNames">演奏者名一覧</param>
		/// <param name="worker">バックグランドワーカー</param>
		public void Trim(string [] halls, string [] composers,
			string [] partNames, string [] playerNames, BackgroundWorker worker)
		{
			ConcertInformation concert = null;

			int childCount = DocumentElement.ChildNodes.Count;

			for (int i=0 ; i<childCount ; i++)
			{
				if (worker != null)
				{
					// BackgroundWorkerは指定されている。

					worker.ReportProgress(i * 100 / childCount);
				}

				XmlNode node = DocumentElement.ChildNodes[i];

				if (node.NodeType == XmlNodeType.Text)
				{
					// テキストノード。

					concert = new ConcertInformation(i);

					LineType lineType = LineType.None;

					StringReader reader = new StringReader(node.Value);
					StringWriter writer = new StringWriter();

					string line;

					while ((line = reader.ReadLine()) != null)
					{
						line = line.Trim();

						try
						{
							if (line.Length > 0)
							{
								// ホワイトスペースのみの行ではない。

								bool kakutei = false;

								if (lineType == LineType.Composer)
								{
									// 直前は作曲家。

									concert.SetTitle(line);
									lineType = LineType.None;
									kakutei = true;
									continue;
								}
								else
								{
									// 直前は何もなし。

									if (concert.name == null &&
										(line.IndexOf("演奏会") >= 0 ||
										line.EndsWith("コンサート") ||
										line.EndsWith("のつどい") ||
										line.EndsWith("Concert")))
									{
										// コンサート名と判断。

										for (int j = 0; j < playerNames.Length; j++)
										{
											if (line.StartsWith(playerNames[j]))
											{
												// オーケストラ名の行である。

												concert.AddPart("管弦楽");
												concert.SetPlayer(playerNames[j]);
												line = Regex.Replace(line, playerNames[j] + "　*", string.Empty);
												break;
											}
										}

										line = line.Replace("のお知らせ", string.Empty);

										concert.name = line;
										continue;
									}

									if (Regex.IsMatch(line, "曲目[:：]*$") || line == "プログラム")
									{
										// 不要な行と判断。

										continue;
									}

									if (Regex.IsMatch(line, "[0-9]{4}年[ 　]*[0-9]*月[ 　]*[0-9]*日"))
									{
										// 日付を含む。

										concert.date =
											Regex.Replace(line, ".*([0-9]{4})年[ 　]*([0-9]*)月[ 　]*([0-9]*)日.*", "$1/$2/$3");
										kakutei = true;
									}
									else if (Regex.IsMatch(line, "[０-９]{4}年[０-９]*月[０-９]*日"))
									{
										// 日付を含む。

										concert.date =
											Regex.Replace(line, ".*([０-９]{4})年([０-９]*)月([０-９]*)日.*", "$1/$2/$3");
										kakutei = true;
									}
									else if (Regex.IsMatch(line, "[0-9]{2}[/.][0-9]*[/.][0-9]*"))
									{
										// 日付を含む。

										concert.date =
											Regex.Replace(line, ".*([0-9]{2})[/.]([0-9]*)[/.]([0-9]*).*", "20$1/$2/$3");
										kakutei = true;
									}
									else if (Regex.IsMatch(line, "[0-9]{4}/[0-9]*/[0-9]*"))
									{
										// 日付を含む。

										concert.date =
											Regex.Replace(line, ".*([0-9]{4})/([0-9]*)/([0-9]*).*", "$1/$2/$3");
										kakutei = true;
									}

									if (Regex.IsMatch(line, "[0-9０-９][0-9０-９][:：][0-9０-９][0-9０-９] *開場[0-9０-９][0-9０-９][:：][0-9０-９][0-9０-９] *開演"))
									{
										// 「13:30開場 14:00開演」の形式の開場・開演時刻を含む。

										concert.kaijou = Regex.Replace(line, ".*([0-9０-９][0-9０-９])[:：]([0-9０-９][0-9０-９])開場.*", "$1:$2");
										concert.kaien = Regex.Replace(line, ".*([0-9０-９][0-9０-９])[:：]([0-9０-９][0-9０-９]) *開演.*", "$1:$2");
										kakutei = true;
									}
									else if (Regex.IsMatch(line, "開場[ 　]*：* *[0-9０-９][0-9０-９][:：][0-9０-９][0-9０-９][ 　]*開演[ 　]*[0-9０-９][0-9０-９][:：][0-9０-９][0-9０-９]"))
									{
										// 「開場13:30 開演14:00」の形式の開場・開演時刻を含む。

										concert.kaijou = Regex.Replace(line, ".*開場[ 　]*：* *([0-9０-９][0-9０-９])[:：]([0-9０-９][0-9０-９]).*", "$1:$2");
										concert.kaien = Regex.Replace(line, ".*開演[ 　]*([0-9０-９][0-9０-９])[:：]([0-9０-９][0-9０-９]).*", "$1:$2");
										kakutei = true;
									}
									else
									{
										if (Regex.IsMatch(line, "[0-9０-９][0-9０-９][:：][0-9０-９][0-9０-９]開場"))
										{
											// 「13:30開場」の形式の開場時刻を含む。

											concert.kaijou = Regex.Replace(line, ".*([0-9０-９][0-9０-９])[:：]([0-9０-９][0-9０-９])開場.*", "$1:$2");
											kakutei = true;
										}
										else if (Regex.IsMatch(line, "[0-9０-９][0-9０-９]時[0-9０-９][0-9０-９]分開場"))
										{
											// 「13時30分開場」の形式の開場時刻を含む。

											concert.kaijou = Regex.Replace(line, ".*([0-9０-９][0-9０-９])時([0-9０-９][0-9０-９])分開場.*", "$1:$2");
											kakutei = true;
										}
										else if (Regex.IsMatch(line, "開場[ 　]*：* *[0-9][0-9][:：][0-9][0-9]"))
										{
											// 「開場：13:30」の形式の開場時刻を含む。

											concert.kaijou = Regex.Replace(line, ".*開場[ 　]*：* *([0-9][0-9])[:：]([0-9][0-9]).*", "$1:$2");
											kakutei = true;
										}

										if (Regex.IsMatch(line, "[0-9０-９][0-9０-９][:：][0-9０-９][0-9０-９] *開演"))
										{
											// 「14:00開演」の形式の開演時刻を含む。

											concert.kaien = Regex.Replace(line, ".*([0-9０-９][0-9０-９])[:：]([0-9０-９][0-9０-９]) *開演.*", "$1:$2");
											kakutei = true;
										}
										else if (Regex.IsMatch(line, "[0-9０-９][0-9０-９]時[0-9０-９][0-9０-９]分 *開演"))
										{
											// 「14時00分開演」の形式の開演時刻を含む。

											concert.kaien = Regex.Replace(line, ".*([0-9０-９][0-9０-９])時([0-9０-９][0-9０-９])分 *開演.*", "$1:$2");
											kakutei = true;
										}
										else if (Regex.IsMatch(line, "[0-9０-９][0-9０-９]時開演"))
										{
											// 「14時開演」の形式の開演時刻を含む。

											concert.kaien = Regex.Replace(line, ".*([0-9０-９][0-9０-９])時開演.*", "$1:00");
											kakutei = true;
										}
										else if (Regex.IsMatch(line, "午後[0-9０-９]時開演"))
										{
											// 「午後2時開演」の形式の開演時刻を含む。

											string hour12 = Regex.Replace(line, ".*午後([0-9０-９])時開演.*", "$1");
											concert.kaien = string.Format("{0}:00", int.Parse(ZenkakuHankakuConverter.ConvertZenkakuToHankaku(hour12)) + 12);
											kakutei = true;
										}
										else if (Regex.IsMatch(line, "開演[ 　]*[0-9][0-9][:：][0-9][0-9]"))
										{
											// 「開演14:00」の形式の開演時刻を含む。

											concert.kaien = Regex.Replace(line, ".*開演[ 　]*([0-9][0-9])[:：]([0-9][0-9]).*", "$1:$2");
											kakutei = true;
										}
										else if (Regex.IsMatch(line, "開演[ 　]*：* *[0-9][0-9][:：][0-9][0-9]"))
										{
											// 「開演：14:00」の形式の開演時刻を含む。

											concert.kaien = Regex.Replace(line, ".*開演[ 　]*：* *([0-9][0-9])[:：]([0-9][0-9]).*", "$1:$2");
											kakutei = true;
										}
									}

									if (kakutei)
									{
										// 行内容確定。

										continue;
									}

									if (line.EndsWith("円"))
									{
										// 料金情報を含む。

										concert.ryoukin = line;
										continue;
									}

									if (line.EndsWith("無料"))
									{
										// 料金情報を含む。

										concert.ryoukin = "入場無料";
										continue;
									}

									if (line.EndsWith("全席自由"))
									{
										// 料金情報を含む。

										concert.ryoukin = line;
										continue;
									}

									if (line.StartsWith("入場料"))
									{
										// 料金情報を含む。

										if (line.EndsWith("未定"))
										{
											// 未定である。

											concert.ryoukin = "未定";
											continue;
										}
										else
										{
											// 料金情報を含む。

											concert.ryoukin =
												Regex.Replace(line, "入場料[:：]* *", string.Empty);
											continue;
										}
									}

									if (line.StartsWith("料金"))
									{
										// 料金情報を含む。

										concert.ryoukin =
											Regex.Replace(line, "料金[:：]* *", string.Empty);
										continue;
									}

									for (int j = 0; j < halls.Length; j++)
									{
										if (line.IndexOf(halls[j]) >= 0 ||
											line.IndexOf(halls[j].Replace("音楽ホール", "・音楽ホール")) >= 0 ||
											line.IndexOf(halls[j].Replace("ホール", "大ホール")) >= 0 ||
											line.IndexOf(halls[j].Replace("ー", "-")) >= 0 ||
											line.IndexOf(halls[j].Replace("こうとう", "江東")) >= 0 ||
											line.IndexOf(halls[j].Replace("市", string.Empty)) >= 0)
										{
											// ホール名を含む。

											concert.hall = halls[j];
											kakutei = true;
											break;
										}
									}

									if (kakutei)
									{
										// 行内容確定。

										continue;
									}

									if (Regex.IsMatch(line, ".*駅.*口"))
									{
										// 会場案内を含む。

										continue;
									}

									for (int j = 0; j < composerSurrogates.Length; j++)
									{
										if (Regex.IsMatch(line, composerSurrogates[j][0] + "[ 　]*[/:／：][ 　]*"))
										{
											// 作曲家：曲名の形式。

											line = Regex.Replace(line, composerSurrogates[j][0] + "[ 　]*[/:／：][ 　]*", string.Empty);
											line = Regex.Replace(line, "曲　*目：", string.Empty);

											concert.AddComposer(composerSurrogates[j][1]);
											concert.SetTitle(line);
											kakutei = true;
											break;
										}

										if (Regex.IsMatch(line, composerSurrogates[j][0] + "作曲「.*」"))
										{
											// 作曲家作曲「曲名」の形式。

											line = Regex.Replace(line, composerSurrogates[j][0] + "作曲「(.*)」", "$1");

											concert.AddComposer(composerSurrogates[j][1]);
											concert.SetTitle(line);
											kakutei = true;
											break;
										}
									}

									for (int j = 0; j < composers.Length; j++)
									{
										if (Regex.IsMatch(line, composers[j] + "[ \t]*[/:／：　][ \t]*"))
										{
											// 作曲家：曲名の形式。

											line = Regex.Replace(line, composers[j] + "[ \t]*[/:／：　][ \t]*", string.Empty);
											line = Regex.Replace(line, "曲　*目：", string.Empty);

											concert.AddComposer(composers[j]);
											concert.SetTitle(line);
											kakutei = true;
											break;
										}

										if (Regex.IsMatch(line, composers[j] + "  *.*"))
										{
											// 作曲家 曲名の形式。

											line = Regex.Replace(line, composers[j] + "  *(.*)", "$1");

											concert.AddComposer(composers[j]);
											concert.SetTitle(line);
											kakutei = true;
											break;
										}

										if (Regex.IsMatch(line, composers[j] + "作曲「.*」"))
										{
											// 作曲家作曲「曲名」の形式。

											line = Regex.Replace(line, composers[j] + "作曲「(.*)」", "$1");

											concert.AddComposer(composers[j]);
											concert.SetTitle(line);
											kakutei = true;
											break;
										}

										if (Regex.IsMatch(line, composers[j] + "作曲[ 　].*"))
										{
											// 作曲家作曲 曲名の形式。

											line = Regex.Replace(line, composers[j] + "作曲[ 　](.*)", "$1");

											concert.AddComposer(composers[j]);
											concert.SetTitle(line);
											kakutei = true;
											break;
										}

										if (Regex.IsMatch(line, composers[j] + "「.*」"))
										{
											// 作曲家「曲名」の形式。

											line = Regex.Replace(line, composers[j] + "「(.*)」", "$1");

											concert.AddComposer(composers[j]);
											concert.SetTitle(line);
											kakutei = true;
											break;
										}
									}

									if (kakutei)
									{
										// 行内容確定。

										continue;
									}

									for (int j = 0; j < composers.Length; j++)
									{
										if (Regex.IsMatch(line, ".* *[/／] *" + composers[j]))
										{
											// 曲名／作曲家の形式。

											line = Regex.Replace(line, " *[/／] *" + composers[j], string.Empty);
											line = Regex.Replace(line, "曲　*目[：　]*", string.Empty);

											concert.AddComposer(composers[j]);
											concert.SetTitle(line);
											kakutei = true;
											break;
										}

										if (Regex.IsMatch(line, ".*[(（]" + composers[j]))
										{
											// 曲名（作曲家）の形式。

											line = Regex.Replace(line, "[(（]" + composers[j], string.Empty);

											concert.AddComposer(composers[j]);
											concert.SetTitle(line);
											kakutei = true;
											break;
										}
									}

									if (kakutei)
									{
										// 行内容確定。

										continue;
									}

									for (int j = 0; j < composers.Length; j++)
									{
										if (line == composers[j] ||
											line == composers[j] + "作曲" ||
											line.IndexOf("オール" + composers[j] + "プログラム") >= 0)
										{
											// 作曲家名の行である。

											concert.AddComposer(composers[j]);
											kakutei = true;
											lineType = LineType.Composer;
											break;
										}
									}

									for (int j = 0; j < composerSurrogates.Length; j++)
									{
										if (line.EndsWith(composerSurrogates[j][0]) ||
											line.IndexOf("オール" + composerSurrogates[j][0] + "プログラム") >= 0)
										{
											// 作曲家名の行である。

											concert.AddComposer(composerSurrogates[j][1]);
											kakutei = true;
											lineType = LineType.Composer;
											break;
										}
									}

									if (kakutei)
									{
										// 行内容確定。

										continue;
									}

									for (int j = 0; j < playerNames.Length && !kakutei; j++)
									{
										if (line == playerNames[j] &&
											(line.StartsWith("Ensemble") ||
											line.EndsWith("楽団") ||
											line.StartsWith("オーケストラ") ||
											line.EndsWith("オーケストラ") ||
											line.EndsWith("シンフォニエッタ") ||
											line.EndsWith("Orchestra")))
										{
											// オーケストラ名の行と判断する。

											concert.AddPart("管弦楽");
											concert.SetPlayer(playerNames[j]);
											kakutei = true;
											break;
										}

										for (int k = 0; k < partNames.Length && !kakutei; k++)
										{
											if ((line.StartsWith(partNames[k]) ||
												line.StartsWith(partNames[k].Insert(1, "　"))) &&
												(line.IndexOf(playerNames[j]) >= 0 ||
												line.IndexOf(playerNames[j].Replace(' ', '　')) >= 0 ||
												line.IndexOf(playerNames[j].Replace(" ", string.Empty)) >= 0))
											{
												// パート名＋演奏者名の行である。

												concert.AddPart(partNames[k]);
												concert.SetPlayer(playerNames[j]);
												kakutei = true;
												break;
											}
										}
									}

									if (kakutei)
									{
										// 行内容確定。

										continue;
									}

									// 未登録の演奏者。
									for (int j = 0; j < partNames.Length; j++)
									{
										if (line.StartsWith("合唱指揮"))
										{
											// 合唱指揮情報である。

											break;
										}

										if (Regex.IsMatch(line, partNames[j] + "[ 　:：]") ||
											Regex.IsMatch(line, partNames[j] + "ソロ[ 　:：]") ||
											Regex.IsMatch(line, partNames[j] + "独奏[ 　:：]"))
										{
											// パート名から始まっている。

											string player =
												Regex.Replace(
													line,
													partNames[j] + "[^ 　\t:：]*[ 　\t:：]*",
													string.Empty);
											player = player.Replace("　", " ");

											concert.AddPart(partNames[j]);
											concert.SetPlayer(player);
											kakutei = true;
											break;
										}
									}

									if (kakutei)
									{
										// 行内容確定。

										continue;
									}
								}

								writer.WriteLine(line);
							}
						}
						catch (Exception exception)
						{
							throw new Exception(
								string.Format(
									"{0}\r\n{1}",
									exception.Message,
									line), exception);
						}
					}
					reader.Close();
					writer.Close();

					XmlText newText = CreateTextNode(
						Environment.NewLine +
						Environment.NewLine +
						writer.ToString() +
						Environment.NewLine +
						"\t");
					DocumentElement.ReplaceChild(newText, node);
				}
				else if (node.NodeType == XmlNodeType.Element)
				{
					// concert要素である。

					if (concert != null)
					{
						// テキストによるコンサート情報あり。

						XmlElement element = (XmlElement)node;
						XmlElement elementHall =
							(XmlElement)node.SelectSingleNode("hall");
						XmlElement elementKyokuCollection =
							(XmlElement)node.SelectSingleNode("kyokuCollection");
						XmlElement elementRyoukin =
							(XmlElement)node.SelectSingleNode("ryoukin");

						if (concert.name != null)
						{
							element.SetAttribute("name", concert.name);
						}

						element.SetAttribute("date", concert.Date);
						element.SetAttribute("kaijou", concert.Kaijou);
						element.SetAttribute("kaien", concert.Kaien);

						elementHall.SetAttribute("name", concert.hall);
						elementRyoukin.SetAttribute("value", concert.ryoukin);

						if (concert.composerNameAndTitles.Count > 0)
						{
							// 曲目情報あり。

							for (int j=elementKyokuCollection.ChildNodes.Count-1 ; j>=0 ; j--)
							{
								elementKyokuCollection.RemoveChild(elementKyokuCollection.ChildNodes[j]);
							}
						}

						for (int j=0 ; j<concert.composerNameAndTitles.Count ; j++)
						{
							XmlElement elementKyoku = CreateElement("kyoku");
							elementKyoku.SetAttribute
								("composer", concert.composerNameAndTitles[j].key);
							elementKyoku.SetAttribute
								("title", concert.composerNameAndTitles[j].value);
							elementKyokuCollection.AppendChild(elementKyoku);

							elementKyokuCollection.InsertBefore(
								CreateTextNode(Environment.NewLine + "\t\t\t"),
								elementKyoku);

							if (j >= concert.composerNameAndTitles.Count - 1)
							{
								// 最後の要素。

								elementKyokuCollection.InsertAfter(
									CreateTextNode(Environment.NewLine + "\t\t"),
									elementKyoku);
							}
						}

						XmlElement elementPlayerCollection =
							(XmlElement)node.SelectSingleNode("playerCollection");

						if (concert.partAndPlayers.Count > 0)
						{
							// 演奏者情報あり。

							for (int j=elementPlayerCollection.ChildNodes.Count-1 ; j>=0 ; j--)
							{
								elementPlayerCollection.RemoveChild
									(elementPlayerCollection.ChildNodes[j]);
							}
						}

						concert.SecureRequiredPart();
						concert.SortPlayerByPart(partNames);

						for (int j=0 ; j<concert.partAndPlayers.Count ; j++)
						{
							bool skip = false;

							for (int k=0 ; k<j && !skip ; k++)
							{
								if (concert.partAndPlayers[j].value == concert.partAndPlayers[k].value &&
									concert.partAndPlayers[j].key == concert.partAndPlayers[k].key)
								{
									// 既存の情報。

									skip = true;
								}
							}

							if (! skip)
							{
								// スキップしない。

								XmlElement elementPlayer = CreateElement("player");
								elementPlayer.SetAttribute
									("name", concert.partAndPlayers[j].value);
								elementPlayer.SetAttribute
									("part", concert.partAndPlayers[j].key);
								elementPlayerCollection.AppendChild(elementPlayer);

								elementPlayerCollection.InsertBefore(
									CreateTextNode(Environment.NewLine + "\t\t\t"),
									elementPlayer);

								if (j >= concert.partAndPlayers.Count - 1)
								{
									// 最後の要素。

									elementPlayerCollection.InsertAfter(
										CreateTextNode(Environment.NewLine + "\t\t"),
										elementPlayer);
								}
							}
						}

						concert = null;
					}
				}
			}
		}
	}
}
