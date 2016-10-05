using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.Storage.Blob;
using System.IO;

namespace Crondale.AzureWrapper.Storage
{
    public static class BlobHelper
    {
        public static CloudBlobContainer GetBlob(String containerName)
        {
            CloudBlobClient blobClient = StorageHelper.GetAccount().CreateCloudBlobClient();

            // Retrieve a reference to a container. 
            CloudBlobContainer container = blobClient.GetContainerReference("files");

            // Create the container if it doesn't already exist.
            container.CreateIfNotExists();

            return container;
        }

        public static void PutFile(Guid fileId, Stream stream)
        {
            PutFile(fileId.ToString(), stream);
        }

        public static void PutFile(String fileId, Stream stream)
        {
            CloudBlockBlob cbb = BlobHelper.GetBlob("files").GetBlockBlobReference(fileId);

            cbb.UploadFromStream(stream);
        }

        public static Stream GetFile(Guid fileId)
        {
            return GetFile(fileId.ToString());
        }

        public static Stream GetFile(String fileId)
        {

            CloudBlockBlob cbb = BlobHelper.GetBlob("files").GetBlockBlobReference(fileId);

            MemoryStream ms = new MemoryStream();
            cbb.DownloadToStream(ms);
            ms.Position = 0;
            return ms;
        }
    }
}
