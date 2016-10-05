using Crondale.AzureWrapper.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crondale.AzureWrapper.Caching
{
    public class DummyCache:ICache
    {

        public void Put(string key, object obj)
        {
            
        }

        public object Get(string key)
        {
            return null;
        }

        public void Remove(string key)
        {

        }

        public void Put(string key, object obj, TimeSpan? timespan = default(TimeSpan?))
        {
            
        }

        public void Increment(string key)
        {
            
        }

        public void Decrement(string key)
        {
            
        }

        public bool KeyExists(string key)
        {
            return false;
        }

        public void Put(string key, EntityModel value, TimeSpan? timespan = null)
        {

        }

        public void Put(string key, String value, TimeSpan? timespan = null)
        {
        }

        public void Put(string key, int value, TimeSpan? timespan = null)
        {
        }

        EntityModel ICache.GetEntityModel(string key)
        {
            return null;
        }

        string ICache.GetString(string key)
        {
            return null;
        }

        int ICache.GetInt(string key)
        {
            return -1;
        }

    }
}
