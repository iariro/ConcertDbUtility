namespace ConcertDbUtility
{
	partial class MainForm
	{
		/// <summary>
		/// 必要なデザイナ変数です。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 使用中のリソースをすべてクリーンアップします。
		/// </summary>
		/// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
		protected override void Dispose(bool disposing)
		{
			if(disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows フォーム デザイナで生成されたコード

		/// <summary>
		/// デザイナ サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディタで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.データベースToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.入力ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.スキーマ生成ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.検証ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.重複コンサートチェックToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.終了ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.textBoxMessage = new ConcertDbUtility.LineTextBox();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.データベースToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(468, 26);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "menuStrip1";
            // 
            // データベースToolStripMenuItem
            // 
            this.データベースToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.入力ToolStripMenuItem,
            this.スキーマ生成ToolStripMenuItem,
            this.toolStripSeparator2,
            this.検証ToolStripMenuItem,
            this.重複コンサートチェックToolStripMenuItem,
            this.toolStripSeparator1,
            this.終了ToolStripMenuItem});
            this.データベースToolStripMenuItem.Name = "データベースToolStripMenuItem";
            this.データベースToolStripMenuItem.Size = new System.Drawing.Size(111, 22);
            this.データベースToolStripMenuItem.Text = "データベース(&D)";
            // 
            // 入力ToolStripMenuItem
            // 
            this.入力ToolStripMenuItem.Name = "入力ToolStripMenuItem";
            this.入力ToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.I)));
            this.入力ToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.入力ToolStripMenuItem.Text = "入力";
            this.入力ToolStripMenuItem.Click += new System.EventHandler(this.入力ToolStripMenuItem_Click);
            // 
            // スキーマ生成ToolStripMenuItem
            // 
            this.スキーマ生成ToolStripMenuItem.Name = "スキーマ生成ToolStripMenuItem";
            this.スキーマ生成ToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.スキーマ生成ToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.スキーマ生成ToolStripMenuItem.Text = "スキーマ生成";
            this.スキーマ生成ToolStripMenuItem.Click += new System.EventHandler(this.スキーマ生成ToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(205, 6);
            // 
            // 検証ToolStripMenuItem
            // 
            this.検証ToolStripMenuItem.Name = "検証ToolStripMenuItem";
            this.検証ToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.検証ToolStripMenuItem.Size = new System.Drawing.Size(256, 22);
            this.検証ToolStripMenuItem.Text = "検証";
            this.検証ToolStripMenuItem.Click += new System.EventHandler(this.検証ToolStripMenuItem_Click);
            // 
            // 重複コンサートチェックToolStripMenuItem
            // 
            this.重複コンサートチェックToolStripMenuItem.Name = "重複コンサートチェックToolStripMenuItem";
            this.重複コンサートチェックToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
            this.重複コンサートチェックToolStripMenuItem.Size = new System.Drawing.Size(256, 22);
            this.重複コンサートチェックToolStripMenuItem.Text = "重複コンサートチェック";
            this.重複コンサートチェックToolStripMenuItem.Click += new System.EventHandler(this.重複コンサートチェックToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(205, 6);
            // 
            // 終了ToolStripMenuItem
            // 
            this.終了ToolStripMenuItem.Name = "終了ToolStripMenuItem";
            this.終了ToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.終了ToolStripMenuItem.Size = new System.Drawing.Size(256, 22);
            this.終了ToolStripMenuItem.Text = "終了";
            this.終了ToolStripMenuItem.Click += new System.EventHandler(this.終了ToolStripMenuItem_Click);
            // 
            // textBoxMessage
            // 
            this.textBoxMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxMessage.Location = new System.Drawing.Point(0, 26);
            this.textBoxMessage.Multiline = true;
            this.textBoxMessage.Name = "textBoxMessage";
            this.textBoxMessage.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxMessage.Size = new System.Drawing.Size(468, 265);
            this.textBoxMessage.TabIndex = 1;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(468, 291);
            this.Controls.Add(this.textBoxMessage);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.Name = "MainForm";
            this.Text = "コンサートDBユーティリティ";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.MenuStrip menuStrip;
		private System.Windows.Forms.ToolStripMenuItem データベースToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 入力ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 検証ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem スキーマ生成ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 終了ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private LineTextBox textBoxMessage;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripMenuItem 重複コンサートチェックToolStripMenuItem;
	}
}

