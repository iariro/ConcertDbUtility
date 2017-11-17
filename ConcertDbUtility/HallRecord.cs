
namespace ConcertDbUtility
{
	/// <summary>
	/// 
	/// </summary>
	class HallRecord
	{
		public readonly string name;
		public readonly string address;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <param name="address"></param>
		public HallRecord(string name, string address)
		{
			this.name = name;
			this.address = address;
		}
	}
}
