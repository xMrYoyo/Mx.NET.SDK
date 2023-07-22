
using Mx.NET.SDK.Core.Domain.Values;

namespace Mx.NET.SDK.Core.Domain.Abi
{
    public class EventDefinition
    {
        public string Name { get; }
        public FieldDefinition[] Input { get; }

        public EventDefinition(string name, FieldDefinition[] input)
        {
            Name = name;
            Input = input;
        }
    }
}
