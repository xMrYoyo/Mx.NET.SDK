using Mx.NET.SDK.Configuration;
using System;

namespace Mx.NET.SDK.Domain.Helper
{
    public static class NetworkConverter
    {
        /// <summary>
        /// Converts Network enum to Chain Id string
        /// </summary>
        /// <param name="network"></param>
        /// <returns>ChainId</returns>
        public static string ToChainId(this Network network)
        {
            string result;
            switch (network)
            {
                case Network.MainNet: result = "1"; break;
                case Network.TestNet: result = "T"; break;
                case Network.DevNet: result = "D"; break;
                default: throw new Exception($"{network} Invalid");
            };

            return result;
        }
        /// <summary>
        /// Converts ChainId string to Network
        /// </summary>
        /// <param name="chainId"></param>
        /// <returns>Network</returns>
        public static Network ToNetwork(this string chainId)
        {
            if (chainId == null) throw new Exception($"{chainId} Invalid");

            if (chainId == "1") return Network.MainNet;
            if (chainId == "T") return Network.TestNet;
            if (chainId == "D") return Network.DevNet;

            throw new Exception($"{chainId} Invalid");
        }
    }
}
