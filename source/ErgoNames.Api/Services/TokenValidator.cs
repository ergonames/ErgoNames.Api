using ErgoNames.Api.Models.Responses;
using ErgoNames.Api.Services.Interfaces;

namespace ErgoNames.Api.Services
{
    public class TokenValidator : ITokenValidator
    {
        private readonly IErgoExplorerClient explorerClient;
        private readonly ILogger<TokenValidator> logger;

        public TokenValidator(IErgoExplorerClient explorerClient, ILogger<TokenValidator> logger)
        {
            this.explorerClient = explorerClient;
            this.logger = logger;
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
            return mintTx.Inputs != null && mintTx.Inputs.First().Address == mintAddress;
        }

        public async Task<string?> ResolveTokenAddressAsync(ExplorerAssetResponse token)
        {
            var unspentBoxUri = $"boxes/unspent/byTokenId/{token.Id}";
            logger.LogDebug("Querying uri {uri} to resolve unspent boxes for token Id {id}", unspentBoxUri, token.Id);
            ExplorerResponseWrapper<ExplorerBoxResponse> unspentBox = await explorerClient.GetAsync<ExplorerResponseWrapper<ExplorerBoxResponse>>(unspentBoxUri);
            logger.LogDebug("{Count} unspent boxes found for token Id {Id}", unspentBox.Items?.Count(), token.Id);
            string? holderAddress = unspentBox.Items != null && unspentBox.Items.Any() ? unspentBox.Items.First().Address : null;
            return holderAddress;
        }

        public Task<bool> TokenHasBeenBurnedAsync(ExplorerAssetResponse token)
        {
            throw new NotImplementedException();
        }

    }
}
