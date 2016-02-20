using System.Collections.Generic;
using ChGK.Core.Models;

namespace ChGK.Core.Services.Database
{
    public interface IDatabaseService
    {
        void CreateTable<T>() where T : class, IDatabaseModel;

        IEnumerable<T> GetAll<T>() where T : class, IDatabaseModel;
        void Insert<T>(T data) where T : class, IDatabaseModel;
        void Delete<T>(int id) where T : class, IDatabaseModel;
        void Delete<T>(T data) where T : class, IDatabaseModel;
        void DeleteAll<T>() where T : class, IDatabaseModel;
    }
}