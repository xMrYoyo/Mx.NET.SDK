using Mx.NET.SDK.Core.Domain.Helper;
using System.Collections.Generic;
using System.Linq;

namespace Mx.NET.SDK.Provider.Dtos.Gateway.Addresses
{
    public class AllStorageDto
    {
        public AllStorageDto(Dictionary<string, string> pairs)
        {
            Pairs = pairs.ToDictionary(
                kvp => Converter.FromHexToUtf8(kvp.Key),
                kvp => Converter.FromHexToUtf8(kvp.Value));
        }

        public Dictionary<string, string> Pairs { get; set; }
    }
}
