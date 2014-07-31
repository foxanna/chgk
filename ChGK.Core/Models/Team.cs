using Cirrious.MvvmCross.Community.Plugins.Sqlite;

namespace ChGK.Core.Models
{
	public class Team
	{
		[PrimaryKey, AutoIncrement]
		public int ID { get; set; }

		public string Name { get; set; }
	}
}

