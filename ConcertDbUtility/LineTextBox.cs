using System.Windows.Forms;

namespace ConcertDbUtility
{
	/// <summary>
	///
	/// </summary>
	class LineTextBox
		: TextBox
	{
		private const string kaigyou = "\r\n";

		/// <summary>
		///
		/// </summary>
		public void AddText(string value)
		{
			base.Text += value + kaigyou;
		}
	}
}
