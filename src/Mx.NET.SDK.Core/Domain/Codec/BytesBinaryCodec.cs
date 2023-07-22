﻿using Mx.NET.SDK.Core.Domain.Helper;
using Mx.NET.SDK.Core.Domain.Values;
using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;

namespace Mx.NET.SDK.Core.Domain.Codec
{
    public class BytesBinaryCodec : IBinaryCodec
    {
        public string Type => TypeValue.BinaryTypes.Bytes;

        private const int BytesSizeOfU32 = 4;

        public (IBinaryType Value, int BytesLength) DecodeNested(byte[] data, TypeValue type)
        {
            var sizeInBytes = (int)BinaryPrimitives.ReadUInt32BigEndian(data.Take(BytesSizeOfU32).ToArray());
            //var sizeInBytes = (int)BitConverter.ToUInt32(data.Take(BytesSizeOfU32).ToArray(), 0);
            //if (BitConverter.IsLittleEndian)
            //{
            //    sizeInBytes = (int)BitConverter.ToUInt32(BitConverter.GetBytes(sizeInBytes).Reverse().ToArray(), 0);
            //}

            var payload = data.Slice(BytesSizeOfU32, BytesSizeOfU32 + sizeInBytes);

            return (new BytesValue(payload, type), sizeInBytes + 4);
        }

        public IBinaryType DecodeTopLevel(byte[] data, TypeValue type)
        {
            return new BytesValue(data, type);
        }

        public byte[] EncodeNested(IBinaryType value)
        {
            var bytes = value.ValueOf<BytesValue>();
            var buffer = new List<byte>();
            var lengthBytes = BitConverter.GetBytes(bytes.GetLength());
            if (BitConverter.IsLittleEndian)
            {
                lengthBytes = lengthBytes.Reverse().ToArray();
            }

            buffer.AddRange(lengthBytes);
            buffer.AddRange(bytes.Buffer);

            var data = buffer.ToArray();

            return data;
        }

        public byte[] EncodeTopLevel(IBinaryType value)
        {
            var bytes = value.ValueOf<BytesValue>();
            return bytes.Buffer;
        }
    }
}
