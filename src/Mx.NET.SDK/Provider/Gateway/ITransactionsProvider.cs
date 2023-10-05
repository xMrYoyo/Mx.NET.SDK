using Mx.NET.SDK.Provider.Dtos.Common.Transactions;
using Mx.NET.SDK.Provider.Dtos.Gateway.Transactions;
using System.Threading.Tasks;

namespace Mx.NET.SDK.Provider.Gateway
{
    public interface ITransactionsProvider
    {
        /// <summary>
        /// This endpoint allows one to send a signed Transaction to the Blockchain.
        /// </summary>
        /// <param name="transactionRequest">The transaction payload</param>
        /// <returns>TxHash</returns>
        Task<TransactionResponseDto> SendTransaction(TransactionRequestDto transactionRequest);

        /// <summary>
        /// This endpoint allows one to send a bulk of Transactions to the Blockchain.
        /// </summary>
        /// <param name="transactionsRequest">Array of transactions payload</param>
        /// <returns><see cref="MultipleTransactionsResponseDto"/></returns>
        Task<MultipleTransactionsResponseDto> SendTransactions(TransactionRequestDto[] transactionsRequest);

        /// <summary>
        /// This endpoint allows one to estimate the cost of a transaction.
        /// </summary>
        /// <param name="transactionRequestDto">The transaction payload</param>
        /// <returns><see cref="TransactionCostDataDto"/></returns>
        Task<TransactionCostResponseDto> GetTransactionCost(TransactionRequestDto transactionRequestDto);

        /// <summary>
        /// This endpoint allows one to query the details of a Transaction.
        /// </summary>
        /// <param name="txHash">The transaction hash</param>
        /// <param name="withResults">Get Smart Contract results</param>
        /// <returns><see cref="TransactionDataResponseDto"/></returns>
        Task<TransactionDataResponseDto> GetTransaction(string txHash, bool withResults = false);
        /// <summary>
        /// This endpoint allows one to query the status of a Transaction.
        /// </summary>
        /// <param name="txHash">The transaction hash</param>
        /// <returns><see cref="TransactionStatusResponseDto"/></returns>
        Task<TransactionStatusResponseDto> GetTransactionStatus(string txHash);
        /// <summary>
        /// This endpoint allows one to query the status of a Transaction.
        /// </summary>
        /// <param name="txHash">The transaction hash</param>
        /// <returns><see cref="TransactionStatusResponseDto"/></returns>
        Task<TransactionStatusResponseDto> GetTransactionProcessStatus(string txHash);
        /// <summary>
        /// This endpoint allows one to query the details of a Transaction.
        /// </summary>
        /// <param name="txHash">The transaction hash</param>
        /// <param name="withResults">Get Smart Contract results</param>
        /// <returns>Your custom Transaction object</returns>
        Task<Transaction> GetTransaction<Transaction>(string txHash, bool withResults = false);

        /// <summary>
        /// This endpoint allows one to query the pool of transactions.
        /// </summary>
        /// <param name="shardId">The shard id</param>
        /// <param name="fields">Fields to get</param>
        /// <returns>List of transactions in pool</returns>
        Task<TransactionPoolResponseDto> GetTransactionsPool(long? shardId = null, string? fields = null);
    }
}
