using Mx.NET.SDK.Provider.Dtos.Gateway.Blocks;
using System.Threading.Tasks;

namespace Mx.NET.SDK.Provider.Gateway
{
    public interface IBlocksProvider
    {
        /// <summary>
        /// This endpoint allows one to query a Shard Block by its nonce (or height).
        /// </summary>
        /// <returns><see cref="BlockDto"/></returns>
        Task<BlockDataDto> GetBlockByNonce(long nonce, long shard, bool withTxs = false);

        /// <summary>
        /// This endpoint allows one to query a Shard Block by its hash.
        /// </summary>
        /// <returns><see cref="BlockDto"/></returns>
        Task<BlockDataDto> GetBlockByHash(string hash, long shard, bool withTxs = false);

        Task<InternalBlockResponseDto> GetInternalBlockByNonce(long nonce);
        Task<InternalBlockResponseDto> GetInternalBlockByHash(string hash);

    }
}
