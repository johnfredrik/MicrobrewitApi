using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using Microbrewit.Model.DTOs;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Microbrewit.Api
{
    public class BlobStorageMultipartStreamProvider : MultipartStreamProvider
    {
        private string _identifyer;
        private BreweryDto _breweryDto;
        private UserDto _userDto;

        public BlobStorageMultipartStreamProvider(BreweryDto breweryDto)
        {
            _breweryDto = breweryDto;
        }
        public BlobStorageMultipartStreamProvider(UserDto userDto)
        {
            _userDto = userDto;
        }

        public override Stream GetStream(HttpContent parent, HttpContentHeaders headers)
        {
            Stream stream = null;
            ContentDispositionHeaderValue contentDisposition = headers.ContentDisposition;
            if (contentDisposition != null)
            {
                if (!String.IsNullOrWhiteSpace(contentDisposition.FileName))
                {
                    string connectionString = ConfigurationManager.AppSettings["azureConnectionString"];
                    var containerName = ConfigurationManager.AppSettings["container"];
                    if (contentDisposition.Name.Contains("avatar"))
                        containerName = "avatar";
                    if (contentDisposition.Name.Contains("headerImage"))
                        containerName = "header";
                    CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
                    CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                    

                    CloudBlobContainer blobContainer = blobClient.GetContainerReference(containerName);
                    CloudBlockBlob blob = blobContainer.GetBlockBlobReference(Guid.NewGuid().ToString() + ".jpg");
                    blob.Metadata.Add("source", ConfigurationManager.AppSettings["source"]);
                    if (_breweryDto != null) blob.Metadata.Add("breweryId", _breweryDto.Id.ToString());
                    if(_userDto != null) blob.Metadata.Add("username", _userDto.Username);
                    stream = blob.OpenWrite();
                    headers.ContentDisposition.FileName = blob.Name;
                }
            }
            return stream;
        }
    }
}