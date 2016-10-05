using Crondale.AzureWrapper.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crondale.AzureWrapper.Caching
{
    public interface ICache
    {

        void Put(String key, EntityModel value,TimeSpan? timespan = null);
        void Put(String key, String value,TimeSpan? timespan = null);
        void Put(String key, int value,TimeSpan? timespan = null);
         void Put(String key, object value,TimeSpan? timespan = null);
        Object Get(String key);
        EntityModel GetEntityModel(String key);
        string GetString(String key);
        int GetInt(String key);
        void Remove(String key);

        void Increment(String key);
        void Decrement(String key);

        bool KeyExists(String key);
    }
}
