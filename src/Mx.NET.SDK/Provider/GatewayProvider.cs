using Mx.NET.SDK.Configuration;
using Mx.NET.SDK.Core.Domain.Helper;
using Mx.NET.SDK.Core.Domain.Values;
using Mx.NET.SDK.Domain.Exceptions;
using Mx.NET.SDK.Provider.Dtos.Common.QueryVm;
using Mx.NET.SDK.Provider.Dtos.Common.Transactions;
using Mx.NET.SDK.Provider.Dtos.Gateway;
using Mx.NET.SDK.Provider.Dtos.Gateway.Addresses;
using Mx.NET.SDK.Provider.Dtos.Gateway.Blocks;
using Mx.NET.SDK.Provider.Dtos.Gateway.ESDTs;
using Mx.NET.SDK.Provider.Dtos.Gateway.Network;
using Mx.NET.SDK.Provider.Dtos.Gateway.Tokens;
using Mx.NET.SDK.Provider.Dtos.Gateway.Transactions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Mx.NET.SDK.Provider
{
    public class GatewayProvider : IGatewayProvider
    {
        private readonly HttpClient _httpGatewayClient;
        public GatewayNetworkConfiguration NetworkConfiguration { get; }

        public GatewayProvider(GatewayNetworkConfiguration configuration, Dictionary<string, string> extraRequestHeaders = null)
        {
            NetworkConfiguration = configuration;

            _httpGatewayClient = new HttpClient
            {
                BaseAddress = configuration.GatewayUri
            };
            if (extraRequestHeaders != null)
            {
                foreach (var header in extraRequestHeaders)
                    _httpGatewayClient.DefaultRequestHeaders.Add(header.Key, header.Value);
            }
        }

        #region generic
        public async virtual Task<TR> Get<TR>(string requestUri)
        {
            var uri = requestUri.StartsWith("/") ? requestUri.Substring(1) : requestUri;
            var response = await _httpGatewayClient.GetAsync($"{uri}");
            var content = await response.Content.ReadAsStringAsync();

            if (response.StatusCode == HttpStatusCode.NotFound)
                throw new APIException(content);

            var result = JsonWrapper.Deserialize<GatewayResponseDto<TR>>(content);
            result.EnsureSuccessStatusCode();
            return result.Data;
        }

        public async virtual Task<TR> Post<TR>(string requestUri, object requestContent)
        {
            var uri = requestUri.StartsWith("/") ? requestUri.Substring(1) : requestUri;
            var raw = JsonWrapper.Serialize(requestContent);
            var payload = new StringContent(raw, Encoding.UTF8, "application/json");
            var response = await _httpGatewayClient.PostAsync(uri, payload);

            var content = await response.Content.ReadAsStringAsync();
            if (response.StatusCode == HttpStatusCode.NotFound)
                throw new APIException(content);

            var result = JsonWrapper.Deserialize<GatewayResponseDto<TR>>(content);
            result.EnsureSuccessStatusCode();
            return result.Data;
        }
        #endregion

        #region addresses

        public async Task<AddressDataDto> GetAddress(string address)
        {
            return await Get<AddressDataDto>($"address/{address}");
        }

        public async Task<AddressGuardianDataDto> GetAddressGuardianData(string address)
        {
            return await Get<AddressGuardianDataDto>($"address/{address}/guardian-data");
        }

        public async Task<StorageValueDto> GetStorageValue(string address, string key, bool isHex = false)
        {
            if (!isHex) key = Converter.ToHexString(key);

            return await Get<StorageValueDto>($"address/{address}/key/{key}");
        }

        public async Task<AllStorageDto> GetAllStorageValues(string address)
        {
            return await Get<AllStorageDto>($"address/{address}/keys");
        }

        #endregion

        #region esdt

        public async Task<EsdtTokenDataDto> GetEsdtTokens(string address)
        {
            return await Get<EsdtTokenDataDto>($"address/{address}/esdt");
        }

        public async Task<EsdtTokenData> GetEsdtToken(string address, string tokenIdentifier)
        {
            return await Get<EsdtTokenData>($"address/{address}/esdt/{tokenIdentifier}");
        }
        public async Task<TokenDataDto> GetToken(string tokenIdentifier)
        {
            var query = await QueryVm(new QueryVmRequestDto()
            {
                ScAddress = "erd1qqqqqqqqqqqqqqqpqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqzllls8a5w6u",
                FuncName = "getTokenProperties",
                Args = new string[]
                {
                   Converter.ToHexString(tokenIdentifier)
                }
            });

            var token = new TokenDataDto() { Token = new TokenDto() { Identifier = tokenIdentifier } };

            if (query.Data.ReturnData.Length > 0)
            {
                token.Token.Name = Converter.FromBase64ToUtf8(query.Data.ReturnData[0]);
                token.Token.Type = Converter.FromBase64ToUtf8(query.Data.ReturnData[1]);
                token.Token.Address = Address.FromBytes(Convert.FromBase64String(query.Data.ReturnData[2]));
                token.Token.TotalSupply = Converter.FromBase64ToBigInteger(query.Data.ReturnData[3]);
                token.Token.Burnt = Converter.FromBase64ToUtf8(query.Data.ReturnData[4]);
                token.Token.Decimals = int.Parse(Converter.FromBase64ToUtf8(query.Data.ReturnData[5]).Split('-')[1]);
                token.Token.IsPaused = bool.Parse(Converter.FromBase64ToUtf8(query.Data.ReturnData[6]).Split('-')[1]);
                token.Token.CanUpgrade = bool.Parse(Converter.FromBase64ToUtf8(query.Data.ReturnData[7]).Split('-')[1]);
                token.Token.CanMint = bool.Parse(Converter.FromBase64ToUtf8(query.Data.ReturnData[8]).Split('-')[1]);
                token.Token.CanBurn = bool.Parse(Converter.FromBase64ToUtf8(query.Data.ReturnData[9]).Split('-')[1]);
                token.Token.CanChangeOwner = bool.Parse(Converter.FromBase64ToUtf8(query.Data.ReturnData[10]).Split('-')[1]);
                token.Token.CanPause = bool.Parse(Converter.FromBase64ToUtf8(query.Data.ReturnData[11]).Split('-')[1]);
                token.Token.CanFreeze = bool.Parse(Converter.FromBase64ToUtf8(query.Data.ReturnData[12]).Split('-')[1]);
            }
            else
            {

            }

            return token;
        }

        #endregion

        #region transactions

        public async Task<TransactionResponseDto> SendTransaction(TransactionRequestDto transactionRequest)
        {
            return await Post<TransactionResponseDto>("transaction/send", transactionRequest);
        }

        public async Task<MultipleTransactionsResponseDto> SendTransactions(TransactionRequestDto[] transactionsRequest)
        {
            return await Post<MultipleTransactionsResponseDto>("transaction/send-multiple", transactionsRequest);
        }

        public async Task<TransactionCostResponseDto> GetTransactionCost(TransactionRequestDto transactionRequestDto)
        {
            return await Post<TransactionCostResponseDto>("transaction/cost", transactionRequestDto);
        }

        public async Task<TransactionDataResponseDto> GetTransaction(string txHash, bool withResults = false)
        {
            return await GetTransaction<TransactionDataResponseDto>(txHash, withResults);
        }
        public async Task<TransactionStatusResponseDto> GetTransactionStatus(string txHash)
        {
            return await Get<TransactionStatusResponseDto>($"transaction/{txHash}/status");
        }
        public async Task<TransactionStatusResponseDto> GetTransactionProcessStatus(string txHash)
        {
            return await Get<TransactionStatusResponseDto>($"transaction/{txHash}/process-status");
        }

        public async Task<Transaction> GetTransaction<Transaction>(string txHash, bool withResults = false)
        {
            return await Get<Transaction>($"transaction/{txHash}?withResults={withResults}");
        }
        
        public async Task<TransactionPoolResponseDto> GetTransactionsPool(long? shardId = null, string fields = null)
        {
            var shardIdQuery = shardId == null ? "" : $"shard-id={shardId}&";
            var fieldsQuery = fields == null ? "" : $"fields={fields}";

            return await Get<TransactionPoolResponseDto>($"transaction/pool/?{shardIdQuery}{fieldsQuery}");
        }
        #endregion

        #region network

        public async Task<NetworkConfigDataDto> GetNetworkConfig()
        {
            return await Get<NetworkConfigDataDto>("network/config");
        }

        public async Task<NetworkEconomicsDataDto> GetNetworkEconomics()
        {
            return await Get<NetworkEconomicsDataDto>("network/economics");
        }

        public async Task<ShardStatusDataDto> GetShardStatus(long? shard = null)
        {
            return await Get<ShardStatusDataDto>($"network/status/{shard}");
        }

        #endregion

        #region nodes


        #endregion

        #region blocks

        public async Task<BlockDataDto> GetBlockByNonce(long nonce, long shard, bool withTxs = false)
        {
            return await Get<BlockDataDto>($"/block/by-nonce/{nonce}?withTxs={withTxs}&withResults=true");
        }

        public async Task<BlockDataDto> GetBlockByHash(string hash, long shard, bool withTxs = false)
        {
            return await Get<BlockDataDto>($"/block/{shard}/by-hash/{hash}?withTxs={withTxs}");
        }

        public async Task<InternalBlockResponseDto> GetInternalBlockByNonce(long nonce)
        {
            return await Get<InternalBlockResponseDto>($"/internal/json/shardblock/by-nonce/{nonce}");
        }

        public async Task<InternalBlockResponseDto> GetInternalBlockByHash(string hash)
        {
            return await Get<InternalBlockResponseDto>($"/internal/json/shardblock/by-hash/{hash}");
        }

        #endregion

        #region queryVM

        public async Task<QueryVmResponseDataDto> QueryVm(QueryVmRequestDto queryVmRequestDto)
        {
            return await Post<QueryVmResponseDataDto>("vm-values/query", queryVmRequestDto);
        }

        #endregion
    }
}