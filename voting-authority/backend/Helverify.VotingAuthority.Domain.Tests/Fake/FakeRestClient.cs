using Helverify.VotingAuthority.DataAccess.Rest;

namespace Helverify.VotingAuthority.Domain.Tests.Fake
{
    internal class FakeRestClient: IRestClient
    {
        public IList<object> Items { get; set; } = new List<object>();

        public async Task<T?> Call<T>(HttpMethod method, Uri endpoint, object? body = null)
        {
            Items.Add(body);

            return default;
        }

        public async Task Call(HttpMethod method, Uri endpoint, object? body = null)
        {
            Items.Add(body);
        }
    }
}
