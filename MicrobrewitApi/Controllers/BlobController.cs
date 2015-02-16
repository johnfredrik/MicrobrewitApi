using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microbrewit.Service.Interface;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Thinktecture.IdentityModel.Authorization.Mvc;

namespace Microbrewit.Api.Controllers
{
    //[ClaimsAuthorize("Blob")]
    [RoutePrefix("blob")]
    public class BlobController : ApiController
    {
        private readonly IUserService _userService;

        public BlobController(IUserService userService)
        {
            _userService = userService;
        }

        public async Task GetCleanBlobStorage()
        {
            var connectionString = ConfigurationManager.AppSettings["azureConnectionString"];
            var storageAccount = CloudStorageAccount.Parse(connectionString);
            var blobClient = storageAccount.CreateCloudBlobClient();

            CloudBlobContainer blobContainer = blobClient.GetContainerReference("avatar");
            //List blobs with a paging size of 10, for the purposes of the example. 
            //The first call does not include the continuation token.
            BlobResultSegment resultSegment = await blobContainer.ListBlobsSegmentedAsync(
                    "", true, BlobListingDetails.All, 100, null, null, null);

            //Enumerate the result segment returned.
            int i = 0;
            if (resultSegment.Results.Any()) { Trace.WriteLine(string.Format("Page {0}:", ++i)); }
            foreach (var blobItem in resultSegment.Results)
            {
                var name = blobItem.Uri.ToString().Split('/').LastOrDefault();
                var blob = await blobContainer.GetBlobReferenceFromServerAsync(name);
                if (blob.Metadata.ContainsKey("source") && blob.Metadata["source"] == ConfigurationManager.AppSettings["source"])
                {
                    if (blob.Metadata.ContainsKey("breweryId")) Trace.WriteLine(blob.Metadata["breweryId"]); ;
                    if (blob.Metadata.ContainsKey("username"))
                    {
                        var user = await _userService.GetSingleAsync(blob.Metadata["username"]);
                        var avatar = user.Avatar.Split('/').LastOrDefault();
                        if (avatar != blob.Name) Trace.WriteLine(blob.Name);
                    }
                }
                
                //Trace.WriteLine(string.Format("\t{0}, meta: {1}", blobItem.StorageUri.PrimaryUri, blob.Metadata["username"]));
            }
            Console.WriteLine();

            //Get the continuation token, if there are additional pages of results.
            BlobContinuationToken continuationToken = resultSegment.ContinuationToken;

            //Check whether there are more results and list them in pages of 10 while a continuation token is returned.
            while (continuationToken != null)
            {
                //This overload allows control of the page size. 
                //You can return all remaining results by passing null for the maxResults parameter, 
                //or by calling a different overload.
                resultSegment = await blobContainer.ListBlobsSegmentedAsync(
                        "", true, BlobListingDetails.All, 10, continuationToken, null, null);
                if (resultSegment.Results.Any()) { Console.WriteLine("Page {0}:", ++i); }
                foreach (var blobItem in resultSegment.Results)
                {
                    var blob = blobContainer.GetBlockBlobReference(blobItem.Uri.ToString());
                    Trace.WriteLine(string.Format("\t{0}, meta count: {1}", blobItem.StorageUri.PrimaryUri,blob.Metadata.Count));
                }
                Console.WriteLine();

                //Get the next continuation token.
                continuationToken = resultSegment.ContinuationToken;
            }

        } 
    }
}
