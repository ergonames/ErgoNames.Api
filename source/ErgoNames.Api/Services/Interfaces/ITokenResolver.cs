using System.Threading.Tasks;

namespace ErgoNames.Api.Services.Interfaces
{
    public interface ITokenResolver
    {
        /// <summary>
        /// Gets the address of the wallet holding the given token.
        /// </summary>
        /// <param name="id">Id of the token to resolve to an address</param>
        /// <returns>Address of the wallet holding the token.</returns>
        Task<string?> ResolveTokenNameToAddressAsync(string tokenName);
    }

}
