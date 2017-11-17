
namespace ConcertDbUtility
{
	/// <summary>
	/// 
	/// </summary>
	class ShutsuenRecord
	{
		public readonly int concertId;
		public readonly int playerId;
		public readonly int partId;

		/// <summary>
		///
		/// </summary>
		/// <param name="concertId"></param>
		/// <param name="playerId"></param>
		/// <param name="partId"></param>
		public ShutsuenRecord(int concertId, int playerId, int partId)
		{
			this.concertId = concertId;
			this.playerId = playerId;
			this.partId = partId;
		}
	}
}
