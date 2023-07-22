using Mx.NET.SDK.Core.Domain.Values;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Mx.NET.SDK.Core.Domain.Abi
{
    public class AbiDefinition
    {
        public string Name { get; set; }
        public Abi.Endpoint[] Endpoints { get; set; }
        public Dictionary<string, Abi.CustomTypes> Types { get; set; }
        public Abi.Events[] Events { get; set; }

        public EndpointDefinition GetEndpointDefinition(string endpoint)
        {
            var data = Endpoints.ToList().SingleOrDefault(s => s.Name == endpoint);
            if (data == null)
                throw new Exception("Endpoint is not define in ABI");

            var inputs = data.Inputs == null
                ? new List<FieldDefinition>()
                : data.Inputs.Select(i => new FieldDefinition(i.Name, "", GetTypeValue(i.Type))).ToList();

            var outputs = data.Outputs == null
                ? new List<FieldDefinition>()
                : data.Outputs.Select(i => new FieldDefinition("", "", GetTypeValue(i.Type))).ToList();

            return new EndpointDefinition(endpoint, inputs.ToArray(), outputs.ToArray());
        }
        public EventDefinition GetEventDefinition(string identifier)
        {
            var data = Events.ToList().SingleOrDefault(s => s.Identifier == identifier);
            if (data == null)
                throw new Exception("Endpoint is not define in ABI");

<<<<<<< HEAD
            var inputs = data.Inputs.Select(i => new FieldDefinition(i.Name, "", GetTypeValue(i.Type))).ToList();
            return new EventDefinition(identifier, inputs.ToArray());
        }
        public TypeValue GetTypeValue(string rustType)
=======
        private TypeValue GetTypeValue(string rustType)
>>>>>>> 5b6b03104aa5cb630661d10eabcd57e615c76864
        {
            var optional = new Regex("^optional<(.*)>$");
            var option = new Regex("^Option<(.*)>$");
            var multi = new Regex("^multi<(.*)>$");
            var variadic = new Regex("^variadic<(.*)>$");
            var list = new Regex("^List<(.*)>$"); // cambio a array en vez de multi
            var tuple = new Regex("^tuple<(.*)>$");

            if (optional.IsMatch(rustType))
            {
                var innerType = optional.Match(rustType).Groups[1].Value;
                var innerTypeValue = GetTypeValue(innerType);
                return TypeValue.OptionValue(innerTypeValue);
            }
            if (option.IsMatch(rustType))
            {
                var innerType = option.Match(rustType).Groups[1].Value;
                var innerTypeValue = GetTypeValue(innerType);
                return TypeValue.OptionValue(innerTypeValue);
            }
            if (variadic.IsMatch(rustType))
            {
                var innerType = variadic.Match(rustType).Groups[1].Value;
                var innerTypeValue = GetTypeValue(innerType);
                return TypeValue.ArrayValue(innerTypeValue);
            }
            if (list.IsMatch(rustType))
            {
                var innerTypes = list.Match(rustType).Groups[1].Value.Split(',').Where(s => !string.IsNullOrEmpty(s));
                var innerTypeValues = innerTypes.Select(GetTypeValue).ToArray();
                return TypeValue.ListValue(innerTypeValues[0]);
            }
            if (list.IsMatch(rustType))
            {
                var innerTypes = list.Match(rustType).Groups[1].Value.Split(',').Where(s => !string.IsNullOrEmpty(s));
                var innerTypeValues = innerTypes.Select(GetTypeValue).ToArray();
                return TypeValue.ArrayValue(innerTypeValues[0]);
            }
            if (multi.IsMatch(rustType))
            {
                var innerTypes = multi.Match(rustType).Groups[1].Value.Split(',').Where(s => !string.IsNullOrEmpty(s));
                var innerTypeValues = innerTypes.Select(GetTypeValue).ToArray();
                return TypeValue.MultiValue(innerTypeValues);
            }
            if (tuple.IsMatch(rustType))
            {
                var innerTypes = tuple.Match(rustType).Groups[1].Value.Split(',').Where(s => !string.IsNullOrEmpty(s));
                var innerTypeValues = innerTypes.Select(GetTypeValue).ToArray();
                return TypeValue.MultiValue(innerTypeValues);
            }
<<<<<<< HEAD
=======

            if (variadic.IsMatch(rustType))
            {
                var innerType = variadic.Match(rustType).Groups[1].Value;
                var innerTypeValue = GetTypeValue(innerType);
                return TypeValue.VariadicValue(innerTypeValue);
            }

            if (list.IsMatch(rustType))
            {
                var innerType = list.Match(rustType).Groups[1].Value;
                var innerTypeValue = GetTypeValue(innerType);
                return TypeValue.ListValue(innerTypeValue);
            }
            if (array.IsMatch(rustType))
            {
                var innerType = list.Match(rustType).Groups[1].Value;
                var innerTypeValue = GetTypeValue(innerType);
                return TypeValue.ArrayValue(innerTypeValue);
            }

>>>>>>> 5b6b03104aa5cb630661d10eabcd57e615c76864
            var typeFromBaseRustType = TypeValue.FromRustType(rustType);
            if (typeFromBaseRustType != null)
                return typeFromBaseRustType;

            if (Types.Keys.Contains(rustType))
            {
                var typeFromStruct = Types[rustType];
                if (typeFromStruct.Type == "enum")
                {
                    return TypeValue.EnumValue(
                       typeFromStruct.Type,
                       typeFromStruct.Variants?
                       .ToList()
                       .Select(c => new FieldDefinition(c.Name, "", GetTypeValue(TypeValue.FromRustType("Enum").RustType)))
                       .ToArray());
                }
                else if (typeFromStruct.Type == "struct")
                {
                    return TypeValue.StructValue(
                        typeFromStruct.Type,
                        typeFromStruct.Fields?
                        .ToList()
                        .Select(c => new FieldDefinition(c.Name, "", GetTypeValue(c.Type)))
                        .ToArray());

                }
<<<<<<< HEAD

=======
>>>>>>> 5b6b03104aa5cb630661d10eabcd57e615c76864
            }

            return null;
        }

        public static AbiDefinition FromJson(string json)
        {
            return Helper.JsonSerializerWrapper.Deserialize<AbiDefinition>(json);
        }

        public static AbiDefinition FromFilePath(string jsonFilePath)
        {
            var fileBytes = File.ReadAllBytes(jsonFilePath);
            var json = Encoding.UTF8.GetString(fileBytes);
            return FromJson(json);
        }
    }
}
