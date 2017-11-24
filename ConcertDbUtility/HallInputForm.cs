using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;

namespace ConcertDbUtility
{
	/// <summary>
	/// 
	/// </summary>
	public partial class HallInputForm
		: Form
	{
		private string hallAddress;

		/// <summary>
		/// 
		/// </summary>
		public string HallAddress
		{
			get { return hallAddress; }
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		public HallInputForm(string name)
		{
			InitializeComponent();

			label2.Text = "ホール：" + name;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void buttonOk_Click(object sender, EventArgs e)
		{
			hallAddress = textBox1.Text;
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