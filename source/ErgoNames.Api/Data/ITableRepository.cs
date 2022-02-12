using System.Threading.Tasks;
using Azure.Data.Tables;
using ErgoNames.Api.Models.Configuration;

namespace ErgoNames.Api.Data
{
    public interface ITableRepository
    {
        Task InitializeTables();

        Task ReserveName(string name);

        Task ReleaseName(string name);
    }

    public class TableRepository : ITableRepository
    {
        private readonly TableServiceClient tableClient;
        private readonly ErgoNameApiConfiguration configuration;

        public TableRepository(ErgoNameApiConfiguration configuration, StorageConnectionString storageConnectionString)
        {
            this.configuration = configuration;
            tableClient = new TableServiceClient(storageConnectionString.ConnectionString);
        }

        public async Task InitializeTables()
        {
            if (configuration.NetworkType == NetworkType.Testnet)
            {
                await tableClient.CreateTableIfNotExistsAsync("testnetapi");
            }
            else
            {
                await tableClient.CreateTableIfNotExistsAsync("mainnetapi");
            }
        }

        public Task ReserveName(string name)
        {
            throw new System.NotImplementedException();
        }

        public Task ReleaseName(string name)
        {
            throw new System.NotImplementedException();
        }
    }
}
