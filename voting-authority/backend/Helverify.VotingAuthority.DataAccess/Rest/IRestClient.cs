namespace Helverify.VotingAuthority.DataAccess.Rest;

public interface IRestClient
{
    Task<T> Call<T>(HttpMethod method, Uri endpoint, object? body = null);
}