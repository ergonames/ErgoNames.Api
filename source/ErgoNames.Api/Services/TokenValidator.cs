using ErgoNames.Api.Models.Responses;
using ErgoNames.Api.Services.Interfaces;

namespace ErgoNames.Api.Services
{
    public class TokenValidator : ITokenValidator
    {
        private readonly IErgoExplorerClient explorerClient;

        public TokenValidator(IErgoExplorerClient explorerClient)
        {
            this.explorerClient = explorerClient;
        }

        public async Task<IEnumerable<ExplorerAssetResponse>> SearchTokenByNameAsync(string tokenName)
        {
            var uri = $"tokens/bySymbol/{tokenName}";
            var results = await explorerClient.GetAsync<IEnumerable<ExplorerAssetResponse>>(uri);
            return results;
        }

        public async Task<ExplorerBoxResponse> LookUpIssuanceBox(ExplorerAssetResponse token)
        {
            var issuanceBoxUri = $"boxes/{token.BoxId}";
            var issuanceBox = await explorerClient.GetAsync<ExplorerBoxResponse>(issuanceBoxUri);
            return issuanceBox;
        }

        public async Task<ExplorerTransactionResponse> LookUpMintTx(ExplorerBoxResponse box)
        {
            var mintTxUri = $"transactions/{box.TransactionId}";
            var mintTx = await explorerClient.GetAsync<ExplorerTransactionResponse>(mintTxUri);
            return mintTx;
        }

        public bool MintTxWasCreatedByMintAddress(ExplorerTransactionResponse mintTx, string mintAddress)
        {
            return mintTx.Inputs.First().Address == mintAddress;
        }

        public async Task<string?> ResolveTokenAddressAsync(ExplorerAssetResponse token)
        {
            var unspentBoxUri = $"boxes/unspent/byTokenId/{token.Id}";
            var unspentBox = await explorerClient.GetAsync<ExplorerResponseWrapper<ExplorerBoxResponse>>(unspentBoxUri);
            var holderAddress = unspentBox.Items.Any() ? unspentBox.Items.First().Address : null;
            return holderAddress;
        }

        public Task<bool> TokenHasBeenBurnedAsync(ExplorerAssetResponse token)
        {
            throw new NotImplementedException();
        }

    }
}
