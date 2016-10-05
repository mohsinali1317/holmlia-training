using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StackExchange.Redis;
using Crondale.AzureWrapper.Caching;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using System.Text;
using Newtonsoft.Json;
using Crondale.AzureWrapper.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System.Globalization;
using System.Diagnostics;

namespace Crondale.AzureWrapper.Caching
{
    public class RedisCache : ICache
    {
        public static Lazy<ConnectionMultiplexer> lazyConnection;
        public IDatabase IDB;

        public static ConnectionMultiplexer Connection
        {
            get
            {
                if (!lazyConnection.Value.IsConnected)
                    throw new Exception("RedisCache failed to connect");
                return lazyConnection.Value;
            }
        }

        public RedisCache(string host)
        {
            try
            {
                lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
                {
                    return ConnectionMultiplexer.Connect(host + ",allowAdmin=true,abortConnect=false");
                });
                IDB = Connection.GetDatabase();
            }
            catch (Exception e)
            {
                throw new Exception("RedisCache failed to connect", e);
            }

        }


        public void Put(string key, EntityModel value, TimeSpan? timespan = null)
        {

            if (!timespan.HasValue)
                IDB.StringSet(key, ((EntityModel)value).Serialize());
            else
                IDB.StringSet(key, ((EntityModel)value).Serialize(), timespan);

        }

        public void Put(string key, String value, TimeSpan? timespan = null)
        {
            if (!timespan.HasValue)
                IDB.StringSet(key, value);
            else
                IDB.StringSet(key, value, timespan);
        }

        public void Put(string key, int value, TimeSpan? timespan = null)
        {
            //if (!timespan.HasValue)
            //    IDB.StringSet(key, value);
            //else
            //    IDB.StringSet(key, value, timespan);

            Put(key, value.ToString(), timespan);
        }

        public void Put(string key, object value, TimeSpan? timespan = null)
        {

            BinaryFormatter binaryFormatter = new BinaryFormatter();
            byte[] objectDataAsStream;

            using (MemoryStream memoryStream = new MemoryStream())
            {
                binaryFormatter.Serialize(memoryStream, value);
                objectDataAsStream = memoryStream.ToArray();
            }

            //RedisValue r;

            //if (value is EntityModel)
            //{
            //    r = ((EntityModel)value).Serialize();
            //}
            //else
            //{
            //    if (value is string)
            //        r = (string)value;
            //    else
            //        throw new InvalidOperationException("Invalid type for Redis");

            //}

            if (!timespan.HasValue)
                IDB.StringSet(key, objectDataAsStream);
            else
                IDB.StringSet(key, objectDataAsStream, timespan);

        }

        public EntityModel GetEntityModel(string key)
        {
            RedisValue value = IDB.StringGet(key);

            if (value.IsNull)
                return null;

            var res = (byte[])value;
            return EntityModel.Deserialize(res) as EntityModel;
        }

        public String GetString(string key)
        {
            RedisValue value = IDB.StringGet(key);

            if (value.IsNull)
                return null;

            return (string)value;
        }

        public int GetInt(string key)
        {
            RedisValue value = IDB.StringGet(key);

            if (value.IsNull)
                return -1;

            return Int32.Parse(value);
        }

        public object Get(string key)
        {
            RedisValue value = IDB.StringGet(key);

            if (value.IsNull)
                return null;

            try
            {
                var bytes = (byte[])value;
                return EntityModel.Deserialize(bytes);
            }
            catch (InvalidCastException)
            {
            }

            return value;
        }

        public void Remove(string key)
        {
            IDB.KeyDelete(key);
        }

        /// <summary>
        /// Just to used when clearing the full cache
        /// </summary>
        public void RemoveAll()
        {
            var endpoints = Connection.GetEndPoints(true);
            foreach (var endpoint in endpoints)
            {
                var server = Connection.GetServer(endpoint);
                server.FlushAllDatabases();
            }
        }

        public void Increment(string key)
        {
            IDB.StringIncrement(key);
        }

        public void Decrement(string key)
        {
            IDB.StringDecrement(key);
        }


        public bool KeyExists(string key)
        {
            return IDB.KeyExists(key);
        }

    }
}