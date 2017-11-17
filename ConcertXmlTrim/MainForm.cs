using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace ConcertXmlTrim
{
	/// <summary>
	/// メイン画面。
	/// </summary>
	public partial class MainForm
		: Form
	{
		/// <summary>
		/// 画面構築。
		/// </summary>
		public MainForm()
		{
			InitializeComponent();
		}

		/// <summary>
		/// 「加工」メニュー選択時。
		/// </summary>
		private void 加工ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			加工1();
		}

		/// <summary>
		/// ファイル選択ダイアログでのファイル選択による実行。
		/// </summary>
		private void 加工1()
		{
			OpenFileDialog dialog = new OpenFileDialog();

			dialog.Title = "NewConcert.xmlを指定";

			if (dialog.ShowDialog() == DialogResult.OK)
			{
				// OK

				OpenFileDialog dialog2 = new OpenFileDialog();

				dialog2.Title = "スキーマファイルを指定";

				if (dialog2.ShowDialog() == DialogResult.OK)
				{
					// OK

					SaveFileDialog dialog3 = new SaveFileDialog();

					dialog3.Title = "出力ファイルを指定";

					if (dialog3.ShowDialog() == DialogResult.OK)
					{
						// OK

						BackgroundWorker worker =
							new TrimXmlWorker(
								dialog.FileName,
								dialog3.FileName,
								dialog2.FileName,
								toolStripProgressBar1);

						worker.RunWorkerAsync();
					}
				}
			}
		}

		/// <summary>
		/// ファイルパス決め打ち実行。
		/// </summary>
		private void 加工2()
		{
			string basePath =
				Path.Combine(
					Environment.GetFolderPath
						(Environment.SpecialFolder.MyDocuments),
					@"Nefertiti の文書\Private\m\c2\");

			string sourceFile = basePath + "NewConcert_.xml";
			string outputFile = basePath + "NewConcert__.xml";
			string schemaFile = basePath + "ConcertSchema.xsd";

			BackgroundWorker worker =
				new TrimXmlWorker
					(sourceFile, outputFile, schemaFile, toolStripProgressBar1);

			worker.RunWorkerAsync();
		}
	}
}
