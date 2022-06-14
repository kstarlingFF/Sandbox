using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace GithubActions.Persistance.Storage;
public interface IBlobStorage<TDocument>
{
    Task<TDocument> FindAsync(string key);
}

public class BlobStorage : IBlobStorage<string>
{
    private readonly BlobContainerClient _client;
    
    public BlobStorage(BlobContainerClient client)
    {
        this._client = client;
    }

    public async Task<string> FindAsync(string key)
    {
        var blob = this._client.GetBlobs(BlobTraits.None, BlobStates.None, key)
            .FirstOrDefault();

        var result = await this._client
            .GetBlobClient(blob.Name)
            .DownloadContentAsync();

        return result.Value.Content.ToString();
    }
}
