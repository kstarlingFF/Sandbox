using System;
using System.IO;
using System.Text;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;

namespace GithubActions.Tests.IntegrationTests.Persistance;
internal class StorageInitializer
{
    public static void InitializeBlobStorage(IConfiguration configuration)
    {
        try
        {
            var serviceClient = new BlobServiceClient(configuration["Storage:StorageConnectionString"]);
            var client = serviceClient.GetBlobContainerClient(configuration["Storage:Container"]);
            client.DeleteIfExists();
            client.CreateIfNotExists();

            var blobName = "sandbox.json";
            var content = "this is a test";

            var encoded = Encoding.UTF8.GetBytes(content);
            using (var stream = new MemoryStream(encoded))
            {
                client.UploadBlob(blobName, stream, default);
            }
        }
        catch (Exception e)
        {
            throw;
        }
    }
}
