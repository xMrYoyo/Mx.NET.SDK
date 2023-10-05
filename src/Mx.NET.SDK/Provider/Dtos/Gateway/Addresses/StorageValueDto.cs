using Mx.NET.SDK.Core.Domain.Helper;

namespace Mx.NET.SDK.Provider.Dtos.Gateway.Addresses
{
    public class StorageValueDto
    {
        public StorageValueDto(string value)
        {
            Value = Converter.FromHexToUtf8(value);
        }

        public string Value { get; set; }
    }
}
