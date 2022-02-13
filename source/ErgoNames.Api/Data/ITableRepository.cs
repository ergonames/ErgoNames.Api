using System.Security.Cryptography;
using System.Threading.Tasks;
using Azure.Data.Tables;
using ErgoNames.Api.Models;
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

        public async Task ReserveName(string name)
        {
            string loweredString = name.ToLowerInvariant();
            TableEntity entity = new TableEntity();
            entity.PartitionKey = HashString(loweredString);
            entity.RowKey = "ergonames";

            // The other values are added like a items to a dictionary
            entity["Name"] = loweredString;
            entity["Requested"] = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            await tableClient.AddEntityAsync(entity);
        }

        public async Task ReleaseName(string name)
        {
            string loweredString = name.ToLowerInvariant();
            string hashedString = HashString(loweredString);

            await tableClient.DeleteEntityAsync(hashedString, "ergonames");
        }
        
        private string HashString(string text, string salt = "")
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            using (var sha = SHA256.Create())
            {
                byte[] textBytes = System.Text.Encoding.UTF8.GetBytes(text + salt);
                byte[] hashBytes = sha.ComputeHash(textBytes);

                string hash = BitConverter
                    .ToString(hashBytes)
                    .Replace("-", string.Empty);

                return hash;
            }
        }
    }
}
