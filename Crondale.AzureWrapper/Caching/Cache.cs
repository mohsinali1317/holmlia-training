using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Text.RegularExpressions;
using Microsoft.Azure;
using System.Diagnostics;

namespace Crondale.AzureWrapper.Caching
{
    public class Cache
    {
        private static Dictionary<String, ICache> caches = new Dictionary<string, ICache>();

        private static Dictionary<String, Type> cacheTypes = new Dictionary<string, Type>()
        {
            {"RedisCache", typeof(RedisCache)}
        };


        public static ICache Get(String str)
        {
            ICache cache = null;

            lock (caches)
            {
                if (!caches.TryGetValue(str, out cache))
                {
                    String config = CloudConfigurationManager.GetSetting("cache-" + str);

                    if (config != null)
                    {
                        Match m = Regex.Match(config, "(?<cacheType>[a-zA-Z]*)=(?<connectionString>.*)");
                        if (!m.Success)
                        {
                            throw new Exception("Failed to parse connection string");
                        }

                        String cacheType = m.Groups["cacheType"].Value;
                        String connectionString = m.Groups["connectionString"].Value;

                        Type cacheTypeType = cacheTypes[cacheType];
                        try
                        {
                            cache = Activator.CreateInstance(cacheTypeType, connectionString) as ICache;
                        }
                        catch (Exception e)
                        {
                            cache = new DummyCache();
                            Debug.WriteLine(e.Message);
                        }
                        caches[str] = cache;

                    }
                    else
                    {
                        cache = new DummyCache();
                        caches[str] = cache;
                    }

                }
            }


            return cache;
        }



    }
}
