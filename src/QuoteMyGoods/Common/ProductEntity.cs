/*
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuoteMyGoods.Common
{
    public class ProductEntity: TableEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string ImgUrl { get; set; }
        public decimal Price { get; set; }

        public EntityResolver<ProductEntity> entityResolver = (pk, rk, ts, props, etag) =>
        {
            ProductEntity resolvedEntity = new ProductEntity();
            resolvedEntity.PartitionKey = pk;
            resolvedEntity.RowKey = rk;
            resolvedEntity.Timestamp = ts;
            resolvedEntity.ETag = etag;
            resolvedEntity.ReadEntity(props, null);

            return resolvedEntity;
        };

        public ProductEntity() { }

    }
}
*/
