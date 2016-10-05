using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crondale.AzureWrapper.Caching
{
    //public class DictionaryCache:ICache
    //{
    //    static Dictionary<String, object> _cache = new Dictionary<string, object>();

    //    public void Put(string key, object obj)
    //    {
    //        _cache[key] = obj;
    //    }

    //    public object Get(string key)
    //    {
    //        object obj = null;

    //        if(_cache.TryGetValue(key, out obj))
    //            return obj;

    //        return null;
    //    }

    //    public void Remove(string key)
    //    {
    //        _cache.Remove(key);
    //    }

    //    public void Put(string key, object obj, TimeSpan? timespan = default(TimeSpan?))
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void Increment(string key)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void Decrement(string key)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public bool KeyExists(string key)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}
