using System;
using System.Collections.Generic;
using System.Xml;

namespace ConcertDbUtility
{
	/// <summary>
	/// 
	/// </summary>
	class DuplicateSiteException
		: ApplicationException
	{
		private readonly string name;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		public DuplicateSiteException(string name)
		{
			this.name = name;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return string.Format("�u{0}�v�̃T�C�g��񂪏d�����Ă��܂��B", name);
		}
	}

	/// <summary>
	///
	/// </summary>
	class UrlDocument
	{
		private readonly string fileName;

		/// <summary>
		///
		/// </summary>
		/// <param name="fileName"></param>
		public UrlDocument(string fileName)
		{
			this.fileName = fileName;
		}

		/// <summary>
		///
		/// </summary>
		public Dictionary<string, string> Sites
		{
			get
			{
				string site, url;
				XmlReader reader;
				Dictionary<string, string> ret;

				ret = new Dictionary<string, string>();

				reader = XmlReader.Create(fileName);
				while(reader.Read())
				{
					if(reader.NodeType == XmlNodeType.Element)
					{
						// �v�f�ł���B

						if(reader.Name == "A")
						{
							// �A���J�[�^�O�B

							reader.MoveToAttribute("HREF");
							url = reader.Value;

							reader.MoveToContent();
							site = reader.ReadElementContentAsString();

							try
							{
								ret.Add(site, url);
							}
							catch(ArgumentException)
							{
								throw new DuplicateSiteException(site);
							}
						}
					}
				}

				return ret;
			}
		}
	}
}
