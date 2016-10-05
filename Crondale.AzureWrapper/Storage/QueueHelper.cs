using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.Storage.Queue;
using System.Runtime.Serialization.Formatters.Binary;

namespace Crondale.AzureWrapper.Storage
{
    public static class QueueHelper
    {


        public static CloudQueue GetQueue(String name)
        {
            CloudQueueClient queueClient = StorageHelper.GetAccount().CreateCloudQueueClient();
            CloudQueue queue = queueClient.GetQueueReference(name);
            queue.CreateIfNotExists();
            
            return queue;
        }


    }
}
