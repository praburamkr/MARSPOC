using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Authentication.Models
{
    public class BlobStorageService
    {
        //Common configuration details for the Azure Blob storage
        private const string accountKey = "E1G1H82DbkHUzMmxfG9iSTzSUbrAYYCZZ1/wFeXqS8AxY18ssZhTYrzVDjr2JRr1WMQbnQtyk3ljrgdSroNfrg==";
        private const string accountName = "marspocstorage";
        private const string imageContainer = "marspoc";


        public BlobStorageService()
        {

        }

        /// <summary>
        ///  Generate and return the filename as per the current datetime
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private string GenerateFileName(string fileName)
        {
            string strFileName = string.Empty;
            string[] strName = fileName.Split('.');
            strFileName = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-dd") + "/" + DateTime.Now.ToUniversalTime().ToString("yyyyMMdd\\THHmmssfff") + "." + strName[strName.Length - 1];
            return strFileName;
        }


        /// <summary>
        ///  
        /// </summary>
        /// <param name="fileStream">File stream data as per the filename</param>
        /// <param name="fileName">Filename with format directoryname/filename </param>
        /// <returns>Returnt the Absolute Url after upload the file onto blob</returns>
        public async Task<string> UploadFileToStorage(Stream fileStream, string fileName)
        {
            try
            {
                // Create storagecredentials object by reading the values from the configuration (appsettings.json)
                StorageCredentials storageCredentials = new StorageCredentials(accountName, accountKey);

                // Create cloudstorage account by passing the storagecredentials
                CloudStorageAccount storageAccount = new CloudStorageAccount(storageCredentials, true);

                // Create the blob client.
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

                // Get reference to the blob container by passing the name by reading the value from the configuration (appsettings.json)
                CloudBlobContainer container = blobClient.GetContainerReference(imageContainer);

                //Check the cloud Blob  
                if (await container.CreateIfNotExistsAsync())
                {
                    await container.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
                }

                // Get the reference to the block blob from the container
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(fileName);

                // Upload the file
                await blockBlob.UploadFromStreamAsync(fileStream);

                //return await Task.FromResult(true);

                return blockBlob.Name;
            }
            catch (Exception ex)
            {
                throw (ex);
            }

        }

        public CloudBlobContainer GetContainerFromStorage()
        {
            // Create storagecredentials object by reading the values from the configuration (appsettings.json)
            StorageCredentials storageCredentials = new StorageCredentials(accountName, accountKey);

            // Create cloudstorage account by passing the storagecredentials

            CloudStorageAccount storageAccount = new CloudStorageAccount(storageCredentials, true);

            CloudBlobClient BlobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = BlobClient.GetContainerReference(imageContainer);

            return container;
        }
    }
}
