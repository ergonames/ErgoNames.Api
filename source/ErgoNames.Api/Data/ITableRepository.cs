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
        private readonly TableClient tableClient;

        public TableRepository(TableClient tableClient)
        {
            this.tableClient = tableClient;
        }

        public async Task InitializeTables()
        {
            await tableClient.CreateIfNotExistsAsync();
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
