namespace Mx.NET.SDK.Provider.Dtos.Gateway.Transactions
{
    public class TransactionPoolResponseDto
    {
        public TransactionPoolDto TxPool { get; set; }
    }
    public class TransactionPoolDto
    {
        public TransactionFieldsDto[] regularTransactions { get; set; }
        public TransactionFieldsDto[] smartContractResults { get; set; }
    }
    public class TransactionFieldsDto
    {
        public TransactionDto txFields { get; set; }
    }
}
