using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ConcertDbUtility
{
	/// <summary>
	/// 
	/// </summary>
	public partial class PlayerInputForm
		: Form
	{
		private string siteurl;

		/// <summary>
		/// 
		/// </summary>
		public string SiteUrl
		{
			get { return siteurl; }
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="playerName"></param>
		public PlayerInputForm(string playerName)
		{
			InitializeComponent();

			labelPlayerName.Text = playerName;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void buttonOk_Click(object sender, EventArgs e)
		{
			siteurl = textBoxUrl.Text;
			DialogResult = DialogResult.OK;
			Close();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void buttonCancel_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			Close();
		}
	}
}