using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace ConcertXmlTrim
{
	/// <summary>
	/// XML自動加工バックグラウンド処理。
	/// </summary>
	class TrimXmlWorker
		: BackgroundWorker
	{
		private readonly string sourceFile;
		private readonly string outputFile;
		private readonly string schemaFile;
		private readonly ToolStripProgressBar progressBar;

		/// <summary>
		/// 指定の値をメンバーに割り当て。
		/// </summary>
		/// <param name="sourceFile">入力ファイル</param>
		/// <param name="outputFile">出力ファイル</param>
		/// <param name="schemaFile">スキーマファイル</param>
		/// <param name="progressBar">プログレスバー</param>
		public TrimXmlWorker(string sourceFile, string outputFile,
			string schemaFile, ToolStripProgressBar progressBar)
		{
			base.WorkerReportsProgress = true;

			this.sourceFile = sourceFile;
			this.outputFile = outputFile;
			this.schemaFile = schemaFile;
			this.progressBar = progressBar;
		}

		/// <summary>
		/// 実処理。
		/// </summary>
		protected override void OnDoWork(DoWorkEventArgs e)
		{
			base.OnDoWork(e);

			ConcertSchemaDocument xsdDocument =
				new ConcertSchemaDocument(schemaFile);

			Stream stream = new FileStream(sourceFile, FileMode.Open);
			NewConcertDocument document = new NewConcertDocument(stream);
			stream.Close();

			string[] halls = xsdDocument.GetHalls();
			string[] composers = xsdDocument.GetComposerNames();
			string[] parts = xsdDocument.GetPartNames();
			string[] players = xsdDocument.GetPlayerNames();

			document.Trim(halls, composers, parts, players, this);

			document.Save(outputFile);
		}

		/// <summary>
		/// 進捗変化。
		/// </summary>
		protected override void OnProgressChanged(ProgressChangedEventArgs e)
		{
			base.OnProgressChanged(e);

			progressBar.Value = e.ProgressPercentage;
		}

		/// <summary>
		/// 処理終了時。
		/// </summary>
		protected override void OnRunWorkerCompleted
			(RunWorkerCompletedEventArgs e)
		{
			base.OnRunWorkerCompleted(e);

			progressBar.Value = 0;

			if (e.Error != null)
			{
				MessageBox.Show(e.Error.ToString());
			}
			else
			{
				MessageBox.Show("完了");
			}
		}
	}
}
