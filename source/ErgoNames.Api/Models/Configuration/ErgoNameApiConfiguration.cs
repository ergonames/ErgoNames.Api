namespace ErgoNames.Api.Models.Configuration
{
    public class ErgoNameApiConfiguration
    {
        public NetworkType NetworkType { get; set; }

        public string IssuingAddress { get; set; }

        public string AuthUser { get; set; }

        public string AuthPassword { get; set; }
    }

    public enum NetworkType
    {
        Mainnet,
        Testnet
    }
}
