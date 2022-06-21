using Ipfs;
using Ipfs.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Helverify.VotingAuthority.DataAccess.Ipfs
{
    public class StorageClient
    {
        private readonly IpfsClient _ipfsClient;

        public StorageClient(IpfsClient ipfsClient)
        {
            _ipfsClient = ipfsClient;
        }

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

        public async Task<T> Retrieve<T>(string id)
        {
            string? json = await _ipfsClient.FileSystem.ReadAllTextAsync(id);

            T obj = JsonConvert.DeserializeObject<T>(json);

            return obj;
        }
    }
}
