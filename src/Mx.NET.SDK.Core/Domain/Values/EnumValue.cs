using Mx.NET.SDK.Core.Domain.Helper;
using System.Collections.Generic;

namespace Mx.NET.SDK.Core.Domain.Values
{
    public class EnumValue : BaseBinaryValue
    {
        public EnumField[] Variants { get; }

        public EnumValue(TypeValue enumType, EnumField[] variants) : base(enumType)
        {
            Variants = variants;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.AppendLine(Type.Name);
            foreach (var enumVariant in Variants)
            {
                builder.AppendLine($"{enumVariant.Name}:{enumVariant.Discriminant}");
            }

            return builder.ToString();
        }

        public override T ToObject<T>()
        {
            return JsonSerializerWrapper.Deserialize<T>(ToJson());
        }

        public override string ToJson()
        {
            var dict = new Dictionary<string, object>();
            foreach (var variant in Variants)
            {
                dict.Add(variant.Name, variant.Discriminant.ToJson());
            }

<<<<<<< HEAD
            return JsonSerializerWrapper.Serialize(dic);
=======
            return JsonUnqtWrapper.Serialize(dict);
>>>>>>> 5b6b03104aa5cb630661d10eabcd57e615c76864
        }
    }
}
