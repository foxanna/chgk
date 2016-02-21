using System.Collections.Generic;
using ChGK.Core.Models.Database;
using SQLite.Net;

namespace ChGK.Core.Services.Database
{
    internal class DatabaseService : IDatabaseService
    {
        private readonly SQLiteConnection _connection;

        public DatabaseService(SQLiteConnection connection)
        {
            _connection = connection;
        }

        public void CreateTable<T>() where T : class, IDatabaseModel
        {
            _connection.CreateTable<T>();
        }

        public IEnumerable<T> GetAll<T>() where T : class, IDatabaseModel
        {
            return _connection.Table<T>();
        }

        public void Insert<T>(T data) where T : class, IDatabaseModel
        {
            _connection.Insert(data);
        }

        public void Delete<T>(int id) where T : class, IDatabaseModel
        {
            _connection.Delete<T>(id);
        }

        public void Delete<T>(T data) where T : class, IDatabaseModel
        {
            _connection.Delete(data);
        }

        public void DeleteAll<T>() where T : class, IDatabaseModel
        {
            _connection.DeleteAll<T>();
        }
    }
}