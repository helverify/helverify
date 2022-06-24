﻿using Ipfs;
using Ipfs.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Helverify.VotingAuthority.DataAccess.Ipfs
{
    /// <inheritdoc cref="IStorageClient"/>
    public class StorageClient : IStorageClient
    {
        private readonly IpfsClient _ipfsClient;
        private readonly JsonSerializerSettings _serializerSettings;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ipfsClient">IPFS connector</param>
        public StorageClient(IpfsClient ipfsClient)
        {
            _ipfsClient = ipfsClient;
            _serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.None
            };
        }

        /// <inheritdoc cref="IStorageClient.Store{T}"/>
        public async Task<string> Store<T>(T obj)
        {
            string json = JsonConvert.SerializeObject(obj, _serializerSettings);

            IFileSystemNode fileSystemNode = await _ipfsClient.FileSystem.AddTextAsync(json);
            
            return fileSystemNode.Id.ToString();
        }

        /// <inheritdoc cref="IStorageClient.Retrieve{T}"/>
        public async Task<T> Retrieve<T>(string id)
        {
            CancellationToken token = default(CancellationToken);

            Stream stream = await _ipfsClient.PostDownloadAsync("cat", token, id); // according to https://github.com/richardschneider/net-ipfs-http-client/issues/71

            // https://www.newtonsoft.com/json/help/html/Performance.htm
            using StreamReader streamReader = new StreamReader(stream);
            using JsonReader jR = new JsonTextReader(streamReader);

            JsonSerializer jsonSerializer = new JsonSerializer();

            T obj = jsonSerializer.Deserialize<T>(jR);

            return obj;
        }
    }
}
