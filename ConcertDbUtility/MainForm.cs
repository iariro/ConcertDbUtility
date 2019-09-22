using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
            FileDialog dialog;
            dialog = new OpenFileDialog();
            dialog.Title = "コンサート情報ファイルを選択してください。";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                ConcertCollectionDocument document = new ConcertCollectionDocument();
                document.Load(dialog.FileName);
                List<Concert> concertCollection = document.GetConcertCollection();

                if (!CheckDuplicateConcert(concertCollection))
                {
                    // チェックNG
                    if (MessageBox.Show("重複がありますが続行しますか？") == DialogResult.No)
                    {
                        return;
                    }
                }

                List<string> emptyCheckErrors = CheckEmptyValue(concertCollection);

                if (emptyCheckErrors.Count > 0)
                {
                    // チェックNG

                    foreach (string error in emptyCheckErrors)
                    {
                        MessageBox.Show(error);
                    }
                    return;
                }

                InputConcertXml(concertCollection);
            }
        }

		/// <summary>
		/// 「検証」選択時
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
			GenerateSchemaFile();
		}

		/// <summary>
		/// 「重複コンサートチェック」選択時
		/// </summary>
		private void 重複コンサートチェックToolStripMenuItem_Click(object sender, EventArgs e)
		{
            FileDialog dialog = new OpenFileDialog();
            dialog.Title = "コンサート情報ファイルを選択してください。";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                ConcertCollectionDocument document = new ConcertCollectionDocument();
                document.Load(dialog.FileName);
                List<Concert> concertCollection;
                try
                {
                    concertCollection = document.GetConcertCollection();
                    CheckDuplicateConcert(concertCollection);
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.ToString());
                }
            }
        }

        /// <summary>
        /// 「終了」選択時
        /// </summary>
        private void 終了ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
