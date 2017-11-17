using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace ConcertDbUtility
{
	/// <summary>
	///
	/// </summary>
	class ConcertDataSet
		: DataSet
	{
		private readonly SqlConnection connection;

		/// <summary>
		///
		/// </summary>
		public ConcertDataSet(SqlConnection connection)
		{
			this.connection = connection;
		}

		/// <summary>
		///
		/// </summary>
		public void Load()
		{
			HallTable.Load(this);
			PlayerTable.Load(this);
			OrchestraTable.Load(this);
			PartTable.Load(this);
			ComposerTable.Load(this);
			ConcertTable.Load(this);
			ShutsuenTable.Load(this);
			KyokumokuTable.Load(this);
		}

		/// <summary>
		///
		/// </summary>
		public ConcertTable ConcertTable
		{
			get { return new ConcertTable(Tables, connection); }
		}

		/// <summary>
		///
		/// </summary>
		public ShutsuenTable ShutsuenTable
		{
			get { return new ShutsuenTable(Tables, connection); }
		}

		/// <summary>
		/// 
		/// </summary>
		public KyokumokuTable KyokumokuTable
		{
			get { return new KyokumokuTable(Tables, connection); }
		}

		/// <summary>
		///
		/// </summary>
		public HallTable HallTable
		{
			get { return new HallTable(Tables, connection); }
		}

		/// <summary>
		///
		/// </summary>
		public PlayerTable PlayerTable
		{
			get { return new PlayerTable(Tables, connection); }
		}

		/// <summary>
		///
		/// </summary>
		public OrchestraTable OrchestraTable
		{
			get { return new OrchestraTable(Tables, connection); }
		}

		/// <summary>
		///
		/// </summary>
		public PartTable PartTable
		{
			get { return new PartTable(Tables, connection); }
		}

		/// <summary>
		///
		/// </summary>
		public ComposerTable ComposerTable
		{
			get { return new ComposerTable(Tables, connection); }
		}
	}
}
