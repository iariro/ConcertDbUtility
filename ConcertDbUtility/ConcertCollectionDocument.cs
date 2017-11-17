using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace ConcertDbUtility
{
	/// <summary>
	/// �R���T�[�g���XML�h�L�������g�B
	/// </summary>
	class ConcertCollectionDocument
		: XmlDocument
	{
		/// <summary>
		/// �R���T�[�g���R���N�V�������擾�B
		/// </summary>
		/// <returns>�R���T�[�g���R���N�V����</returns>
		public List<Concert> GetConcertCollection()
		{
			List<Concert> collections = new List<Concert>();

			for (int i=0 ; i<DocumentElement.ChildNodes.Count ; i++)
			{
				XmlElement element =
					DocumentElement.ChildNodes[i] as XmlElement;

				if (element != null)
				{
					// �v�f�ł������B

					try
					{
						collections.Add(new Concert(element));
					}
					catch (FormatException exception)
					{
						throw new Exception(
							string.Format(
								"{0}�ڂ̃R���T�[�g���ŃG���[\r\n{1}",
								i + 1,
								exception.ToString()));
					}
				}
			}

			return collections;
		}
	}
}
