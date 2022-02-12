using System.Collections.Generic;
using System.Threading.Tasks;
using ErgoNames.Api.Models.Responses;

namespace ErgoNames.Api.Services.Interfaces
{
    public interface ITokenValidator
    {
        /// <summary>
        /// Searches token by name against the /tokens/bySymbol/{tokenName} endpoint on the Ergo Explorer API.
        /// </summary>
        Task<IEnumerable<ExplorerAssetResponse>> SearchTokenByNameAsync(string tokenName);

        // bool IsExactNameMatch(string tokenName, ExplorerAssetResponse token);

        /// <summary>
        /// Searches for issuance box  /tokens/bySymbol/{tokenName} endpoint on the Ergo Explorer API.
        /// </summary>
        Task<ExplorerBoxResponse> LookUpIssuanceBox(ExplorerAssetResponse token);

        Task<ExplorerTransactionResponse> LookUpMintTx(ExplorerBoxResponse box);

        bool MintTxWasCreatedByMintAddress(ExplorerTransactionResponse mintTx, string mintAddress);

        Task<string?> ResolveTokenAddressAsync(ExplorerAssetResponse token);

        /// <summary>
        /// Checks if a token by the given id is held in an unspent box by querying the /boxes/unspent/byTokenId/{tokenId} endpoint on the Ergo Explorer API.
        /// If an unspent box containing the token cannot be found, it is considered to be out of circulation.
        /// </summary>
        Task<bool> TokenHasBeenBurnedAsync(ExplorerAssetResponse token);
    }
}
