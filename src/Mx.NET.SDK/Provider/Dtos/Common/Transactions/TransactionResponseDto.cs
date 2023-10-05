using System;
using System.Threading.Tasks;

namespace Mx.NET.SDK.Provider.Dtos.Common.Transactions
{
    public class TransactionResponseDto
    {
        public string TxHash { get; set; }

        protected async Task<string> Sync(GatewayProvider provider)
        {
            var transaction = await provider.GetTransactionStatus(TxHash);
            return transaction.Status;
        }

        /// <summary>
        /// Wait for the execution of the transaction
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public async Task<bool> AwaitExecuted(GatewayProvider provider, TimeSpan? timeout = null)
        {
            if (!timeout.HasValue) timeout = TimeSpan.FromSeconds(60);
            
            var isExecuted = false;
            var currentIteration = 0;

            do
            {
                await Task.Delay(1000); // 1 second
                var status = await Sync(provider);
                isExecuted = status == "success";

                currentIteration++;

            } while (!isExecuted && currentIteration < timeout.Value.TotalSeconds);

            return isExecuted;
        }
    }
}
