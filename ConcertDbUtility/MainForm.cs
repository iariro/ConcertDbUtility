using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Schema;

namespace ConcertDbUtility
{
	/// <summary>
	/// コンサート情報DB登録ツールメイン画面。
	/// </summary>
	public partial class MainForm
		: Form
	{
		/// <summary>
		/// コンストラクタ。
		/// </summary>
		public MainForm()
		{
			InitializeComponent();
		}

		private string schemaBaseFilePath;
		private string schemaFilePath;

		/// <summary>
		/// 画面ロード時。設定の読み込み。
		/// </summary>
        private void MainForm_Load(object sender, EventArgs e)
        {
            schemaBaseFilePath = Properties.Settings.Default.SchemaBaseFilePath;
            schemaFilePath = Properties.Settings.Default.SchemaFilePath;
        }

		/// <summary>
		/// 画面クローズ時。設定の書き込み。
		/// </summary>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.SchemaBaseFilePath = schemaBaseFilePath;
            Properties.Settings.Default.SchemaFilePath = schemaFilePath;
            Properties.Settings.Default.Save();
        }

		/// <summary>
		/// 「入力」メニュー選択時。入力処理。
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void 入力ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			int i;
			int concertId, hallId, partId, playerId, composerId;
			bool insert;
			DateTime createdate = DateTime.Now;
			FileDialog dialog;
			DialogResult dialogResult;
			ConcertRecord record;
			SqlConnection connection;
			List<Concert> concertCollection;
			HallInputForm hallInputForm;
			ConcertDataSet dataset;
			PlayerInputForm playerInformationInputForm;
			ConcertCollectionDocument document;

			dialog = new OpenFileDialog();
			dialog.Title = "コンサート情報ファイルを選択してください。";
			if(dialog.ShowDialog() == DialogResult.OK)
			{
				document = new ConcertCollectionDocument();
				document.Load(dialog.FileName);
				concertCollection = document.GetConcertCollection();

				connection =
					new SqlConnection(Properties.Resources.connectionString);
				dataset = new ConcertDataSet(connection);
				dataset.Load();

				foreach(Concert concert in concertCollection)
				{
					insert = true;

					// ホール情報検索。
					hallId = dataset.HallTable[concert.hallName];

					if(hallId < 0)
					{
						// ホール情報がない。

						hallInputForm = new HallInputForm(concert.hallName);

						if(hallInputForm.ShowDialog() == DialogResult.OK)
						{
							// ホール情報を入力した。

							dataset.HallTable.Add(
								new HallRecord(
									concert.hallName,
									hallInputForm.HallAddress));

							dataset.HallTable.Save();
							dataset.HallTable.Load(dataset);

							hallId = dataset.HallTable [concert.hallName];
						}
						else
						{
							insert = false;
						}
					}

					if(insert)
					{
						if (dataset.ConcertTable[concert.date, hallId, concert.kaijou] >= 0)
						{
							textBoxMessage.Text +=
								string.Format("スキップしました {0} {1} {2}", concert.hallName, concert.date, concert.kaijou);
							continue;
						}

						record =
							new ConcertRecord(
								concert.name,
								concert.date,
								concert.kaijou,
								concert.kaien,
								hallId,
								concert.ryoukin,
								createdate);

						dataset.ConcertTable.Add(record);
						dataset.ConcertTable.Save();
						dataset.ConcertTable.Load(dataset);

						concertId =
							dataset.ConcertTable
								[concert.date, hallId, concert.kaijou];

						// 演奏者入力。
						for(i = 0 ;
							i < concert.playerCollection.Count && insert ; i++)
						{
							playerId =
								dataset.PlayerTable [
									concert.playerCollection [i].playerName];

							if(playerId < 0)
							{
								// 演奏者がいない。

								playerInformationInputForm =
									new PlayerInputForm
										(concert.playerCollection [i].
											playerName);

								if(playerInformationInputForm.ShowDialog() ==
									DialogResult.OK)
								{
									dataset.PlayerTable.Add(
										new PlayerRecord(
											concert.playerCollection [i].
												playerName,
											playerInformationInputForm.
												SiteUrl,
												true));

									dataset.PlayerTable.Save();
									dataset.PlayerTable.Load(dataset);

									playerId = dataset.PlayerTable[
										concert.playerCollection[i].playerName];
								}
								else
								{
									insert = false;
								}
							}

							// パート。
							partId =
								dataset.PartTable[
									concert.playerCollection[i].partName];

							if(partId < 0)
							{
								// パートがない。

								dialogResult =
									MessageBox.Show(
										concert.playerCollection[i].partName +
										"を登録しますか？",
										"caption",
										MessageBoxButtons.OKCancel);

								if(dialogResult == DialogResult.OK)
								{
									dataset.PartTable.Add(
										new PartRecord(
											concert.playerCollection[i].
												partName));

									dataset.PartTable.Save();
									dataset.PartTable.Load(dataset);

									partId =
										dataset.PartTable[
											concert.playerCollection[i].
												partName];
								}
								else
								{
									insert = false;
								}
							}

							if(insert)
							{
								dataset.ShutsuenTable.Add(
									new ShutsuenRecord
										(concertId, playerId, partId));
							}
						}

						// 曲入力。
						for(i = 0 ; i < concert.kyokuCollection.Count ; i++)
						{
							composerId = dataset.ComposerTable [
								concert.kyokuCollection [i].composerName];

							if(composerId < 0)
							{
								// 作曲家がいない。

								dialogResult =
									MessageBox.Show(
										concert.kyokuCollection[i].composerName+
										"を登録しますか？",
										"caption",
										MessageBoxButtons.OKCancel);

								if(dialogResult == DialogResult.OK)
								{
									dataset.ComposerTable.Add(
										new ComposerRecord(
											concert.kyokuCollection [i].
												composerName));

									dataset.ComposerTable.Save();
									dataset.ComposerTable.Load(dataset);

									composerId =
										dataset.ComposerTable [
											concert.kyokuCollection [i].
												composerName];
								}
								else
								{
									insert = false;
								}
							}

							if(insert)
							{
								dataset.KyokumokuTable.Add(
									new KyokumokuRecord(
										concertId,
										composerId,
										concert.kyokuCollection [i].title));
							}
						}

						textBoxMessage.Text +=
							string.Format(
								"{0} {1}を登録しました。\r\n",
								concert.date.ToShortDateString(),
								concert.name);
					}
					else
					{
						textBoxMessage.Text +=
							concert.name + "をスキップしました。\n";
					}
				}

				dataset.HallTable.Save();
				dataset.PlayerTable.Save();
				dataset.PartTable.Save();
				dataset.ComposerTable.Save();
				dataset.ShutsuenTable.Save();
				dataset.KyokumokuTable.Save();

				connection.Close();

				GenerateSchemaFile(schemaBaseFilePath, schemaFilePath);
			}
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void 検証ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			XmlEasyValidator validator;
			OpenFileDialog dialog1, dialog2;

