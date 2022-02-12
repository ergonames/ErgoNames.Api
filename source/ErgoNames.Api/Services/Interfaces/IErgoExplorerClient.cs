namespace ErgoNames.Api.Services.Interfaces
{
    public interface IErgoExplorerClient
    {
        Task<T> GetAsync<T>(string queryString);
    }
}
