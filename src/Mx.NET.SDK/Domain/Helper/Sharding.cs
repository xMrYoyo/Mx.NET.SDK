
using Mx.NET.SDK.Core.Domain.Values;
using System;
using System.Linq;

namespace Mx.NET.SDK.Domain.Helper
{
    public static class Sharding
    {
        public static long ComputeShard(Address address)
        {
            var numShards = 3;
            var maskHigh = Convert.ToInt32("11", 2);
            var maskLow = Convert.ToInt32("01", 2);
            var pubKey = address.PublicKey();
            var lastByteOfPubKey = pubKey[31];
            if (IsAddressOfMetachain(address))
            {
                return 4294967295;
            }
            var shard = lastByteOfPubKey & maskHigh;

            if (shard > numShards - 1)
            {
                shard = lastByteOfPubKey & maskLow;
            }

            return shard;
        }

        public static bool IsAddressOfMetachain(Address address)
        {
            var pubKey = address.PublicKey();
            byte[] metachainPrefix = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, };
            var pubKeyPrefix = pubKey.Skip(metachainPrefix.Length);

            if (pubKeyPrefix.Equals(metachainPrefix))
            {
                return true;
            }

            var zeroAddress = new byte[32];

            if (pubKey.Equals(zeroAddress))
            {
                return true;
            }

            return false;
        }

    }
}