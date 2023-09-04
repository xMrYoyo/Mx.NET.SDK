using Mx.NET.SDK.Core.Domain.Values;
using System.Numerics;

namespace Mx.NET.SDK.Provider.Dtos.Gateway.Tokens
{
    public class TokenDataDto
    {
        public TokenDto Token { get; set; }
    }
    public class TokenDto
    {
        public string Identifier { get; set; }
        public string Name { get; set; }
        public string Ticker { get; set; }
        public string Owner { get; set; }
        public string Type { get; set; }
        public Address Address { get; set; }
        public string Minted { get; set; }
        public string Burnt { get; set; }
        public BigInteger TotalSupply { get; set; }
        public int Decimals { get; set; }
        public bool IsPaused { get; set; }
        public bool CanUpgrade { get; set; }
        public bool CanMint { get; set; }
        public bool CanBurn { get; set; }
        public bool CanChangeOwner { get; set; }
        public bool CanAddSpecialRoles { get; set; }
        public bool CanPause { get; set; }
        public bool CanFreeze { get; set; }
        public bool CanWipe { get; set; }
    }
}
