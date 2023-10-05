﻿using System;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;

namespace Mx.NET.SDK.Core.Domain.Helper
{
    public static class Converter
    {
        private const byte UnsignedByte = 0x00;

        public static BigInteger ToBigInteger(byte[] bytes, bool isUnsigned = false, bool isBigEndian = false)
        {
            if (isUnsigned)
            {
                if (bytes.FirstOrDefault() != UnsignedByte)
                {
                    var data = new[] { UnsignedByte }.ToList();
                    data.AddRange(bytes);
                    bytes = data.ToArray();
                }
            }

            if (isBigEndian)
            {
                bytes = bytes.Reverse().ToArray();
            }

            return new BigInteger(bytes);
        }

        public static byte[] FromBigInteger(BigInteger bigInteger, bool isUnsigned = false, bool isBigEndian = false)
        {
            var bytes = bigInteger.ToByteArray();
            if (isBigEndian)
            {
                bytes = bytes.Reverse().ToArray();
            }

            if (!isUnsigned)
                return bytes;

            if (bytes.FirstOrDefault() == UnsignedByte)
            {
                bytes = bytes.Slice(1);
            }

            return bytes;
        }

        public static string ToHexString(byte[] bytes)
        {
            var hex = BitConverter
                     .ToString(bytes)
                     .Replace("-", "");

            return hex;
        }

        public static string FromHexToUtf8(string hex)
        {
            return Encoding.UTF8.GetString(FromHexString(hex));
        }

        public static byte[] FromHexString(string hex)
        {
            var bytes = new byte[hex.Length / 2];
            var hexValue = new[] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F };

            for (int x = 0, i = 0; i < hex.Length; i += 2, x += 1)
            {
                bytes[x] = (byte)(hexValue[char.ToUpper(hex[i + 0]) - '0'] << 4 |
                                  hexValue[char.ToUpper(hex[i + 1]) - '0']);
            }

            return bytes;
        }
        public static BigInteger FromHexToBigInt(string hex) => BigInteger.Parse("0" + hex, NumberStyles.AllowHexSpecifier);

        public static string ToHexString(string utf8Value)
        {
            return ToHexString(Encoding.UTF8.GetBytes(utf8Value)).ToLower();
        }

        public static string ToBase64String(string value)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(value));
        }
        public static string FromBase64ToUtf8(string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }
        public static string FromBase64ToHex(string base64EncodedData)
        {
            // Convierte la cadena Base64 a bytes
            byte[] bytes = Convert.FromBase64String(base64EncodedData);

            // Convierte los bytes a una representación hexadecimal
            string hexString = BitConverter.ToString(bytes).Replace("-", "");

            return hexString;
        }
        public static BigInteger FromBase64ToBigInteger(string base64EncodedData)
        {
            var hex = Converter.FromBase64ToHex(base64EncodedData);

            return Converter.FromHexToBigInt(hex);
        }
    }
}
