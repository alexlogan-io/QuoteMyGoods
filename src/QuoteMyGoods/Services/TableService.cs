using QuoteMyGoods.Common;
using QuoteMyGoods.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuoteMyGoods.Services
{
    public interface ITableService
    {
        void SetUpTable();
        //IEnumerable<T> GetTables<T>(string partition, EntityResolver<T> entityResolver);
        void AddObjectToTable(object obj);
    }
    public class TableService: ITableService
    {
        //private TableStorage _tableStorage;

        public TableService()
        {
            //_tableStorage = new TableStorage("qmgusers",Startup.Configuration["TableStorage:ConnectionString"]);
        }

        public void AddObjectToTable(object obj)
        {
            //_tableStorage.InsertObjectIntoTable((TableEntity)obj);
        }

        public IEnumerable<T> GetTables<T>(string partition, EntityResolver<T> entityResolver)
        {
            // return _tableStorage.GetAllEntitiesInPartition(partition, entityResolver);
            return default(IEnumerable<T>);
        }

        public void SetUpTable()
        {
            /*
            var user = new UserEntity("logan", "alex");
            var user2 = new UserEntity("smith", "dan");
            var user3 = new UserEntity("smith", "jeff");
            _tableStorage.InsertObjectIntoTable(user);
            _tableStorage.BatchInsertObjectIntoTable(new TableEntity[] { user2, user3 });
            */
        }
    }

    public class EntityResolver<T>
    {
    }
}
