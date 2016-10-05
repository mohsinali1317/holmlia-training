using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.ApplicationServer.Caching;
using Crondale.AzureWrapper.Caching;
using System.Diagnostics;

namespace Crondale.AzureWrapper.Storage
{
    public static class TableHelper
    {
        public static CloudTable GetTable(String tableName)
        {
            CloudStorageAccount cloudStorageAccount = StorageHelper.GetAccount();
            CloudTableClient cloudTableClient = cloudStorageAccount.CreateCloudTableClient();
            CloudTable table = cloudTableClient.GetTableReference(tableName);
            table.CreateIfNotExists();

            return table;
        }
        
        public static CloudTable GetTable<T>() where T : EntityModel
        {
            return GetTable(GetTableName<T>());
        }

        public static String GetTableName<T>() where T : EntityModel
        {
            EntityTableAttribute attr = (Attribute.GetCustomAttributes(typeof(T), typeof(EntityTableAttribute)).FirstOrDefault() as EntityTableAttribute);

            return attr == null ? typeof(T).Name.ToLower() : attr.TableName;
        }

        public static void Delete<T>(T entity) where T : EntityModel
        {
            String tableName = GetTableName<T>();
            TableHelper.GetTable(tableName).Execute(TableOperation.Delete(entity));

            String cacheKey = String.Format("{0}_{1}_{2}", tableName, entity.PartitionKey, entity.RowKey);

            Cache.Get("objects").Remove(cacheKey);

        }

        public static void Save<T>(T entity) where T : EntityModel
        {
            String tableName = GetTableName<T>();
            String cacheKey = String.Format("{0}_{1}_{2}", tableName, entity.PartitionKey, entity.RowKey);

            CloudTable table = TableHelper.GetTable(tableName);
            table.Execute(TableOperation.InsertOrReplace(entity));

            Cache.Get("objects").Put(cacheKey, entity);
        }

        public static T Get<T>(String partitionId, String rowId) where T : EntityModel
        {
            String tableName = GetTableName<T>();
            String cacheKey = String.Format("{0}_{1}_{2}", tableName, partitionId, rowId);

            Debug.Write("[Table] Get: " + tableName + ": {" + partitionId + ", " + rowId + "}");

            T entity = Cache.Get("objects").Get(cacheKey) as T;

            if (entity == null)
            {
                CloudTable table = TableHelper.GetTable(tableName);

                TableResult retrievedResult = table.Execute(TableOperation.Retrieve<T>(partitionId, rowId));

                entity = retrievedResult.Result as T;

                if (entity != null)
                    Cache.Get("objects").Put(cacheKey, entity);
            }
            else
            {
                Debug.Write(" - From Cache");
            }

            Debug.WriteLine("");

            return entity;
        }

        public static T GetOrNew<T>(String partitionId, String rowId) where T : EntityModel, new()
        {
            String tableName = GetTableName<T>();
            String cacheKey = String.Format("{0}_{1}_{2}", tableName, partitionId, rowId);


            T entity = Cache.Get("objects").Get(cacheKey) as T;

            if (entity == null)
            {
                CloudTable table = TableHelper.GetTable(tableName);

                TableResult retrievedResult = table.Execute(TableOperation.Retrieve<T>(partitionId, rowId));

                entity = retrievedResult.Result as T;

                if (entity == null)
                    entity = new T() { PartitionKey = partitionId, RowKey = rowId };

                Cache.Get("objects").Put(cacheKey, entity);
            }

            return entity;
        }

        public static T Get<T>(Guid partitionId, Guid rowId) where T : EntityModel
        {
            return Get<T>(partitionId.ToString(), rowId.ToString());
        }

        public static IEnumerable<T> GetByPartitionId<T>(String partitionId) where T : EntityModel, new()
        {
            String tableName = GetTableName<T>();
            Debug.WriteLine("[Table] GetByPartitionId: " + tableName + ": {" + partitionId + "}");

            TableQuery<T> rangeQuery = new TableQuery<T>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionId));

            return TableHelper.GetTable(tableName).ExecuteQuery<T>(rangeQuery);
        }

        public static IEnumerable<T> GetByRowId<T>(String rowId) where T : EntityModel, new()
        {
            String tableName = GetTableName<T>();
            Debug.WriteLine("[Table] GetByRowId: " + tableName + ": {" + rowId + "}");

            TableQuery<T> rangeQuery = new TableQuery<T>().Where(TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, rowId));

            return TableHelper.GetTable(tableName).ExecuteQuery<T>(rangeQuery);
        }

        public static IEnumerable<T> GetByPartitionId<T>(Guid partitionId) where T : EntityModel, new()
        {
            return GetByPartitionId<T>(partitionId.ToString());
        }

        public static IEnumerable<T> GetWhere<T>(String filter) where T : EntityModel, new()
        {
            String tableName = GetTableName<T>();
            Debug.WriteLine("[Table] GetWhere: " + tableName + ": {" + filter + "}");

            TableQuery<T> rangeQuery = new TableQuery<T>().Where(filter);

            return TableHelper.GetTable(tableName).ExecuteQuery<T>(rangeQuery);
        }

        public static IEnumerable<T> GetAll<T>() where T : EntityModel, new()
        {
            String tableName = GetTableName<T>();
            Debug.WriteLine("[Table] GetAll: " + tableName + ": {}");

            TableQuery<T> rangeQuery = new TableQuery<T>();

            return TableHelper.GetTable(tableName).ExecuteQuery<T>(rangeQuery);
        }

        public static IEnumerable<string> GetPartitionIds<T>() where T : EntityModel, new()
        {
            String tableName = GetTableName<T>();
            Debug.WriteLine("[Table] GetPartitionIds: " + tableName + ": {}");

            var row = GetAll<T>().FirstOrDefault();

            while (row != null)
            {
                yield return row.PartitionKey;

                row = GetWhere<T>(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.GreaterThan,
                    row.PartitionKey)).FirstOrDefault();
            }

        }
    }
}
