using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.Azure;
using SnowMaker;
using Crondale.AzureWrapper.Caching;

namespace Crondale.AzureWrapper.Storage
{
    public static class StorageHelper
    {
        private static String connectionString = null;
        public static String ConnectionString
        {
            get
            {
                if (connectionString == null && ConfigurationManager.ConnectionStrings["objectStorage"] != null)
                    connectionString = ConfigurationManager.ConnectionStrings["objectStorage"].ConnectionString;

                if (connectionString == null)
                    connectionString = CloudConfigurationManager.GetSetting("objectStorage");

                if (connectionString == null)
                    connectionString = "UseDevelopmentStorage=true";

                return connectionString;
            }
            set
            {
                connectionString = value;
            }
        }

        public static CloudStorageAccount GetAccount()
        {
            return CloudStorageAccount.Parse(ConnectionString);
        }

        public static CloudTable GetTable(String tableName)
        {
            CloudStorageAccount cloudStorageAccount = StorageHelper.GetAccount();
            CloudTableClient cloudTableClient = cloudStorageAccount.CreateCloudTableClient();
            CloudTable table = cloudTableClient.GetTableReference(tableName);
            table.CreateIfNotExists();

            return table;
        }

        private static UniqueIdGenerator serialIdGenerator = null;
        public static UniqueIdGenerator SerialIdGenerator
        {
            get
            {
                if (serialIdGenerator == null)
                    serialIdGenerator = new UniqueIdGenerator(new BlobOptimisticDataStore(GetAccount(), "snowmaker"));

                return serialIdGenerator;
            }
        }

        public static long GetSerialId(string name)
        {
            return SerialIdGenerator.NextId(name);
        }

        /*
        [Obsolete("Remove before release")]
        public static void ClearStorage()
        {

            CloudStorageAccount cloudStorageAccount = GetAccount();
            CloudTableClient cloudTableClient = cloudStorageAccount.CreateCloudTableClient();

            foreach (CloudTable table in cloudTableClient.ListTables())
            {
                table.Delete();
            }

            CloudQueueClient queueClient = cloudStorageAccount.CreateCloudQueueClient();

            foreach (CloudQueue queue in queueClient.ListQueues())
            {
                queue.Delete();
            }

        }
        */
        public static void ClearLocalDev()
        {
            if (ConnectionString != "UseDevelopmentStorage=true")
                return;

            RedisCache rc = Cache.Get("tippnett") as RedisCache;
            if (rc != null)
                rc.RemoveAll();

            rc = Cache.Get("objects") as RedisCache;
            if (rc != null)
                rc.RemoveAll();

            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse("UseDevelopmentStorage=true");
            CloudTableClient cloudTableClient = cloudStorageAccount.CreateCloudTableClient();

            foreach (CloudTable table in cloudTableClient.ListTables())
            {
                table.Delete();
            }

            CloudQueueClient queueClient = cloudStorageAccount.CreateCloudQueueClient();

            foreach (CloudQueue queue in queueClient.ListQueues())
            {
                queue.Delete();
            }

        }

        public static void ClearCache()
        {
            if (ConnectionString != "UseDevelopmentStorage=true")
                return;

            RedisCache rc = Cache.Get("tippnett") as RedisCache;
            if (rc != null)
                rc.RemoveAll();

            rc = Cache.Get("objects") as RedisCache;
            if (rc != null)
                rc.RemoveAll();
            

        }



    }
}
