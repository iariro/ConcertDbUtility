using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace ConcertDbUtility
{
	/// <summary>
	/// コンサート情報XMLドキュメント。
	/// </summary>
	class ConcertCollectionDocument
		: XmlDocument
	{
		/// <summary>
		/// コンサート情報コレクションを取得。
		/// </summary>
		/// <returns>コンサート情報コレクション</returns>
		public List<Concert> GetConcertCollection()
		{
			List<Concert> collections = new List<Concert>();

			for (int i=0 ; i<DocumentElement.ChildNodes.Count ; i++)
			{
				XmlElement element =
					DocumentElement.ChildNodes[i] as XmlElement;

				if (element != null)
				{
					// 要素であった。

					try
					{
						collections.Add(new Concert(element));
					}
					catch (FormatException exception)
					{
						throw new Exception(
							string.Format(
								"{0}個目のコンサート情報でエラー\r\n{1}",
								i + 1,
								exception.ToString()));
					}
				}
			}

			return collections;
		}
	}
}
