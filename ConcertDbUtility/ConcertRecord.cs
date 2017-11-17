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
        public DateTime createdate;

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
			DateTime kaien, int hallId, string ryoukin, DateTime createdate)
		{
			this.name = name;
			this.date = date;
			this.kaijou = kaijou;
			this.kaien = kaien;
			this.hallId = hallId;
			this.ryoukin = ryoukin;
            this.createdate = createdate;
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
            this.createdate = (DateTime)row["createdate"];
		}

		public override string ToString()
		{
			return
				string.Format(
                    "{0}Å^{1}Å^{2}Å^{3}Å^{4}Å^{5}Å^{6}",
					name,
					date,
					kaijou,
					kaien,
					hallId,
					ryoukin,
                    createdate);
		}
	}
}
