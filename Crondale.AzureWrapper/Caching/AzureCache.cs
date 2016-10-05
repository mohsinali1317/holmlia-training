using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ApplicationServer.Caching;
using Crondale.AzureWrapper.Storage;

namespace Crondale.AzureWrapper.Caching
{
    public class AzureCache:ICache
    {
        DataCache dataCache = null;

        public AzureCache(String name)
        {
            dataCache = new DataCache(name);
        }

        public void Put(string key, object obj)
        {
            dataCache.Put(key, obj);
        }

        public object Get(string key)
        {
            return dataCache.Get(key);
        }

        public void Remove(string key)
        {
            dataCache.Remove(key);
        }

        public bool KeyExists(string key)
        {
            return false;
        }

        public void Put(string key, object obj, TimeSpan? timespan = default(TimeSpan?))
        {
            throw new NotImplementedException();
        }

        public void Increment(string key)
        {
            throw new NotImplementedException();
        }

        public void Decrement(string key)
        {
            throw new NotImplementedException();
        }

        public void Put(string key, EntityModel value, TimeSpan? timespan = null)
        {

            throw new NotImplementedException();
        }

        public void Put(string key, String value, TimeSpan? timespan = null)
        {
            throw new NotImplementedException();
        }

        public void Put(string key, int value, TimeSpan? timespan = null)
        {
            throw new NotImplementedException();
        }

        EntityModel ICache.GetEntityModel(string key)
        {
            throw new NotImplementedException();
        }

        String ICache.GetString(string key)
        {
            throw new NotImplementedException();
        }

        int ICache.GetInt(string key)
        {
            throw new NotImplementedException();
        }
    }
}
