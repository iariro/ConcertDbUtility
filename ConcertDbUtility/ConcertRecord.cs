using System;
using System.Data;

namespace ConcertDbUtility
{
	/// <summary>
	///
	/// </summary>
	class ConcertRecord
	{
		public int id;
		public string name;
		public DateTime date;
		public DateTime kaijou;
		public DateTime kaien;
		public int hallId;
		public string ryoukin;

		/// <summary>
		///
		/// </summary>
		/// <param name="id"></param>
		/// <param name="name"></param>
		/// <param name="date"></param>
		/// <param name="kaijou"></param>
		/// <param name="kaien"></param>
		/// <param name="hallId"></param>
		/// <param name="ryoukin"></param>
		public ConcertRecord(string name, DateTime date, DateTime kaijou,
			DateTime kaien, int hallId, string ryoukin)
		{
			this.name = name;
			this.date = date;
			this.kaijou = kaijou;
			this.kaien = kaien;
			this.hallId = hallId;
			this.ryoukin = ryoukin;
		}

		public ConcertRecord(DataRow row)
		{
			this.id = (int)row["id"];
			this.name = (string)row["name"];
			this.date = (DateTime)row["date"];
			this.kaijou = (DateTime)row["kaijou"];
			this.kaien = (DateTime)row["kaien"];
			this.hallId = (int)row["hallId"];
			this.ryoukin = (string)row["ryoukin"];
		}

		public override string ToString()
		{
			return
				string.Format(
					"{0}�^{1}�^{2}�^{3}�^{4}�^{5}",
					name,
					date,
					kaijou,
					kaien,
					hallId,
					ryoukin);
		}
	}
}
