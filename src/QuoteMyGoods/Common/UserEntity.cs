/*
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuoteMyGoods.Common
{
    public class UserEntity: TableEntity
    {
        public string Email { get; set; }
        public string Phone { get; set; }
        public EntityResolver<UserEntity> entityResolver = (pk, rk, ts, props, etag) =>
        {
            UserEntity resolvedEntity = new UserEntity();
            resolvedEntity.PartitionKey = pk;
            resolvedEntity.RowKey = rk;
            resolvedEntity.Timestamp = ts;
            resolvedEntity.ETag = etag;
            resolvedEntity.ReadEntity(props, null);

            return resolvedEntity;
        };
        public UserEntity(string lastName, string firstName)
        {
            PartitionKey = lastName;
            RowKey = firstName;
        }

        public UserEntity() {
        }

    }
}
*/
