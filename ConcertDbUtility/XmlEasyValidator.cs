using System;
using System.Xml;
using System.Xml.Schema;

namespace ConcertDbUtility
{
	/// <summary>
	///
	/// </summary>
	class XmlEasyValidator
	{
		private readonly XmlSchemaSet schemas;
		private readonly XmlReaderSettings setting;
		private int count;
		private ValidationEventHandler validationEventHandler;

		/// <summary>
		///
		/// </summary>
		public int Count
		{
			get { return count; }
		}

		/// <summary>
		///
		/// </summary>
		public ValidationEventHandler ValidationEventHandler
		{
			set { validationEventHandler = value; }
			get { return validationEventHandler; }
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="document"></param>
		/// <param name="fileName"></param>
		public XmlEasyValidator(string document, string fileName)
		{
			count = 0;
			schemas = new XmlSchemaSet();
			schemas.Add(document, fileName);

			setting = new XmlReaderSettings();
			setting.ValidationType = ValidationType.Schema;
			setting.ValidationFlags |=
				XmlSchemaValidationFlags.ProcessInlineSchema;
			setting.ValidationFlags |=
				XmlSchemaValidationFlags.ReportValidationWarnings;
			setting.Schemas = schemas;
			setting.ValidationEventHandler +=
				delegate(object o, ValidationEventArgs args)
				{
					count++;

					validationEventHandler(o, args);
				};
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="fileName"></param>
		public void Run(string fileName)
		{
			XmlReader reader;

			reader = XmlReader.Create(fileName, setting);

			while(reader.Read())
			{
			}

			reader.Close();
		}
	}
}
