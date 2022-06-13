namespace Helverify.VotingAuthority.DataAccess.Rest;

/// <summary>
/// Allows to call a REST Api
/// </summary>
public interface IRestClient
{
    /// <summary>
    /// Calls a specific REST Api endpoint with the specified parameters.
    /// </summary>
    /// <typeparam name="T">Type of result</typeparam>
    /// <param name="method">HttpMethod <see cref="HttpMethod"/></param>
    /// <param name="endpoint">Uri of the HTTP endpoint to be called</param>
    /// <param name="body">Content of the HTTP body</param>
    /// <returns></returns>
    Task<T?> Call<T>(HttpMethod method, Uri endpoint, object? body = null);
}