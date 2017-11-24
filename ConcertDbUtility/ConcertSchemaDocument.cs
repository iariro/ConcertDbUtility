
namespace ConcertDbUtility
{
	/// <summary>
	///
	/// </summary>
	class ConcertSchemaDocument
		: XmlSchemaDocument
	{
		/// <summary>
		/// 候補を列挙。
		/// </summary>
		/// <param name="dataset"></param>
		public void EnumerateAll(ConcertDataSet dataset)
		{
			Enumerate(
				new string[]
					{
						"schema",
						"simpleType[@name='hallName']",
						"restriction"
					},
				dataset.HallTable.Names);

			Enumerate(
				new string[]
					{
						"schema",
						"simpleType[@name='playerName']",
						"restriction"
					},
				dataset.PlayerTable.Names);

			Enumerate(
				new string[]
					{
						"schema",
						"simpleType[@name='partName']",
						"restriction"
					},
				dataset.PartTable.Names);

			Enumerate(
				new string[]
					{
						"schema",
						"simpleType[@name='composerName']",
						"restriction"
					},
				dataset.ComposerTable.Names);
		}
	}
}