			dialog1 = new OpenFileDialog();
			dialog1.Title = "コンサート情報ファイルを選択してください。";
			if(dialog1.ShowDialog() == DialogResult.OK)
			{
				// 処理対象の XML 選択について「OK」が押された。

				dialog2 = new OpenFileDialog();
				dialog2.Title = "スキーマファイルを選択してください。";
				if(dialog2.ShowDialog() == DialogResult.OK)
				{
					// XML Schema 選択について「OK」が押された。

					validator =
						new XmlEasyValidator("concert", dialog2.FileName);

					validator.ValidationEventHandler +=
						delegate(object o, ValidationEventArgs args)
						{
							textBoxMessage.AddText(
								args.Exception.LineNumber + " " + args.Message);
						};

					textBoxMessage.Text = string.Empty;
					try
					{
						validator.Run(dialog1.FileName);
						textBoxMessage.AddText(validator.Count + "件");
					}
					catch(XmlException exception)
					{
						textBoxMessage.AddText(exception.Message);
					}
				}
			}
		}

		/// <summary>
		/// 「スキーマ生成」メニュー選択時。
		/// </summary>
		private void スキーマ生成ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FileDialog loadDialog = new OpenFileDialog();
			loadDialog.Title = "スキーマのもとファイルを選択してください。";
			if(loadDialog.ShowDialog() == DialogResult.OK)
			{
				// 読み込むファイル選択について「OK」が押された。

				FileDialog saveDialog = new SaveFileDialog();
				saveDialog.Title = "生成するファイルを選択してください。";
				if(saveDialog.ShowDialog() == DialogResult.OK)
				{
					// 保存するファイル選択について「OK」が押された。

					GenerateSchemaFile(loadDialog.FileName, saveDialog.FileName);
				}
			}
		}

		/// <summary>
		/// スキーマ生成実処理。
		/// </summary>
		/// <param name="schemaBaseFilePath">スキーマ元ファイルパス</param>
		/// <param name="schemaFilePath">スキーマファイルパス</param>
		private void GenerateSchemaFile(string schemaBaseFilePath, string schemaFilePath)
		{
			// スキーマのベース読み込み。
			ConcertSchemaDocument document = new ConcertSchemaDocument();
			document.Load(schemaBaseFilePath);

			SqlConnection connection =
				new SqlConnection(Properties.Resources.connectionString);
			ConcertDataSet dataset = new ConcertDataSet(connection);
			dataset.Load();

			// 候補の列挙。
			document.EnumerateAll(dataset);

			document.Save(schemaFilePath);
			textBoxMessage.AddText(saveDialog.FileName + "生成しました。");

			connection.Close();
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void URL検証ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			string fileUrl;
			SqlConnection connection;
			OpenFileDialog dialog;
			ConcertDataSet dataset;
			Dictionary<string, string> sitesFile;

			dialog = new OpenFileDialog();
			if(dialog.ShowDialog() == DialogResult.OK)
			{
				// 「OK」が押された。

				try
				{
					sitesFile = new UrlDocument(dialog.FileName).Sites;

					// 接続。
					connection =
						new SqlConnection(Properties.Resources.connectionString);
					dataset = new ConcertDataSet(connection);
					dataset.Load();

					textBoxMessage.Text = string.Empty;
					foreach(KeyValuePair<string, string> pair in
						dataset.OrchestraTable.Sites)
					{
						sitesFile.TryGetValue(pair.Key, out fileUrl);
						if(fileUrl != null)
						{
							// サイト名あり。

							if(pair.Value != fileUrl)
							{
								// URLが異なる。

								textBoxMessage.AddText(pair.Key + "URL違い");
							}
						}
						else
						{
							// サイト名なし。

							textBoxMessage.AddText(pair.Key + "なし");
						}
					}
				}
				catch(DuplicateSiteException exception)
				{
					textBoxMessage.AddText(exception.ToString());
				}
			}
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void XMLにエクスポートToolStripMenuItem_Click
			(object sender, EventArgs e)
		{
			FileDialog saveDialog;
			SqlConnection connection;
			ConcertDataSet dataset;

			connection =
				new SqlConnection(Properties.Resources.connectionString);
			dataset = new ConcertDataSet(connection);
			dataset.Load();

			saveDialog = new SaveFileDialog();
			if(saveDialog.ShowDialog() == DialogResult.OK)
			{
				// 保存するファイル選択について「OK」が押された。

				dataset.WriteXml(saveDialog.FileName);
			}

			connection.Close();
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void 終了ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void 日時修正ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SqlConnection connection =
				new SqlConnection(Properties.Resources.connectionString);
			ConcertDataSet dataset = new ConcertDataSet(connection);
			dataset.ConcertTable.Load(dataset);

			foreach(DataRow row in dataset.ConcertTable.Table.Rows)
			{
				ConcertRecord concert = new ConcertRecord(row);

				dataset.ConcertTable.SetKaijouKaien(
					concert.id,
					new DateTime(
						concert.date.Year,
						concert.date.Month,
						concert.date.Day,
						concert.kaijou.Hour,
						concert.kaijou.Minute,
						concert.kaijou.Second),
					new DateTime(
						concert.date.Year,
						concert.date.Month,
						concert.date.Day,
						concert.kaien.Hour,
						concert.kaien.Minute,
						concert.kaien.Second));
			}

			dataset.ConcertTable.Save();

			connection.Close();
		}

		/// <summary>
		/// 重複コンサートチェック。
		/// </summary>
		private void 重複コンサートチェックToolStripMenuItem_Click
			(object sender, EventArgs e)
		{
			int count = 0;
			int concertId, hallId;
			SqlConnection connection;
			List<Concert> concertCollection;
			ConcertDataSet dataset;
			ConcertCollectionDocument document;

			FileDialog dialog = new OpenFileDialog();
			dialog.Title = "コンサート情報ファイルを選択してください。";

			if (dialog.ShowDialog() == DialogResult.OK)
			{
				document = new ConcertCollectionDocument();
				document.Load(dialog.FileName);

				try
				{
					concertCollection = document.GetConcertCollection();
				}
				catch (Exception exception)
				{
					MessageBox.Show(exception.ToString());
					return;
				}

				connection =
					new SqlConnection(Properties.Resources.connectionString);
				dataset = new ConcertDataSet(connection);
				dataset.Load();

				foreach(Concert concert in concertCollection)
				{
					// ホール情報検索。
					hallId = dataset.HallTable[concert.hallName];

					if(hallId >= 0)
					{
						// 登録済みのホール。

						concertId =
							dataset.ConcertTable
								[concert.date, hallId, concert.kaijou];

						if(concertId >= 0)
						{
							// すでに存在するコンサート情報。

							string message =
								string.Format(
									"{0} {1} {2}",
									concertId,
									concert.date,
									concert.hallName);

							textBoxMessage.AddText(message);

							count++;
						}
					}
				}

				if(count <= 0)
				{
					// エラーなし。

					textBoxMessage.AddText("エラーなし");
				}

				textBoxMessage.AddText(string.Empty);

				connection.Close();
			}
		}
	}
}
