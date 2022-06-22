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
            string? json = await _ipfsClient.FileSystem.ReadAllTextAsync(id);

            T obj = JsonConvert.DeserializeObject<T>(json);

            return obj;
        }
    }
}
