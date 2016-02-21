using SQLite.Net.Attributes;

namespace ChGK.Core.Models.Database
{
    public interface IDatabaseModel
    {
        int Id { get; }
    }

    public abstract class DatabaseModel : IDatabaseModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
    }
}