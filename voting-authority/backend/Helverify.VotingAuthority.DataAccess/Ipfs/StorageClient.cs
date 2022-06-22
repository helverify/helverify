using Ipfs;
using Ipfs.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Helverify.VotingAuthority.DataAccess.Ipfs
{
    /// <inheritdoc cref="IStorageClient"/>
    public class StorageClient : IStorageClient
    {
        private readonly IpfsClient _ipfsClient;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ipfsClient">IPFS connector</param>
        public StorageClient(IpfsClient ipfsClient)
        {
            _ipfsClient = ipfsClient;
        }

        /// <inheritdoc cref="IStorageClient.Store{T}"/>
        public async Task<string> Store<T>(T obj)
        {
            string json = JsonConvert.SerializeObject(obj, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.Indented
            });

            IFileSystemNode fileSystemNode = await _ipfsClient.FileSystem.AddTextAsync(json);
            
            return fileSystemNode.Id.ToString();
        }

        /// <inheritdoc cref="IStorageClient.Retrieve{T}"/>
        public async Task<T> Retrieve<T>(string id)
        {
            CancellationToken token = default(CancellationToken);

            Stream stream = await _ipfsClient.PostDownloadAsync("cat", token, id); // according to https://github.com/richardschneider/net-ipfs-http-client/issues/71

            string? json = await new StreamReader(stream).ReadToEndAsync();
            
            T obj = JsonConvert.DeserializeObject<T>(json);

            return obj;
        }
    }
}
