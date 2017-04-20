using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace QMGAzure
{
    public class TableStorage
    {
        private CloudTable _table;
        private CloudStorageAccount _storageAccount;
        private CloudTableClient _tableClient;

        public TableStorage(string tableReference, string connectionString)
        {
            _storageAccount = CloudStorageAccount.Parse(connectionString);
            _tableClient = _storageAccount.CreateCloudTableClient();
            _table = _tableClient.GetTableReference(tableReference);
            _table.CreateIfNotExists();
        }

        public void InsertObjectIntoTable(TableEntity obj)
        {
            var insertOperation = TableOperation.InsertOrReplace(obj);
            _table.Execute(insertOperation);
        }

        public void BatchInsertObjectIntoTable(params TableEntity[] objs)
        {
            var batchOperation = new TableBatchOperation();
            foreach(TableEntity ent in objs)
            {
                batchOperation.InsertOrReplace(ent);
            }
            _table.ExecuteBatch(batchOperation);
        }

        public IEnumerable<T> GetAllEntitiesInPartition<T>(string partition,EntityResolver<T> entityResolver)
        {
            TableQuery query = new TableQuery().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partition));
            IEnumerable<T> res = _table.ExecuteQuery(query,entityResolver);
            return res;
        }

        public TableEntity GetSingleEntity<T>(string partition, string key, EntityResolver<T> entityResolver)
        {
            var retrieveOperation = TableOperation.Retrieve<T>(partition, key, entityResolver);
            return  (TableEntity)_table.Execute(retrieveOperation).Result;
        }
    }
}

