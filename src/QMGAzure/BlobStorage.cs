/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using System.Text;
using System.IO;

namespace QMGAzure
{
    public class BlobStorage
    {
        private CloudBlobClient _blobClient;
        private CloudStorageAccount _cloudStorageAccount;
        private CloudBlobContainer _container;

        public BlobStorage(string connectionString)
        {
            _cloudStorageAccount = CloudStorageAccount.Parse(connectionString);
            _blobClient = _cloudStorageAccount.CreateCloudBlobClient();
            _container = _blobClient.GetContainerReference("qmgbaskets");
            _container.CreateIfNotExists();
        }

        public void uploadBlob<T>(string blobReference, T file)
        {
            var jsonToUpload = JsonConvert.SerializeObject(file);
            var blockBlob = _container.GetBlockBlobReference(blobReference);
            blockBlob.UploadFromByteArray(Encoding.ASCII.GetBytes(jsonToUpload),0,jsonToUpload.Length);
        }

        public T DownloadBlob<T>(string blobReference)
        {
            var blockBlob = _container.GetBlockBlobReference(blobReference);
            var bytes = new byte[blockBlob.StreamWriteSizeInBytes];

            blockBlob.DownloadToByteArray(bytes, 0);

            return JsonConvert.DeserializeObject<T>(Encoding.Default.GetString(bytes));
        }

        public void deleteBlob(string blobReference)
        {
            var blockBlob = _container.GetBlockBlobReference(blobReference);
            blockBlob.Delete();
        }
    }
}
*/
