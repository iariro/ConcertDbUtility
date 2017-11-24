using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace ConcertDbUtility
{
	/// <summary>
	///
	/// </summary>
	abstract class TableBase
	{
		private readonly SqlDataAdapter adapter;
		protected readonly DataTable table;

		/// <summary>
		///
		/// </summary>
		public DataTable Table
		{
			get { return table; }
		}

		/// <summary>
		///
		/// </summary>
		public abstract string SelectString
		{
			get;
		}

		/// <summary>
		///
		/// </summary>
		public abstract string TableName
		{
			get;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="tables"></param>
		public TableBase(DataTableCollection tables, SqlConnection connection)
		{
			this.table = tables[TableName];
			adapter = new SqlDataAdapter(SelectString, connection);
			new SqlCommandBuilder(adapter);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="dataset"></param>
		public void Load(DataSet dataset)
		{
			if(table != null)
			{
				table.Clear();
			}
			adapter.Fill(dataset, TableName);
		}

		/// <summary>
		///
		/// </summary>
		public void Save()
		{
			adapter.Update(table);
		}

		/// <summary>
		///
		/// </summary>
		public string [] Names
		{
			get
			{
				List<string> ret;

				ret = new List<string>();
				foreach(DataRow row in table.Rows)
				{
					ret.Add(row[table.Columns["name"]].ToString());
				}

				return ret.ToArray();
			}
		}
	}

	/// <summary>
	///
	/// </summary>
	class ConcertTable
		: TableBase
	{
		/// <summary>
		///
		/// </summary>
		/// <param name="tables"></param>
		/// <param name="connection"></param>
		public ConcertTable
			(DataTableCollection tables, SqlConnection connection)
			: base(tables, connection)
		{
		}

		/// <summary>
		///
		/// </summary>
		public override string SelectString
		{
			get { return "select * from " + TableName; }
		}

		/// <summary>
		///
		/// </summary>
		public override string TableName
		{
			get { return "Concert"; }
		}

		public void SetKaijouKaien(int id, DateTime kaijou, DateTime kaien)
		{
			DataRow [] rows;

			rows = base.table.Select(string.Format("id='{0}'", id));
			if(rows.Length == 1)
			{
				rows[0]["kaijou"] = kaijou;
				rows[0]["kaien"] = kaien;
			}
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="concertRecord"></param>
		public void Add(ConcertRecord concertRecord)
		{
			DataRow row;

			row = table.NewRow();
			row["name"] = concertRecord.name;
			row["date"] = concertRecord.date;
			row["kaijou"] = concertRecord.kaijou;
			row["kaien"] = concertRecord.kaien;
			row["hallId"] = concertRecord.hallId;
			row["ryoukin"] = concertRecord.ryoukin;
            row["createdate"] = concertRecord.createdate;

			base.table.Rows.Add(row);
		}

		public ConcertRecord this[int id]
		{
			get
			{
				DataRow [] rows;
				ConcertRecord ret;

				rows = base.table.Select(string.Format("id='{0}'", id));

				if(rows.Length == 1)
				{
					ret = new ConcertRecord(rows[0]);
				}
				else
				{
					throw new ApplicationException();
				}

				return ret;
			}
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="date"></param>
		/// <param name="hallId"></param>
		/// <returns></returns>
		public int this [DateTime date, int hallId, DateTime kaijou]
		{
			get
			{
				int id;
				DataRow [] rows;

				rows =
					base.table.Select(
						string.Format(
							"date='{0}' and hallId={1} and kaijou='{2}'",
							date,
							hallId,
							kaijou));

				if(rows.Length == 1)
				{
					id = (int)rows[0]["id"];
				}
				else if(rows.Length <= 0)
				{
					id = -1;
				}
				else
				{
					string message = "データ重複：\n";

					foreach(DataRow row in rows)
					{
						message += string.Format("{0} {1} {2}\n", row["id"], row["date"], row["hallId"]);
					}

					throw new ApplicationException(message);
				}

				return id;
			}
		}
	}

	/// <summary>
	///
	/// </summary>
	class ShutsuenTable
		: TableBase
	{
		/// <summary>
		///
		/// </summary>
		/// <param name="tables"></param>
		/// <param name="connection"></param>
		public ShutsuenTable
			(DataTableCollection tables, SqlConnection connection)
			: base(tables, connection)
		{
		}

		/// <summary>
		///
		/// </summary>
		public override string SelectString
		{
			get { return "select * from " + TableName; }
		}

		/// <summary>
		///
		/// </summary>
		public override string TableName
		{
			get { return "Shutsuen"; }
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="record"></param>
		public void Add(ShutsuenRecord record)
		{
			DataRow row;

			row = table.NewRow();
			row["concertId"] = record.concertId;
			row["playerId"] = record.playerId;
			row["partId"] = record.partId;

			table.Rows.Add(row);
		}
	}

	/// <summary>
	///
	/// </summary>
	class KyokumokuTable
		: TableBase
	{
		/// <summary>
		///
		/// </summary>
		/// <param name="tables"></param>
		/// <param name="connection"></param>
		public KyokumokuTable
			(DataTableCollection tables, SqlConnection connection)
			: base(tables, connection)
		{
		}

		/// <summary>
		///
		/// </summary>
		public override string SelectString
		{
			get { return "select * from " + TableName; }
		}

		/// <summary>
		///
		/// </summary>
		public override string TableName
		{
			get { return "Kyokumoku"; }
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="record"></param>
		internal void Add(KyokumokuRecord record)
		{
			DataRow row;

			row = table.NewRow();
			row["concertId"] = record.concertId;
			row["composerId"] = record.composerId;
			row["title"] = record.title;

			table.Rows.Add(row);
		}
	}

	/// <summary>
	///
	/// </summary>
	class HallTable
		: TableBase
	{
		/// <summary>
		///
		/// </summary>
		public override string SelectString
		{
			get { return "select * from " + TableName; }
		}

		/// <summary>
		///
		/// </summary>
		public override string TableName
		{
			get { return "Hall"; }
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="tables"></param>
		/// <param name="connection"></param>
		public HallTable(DataTableCollection tables, SqlConnection connection)
			: base(tables, connection)
		{
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public int this[string name]
		{
			get
			{
				int id;
				DataRow [] rows;

				rows = base.table.Select(string.Format("name='{0}'", name));
				if(rows.Length == 1)
				{
					id = (int)rows[0]["id"];
				}
				else if(rows.Length <= 0)
				{
					id = -1;
				}
				else
				{
					throw new ApplicationException();
				}

				return id;
			}
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="hallRecord"></param>
		public void Add(HallRecord hallRecord)
		{
			DataRow row;

			row = table.NewRow();
			row["name"] = hallRecord.name;
			row["address"] = hallRecord.address;

			table.Rows.Add(row);
		}
	}

	/// <summary>
	///
	/// </summary>
	class PlayerTable
		: TableBase
	{
		/// <summary>
		///
		/// </summary>
		public override string SelectString
		{
			get { return "select * from " + TableName; }
		}

		/// <summary>
		///
		/// </summary>
		public override string TableName
		{
			get { return "Player"; }
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="tables"></param>
		/// <param name="connection"></param>
		public PlayerTable(DataTableCollection tables, SqlConnection connection)
			: base(tables, connection)
		{
		}

		/// <summary>
		///
		/// </summary>
		public Dictionary<string, string> Sites
		{
			get
			{
				Dictionary<string, string> ret;

				ret = new Dictionary<string, string>();
				foreach(DataRow row in table.Rows)
				{
					ret.Add(
						row[table.Columns["name"]].ToString(),
						row[table.Columns["siteurl"]].ToString());
				}

				return ret;
			}
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public int this[string name]
		{
			get
			{
				int id;
				DataRow [] rows;

				rows = base.table.Select(string.Format("name='{0}'", name));
				if(rows.Length == 1)
				{
					id = (int)rows[0]["id"];
				}
				else if(rows.Length <= 0)
				{
					id = -1;
				}
				else
				{
					throw new ApplicationException();
				}

				return id;
			}
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="record"></param>
		public void Add(PlayerRecord record)
		{
			DataRow row;

			row = table.NewRow();
			row["name"] = record.name;
			row["siteurl"] = record.siteurl;
			row["active"] = record.active;

			base.table.Rows.Add(row);
		}
	}

	/// <summary>
	///
	/// </summary>
	class OrchestraTable
		: PlayerTable
	{
		/// <summary>
		///
		/// </summary>
		public override string SelectString
		{
			get
			{
				return
					"select * from Player" +
					" where id in" +
					" (" +
					" select playerId" +
					" from Shutsuen" +
					" where partId=1" +
					" )";
			}
		}

		/// <summary>
		///
		/// </summary>
		public override string TableName
		{
			get { return "Orchestra"; }
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="tables"></param>
		public OrchestraTable
			(DataTableCollection tables, SqlConnection connection)
			: base(tables, connection)
		{
		}
	}

	/// <summary>
	///
	/// </summary>
	class PartTable
		: TableBase
	{
		/// <summary>
		///
		/// </summary>
		public override string SelectString
		{
			get { return "select * from " + TableName; }
		}

		/// <summary>
		///
		/// </summary>
		public override string TableName
		{
			get { return "Part"; }
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="tables"></param>
		/// <param name="connection"></param>
		public PartTable(DataTableCollection tables, SqlConnection connection)
			: base(tables, connection)
		{
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public int this[string name]
		{
			get
			{
				int id;
				DataRow [] rows;

				rows = base.table.Select(string.Format("name='{0}'", name));
				if(rows.Length == 1)
				{
					id = (int)rows[0]["id"];
				}
				else if(rows.Length <= 0)
				{
					id = -1;
				}
				else
				{
					throw new ApplicationException();
				}

				return id;
			}
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="record"></param>
		public void Add(PartRecord record)
		{
			DataRow row;

			row = table.NewRow();
			row["name"] = record.name;

			table.Rows.Add(row);
		}
	}

	/// <summary>
	///
	/// </summary>
	class ComposerTable
		: TableBase
	{
		/// <summary>
		///
		/// </summary>
		public override string SelectString
		{
			get { return "select * from " + TableName; }
		}

		/// <summary>
		///
		/// </summary>
		public override string TableName
		{
			get { return "Composer"; }
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="tables"></param>
		/// <param name="connection"></param>
		public ComposerTable
			(DataTableCollection tables, SqlConnection connection)
			: base(tables, connection)
		{
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="record"></param>
		public void Add(ComposerRecord record)
		{
			DataRow row;

			row = table.NewRow();
			row["name"] = record.name;

			table.Rows.Add(row);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public int this[string name]
		{
			get
			{
				int id;
				DataRow [] rows;

				rows = base.table.Select(string.Format("name='{0}'", name));
				if(rows.Length == 1)
				{
					id = (int)rows[0]["id"];
				}
				else if(rows.Length <= 0)
				{
					id = -1;
				}
				else
				{
					throw new ApplicationException();
				}

				return id;
			}
		}
	}
}
