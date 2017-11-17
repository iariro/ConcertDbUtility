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
			this.URL検証ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.XMLにエクスポートToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.検証ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.終了ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.textBoxMessage = new ConcertDbUtility.LineTextBox();
			this.日時修正ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.menuStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// menuStrip
			// 
			this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.データベースToolStripMenuItem});
			this.menuStrip.Location = new System.Drawing.Point(0, 0);
			this.menuStrip.Name = "menuStrip";
			this.menuStrip.Size = new System.Drawing.Size(468, 24);
			this.menuStrip.TabIndex = 0;
			this.menuStrip.Text = "menuStrip1";
			// 
			// データベースToolStripMenuItem
			// 
			this.データベースToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.入力ToolStripMenuItem,
            this.スキーマ生成ToolStripMenuItem,
            this.URL検証ToolStripMenuItem,
            this.XMLにエクスポートToolStripMenuItem,
            this.toolStripSeparator2,
            this.検証ToolStripMenuItem,
            this.toolStripSeparator1,
            this.日時修正ToolStripMenuItem,
            this.toolStripSeparator3,
            this.終了ToolStripMenuItem});
			this.データベースToolStripMenuItem.Name = "データベースToolStripMenuItem";
			this.データベースToolStripMenuItem.Size = new System.Drawing.Size(74, 20);
			this.データベースToolStripMenuItem.Text = "データベース";
			// 
			// 入力ToolStripMenuItem
			// 
			this.入力ToolStripMenuItem.Name = "入力ToolStripMenuItem";
			this.入力ToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
			this.入力ToolStripMenuItem.Text = "入力";
			this.入力ToolStripMenuItem.Click += new System.EventHandler(this.入力ToolStripMenuItem_Click);
			// 
			// スキーマ生成ToolStripMenuItem
			// 
			this.スキーマ生成ToolStripMenuItem.Name = "スキーマ生成ToolStripMenuItem";
			this.スキーマ生成ToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
			this.スキーマ生成ToolStripMenuItem.Text = "スキーマ生成";
			this.スキーマ生成ToolStripMenuItem.Click += new System.EventHandler(this.スキーマ生成ToolStripMenuItem_Click);
			// 
			// URL検証ToolStripMenuItem
			// 
			this.URL検証ToolStripMenuItem.Name = "URL検証ToolStripMenuItem";
			this.URL検証ToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
			this.URL検証ToolStripMenuItem.Text = "URL検証";
			this.URL検証ToolStripMenuItem.Click += new System.EventHandler(this.URL検証ToolStripMenuItem_Click);
			// 
			// XMLにエクスポートToolStripMenuItem
			// 
			this.XMLにエクスポートToolStripMenuItem.Name = "XMLにエクスポートToolStripMenuItem";
			this.XMLにエクスポートToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
			this.XMLにエクスポートToolStripMenuItem.Text = "XMLにエクスポート";
			this.XMLにエクスポートToolStripMenuItem.Click += new System.EventHandler(this.XMLにエクスポートToolStripMenuItem_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(152, 6);
			// 
			// 検証ToolStripMenuItem
			// 
			this.検証ToolStripMenuItem.Name = "検証ToolStripMenuItem";
			this.検証ToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
			this.検証ToolStripMenuItem.Text = "検証";
			this.検証ToolStripMenuItem.Click += new System.EventHandler(this.検証ToolStripMenuItem_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(152, 6);
			// 
			// 終了ToolStripMenuItem
			// 
			this.終了ToolStripMenuItem.Name = "終了ToolStripMenuItem";
			this.終了ToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
			this.終了ToolStripMenuItem.Text = "終了";
			this.終了ToolStripMenuItem.Click += new System.EventHandler(this.終了ToolStripMenuItem_Click);
			// 
			// textBoxMessage
			// 
			this.textBoxMessage.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textBoxMessage.Location = new System.Drawing.Point(0, 24);
			this.textBoxMessage.Multiline = true;
			this.textBoxMessage.Name = "textBoxMessage";
			this.textBoxMessage.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.textBoxMessage.Size = new System.Drawing.Size(468, 267);
			this.textBoxMessage.TabIndex = 1;
			// 
			// 日時修正ToolStripMenuItem
			// 
			this.日時修正ToolStripMenuItem.Name = "日時修正ToolStripMenuItem";
			this.日時修正ToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
			this.日時修正ToolStripMenuItem.Text = "日時修正";
			this.日時修正ToolStripMenuItem.Click += new System.EventHandler(this.日時修正ToolStripMenuItem_Click);
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(152, 6);
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
		private System.Windows.Forms.ToolStripMenuItem URL検証ToolStripMenuItem;
		private LineTextBox textBoxMessage;
		private System.Windows.Forms.ToolStripMenuItem XMLにエクスポートToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripMenuItem 日時修正ToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
	}
}

