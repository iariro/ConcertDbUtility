
namespace ConcertXmlTrim
{
	/// <summary>
	/// キー文字列と値文字列の組。
	/// </summary>
	class KeyAndValue
	{
		public string key;
		public string value;

		/// <summary>
		/// 指定の値をメンバーに割り当てる。
		/// </summary>
		/// <param name="key">キー文字列</param>
		/// <param name="value">値文字列</param>
		public KeyAndValue(string key, string value)
		{
			this.key = key;
			this.value = value;
		}
	}
}
