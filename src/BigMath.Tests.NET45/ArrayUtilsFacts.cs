// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ArrayUtilsFacts.cs">
//   Copyright (c) 2013 Alexander Logger. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BigMath.Utils;
using NUnit.Framework;

namespace BigMath.Tests
{
    using System;

    [TestFixture]
    public class ArrayUtilsFacts
    {
        [TestCaseSource("ByteArrayToHexStringTestCases")]
        public string Should_convert_bytes_to_hex_string(HexBytes hexBytes)
        {
            return hexBytes.Bytes.ToHexString(hexBytes.Caps, hexBytes.Min, hexBytes.SpaceEveryByte, hexBytes.TrimZeros);
        }

        [TestCaseSource("HexStringToByteArrayTestCases")]
        public byte[] Should_convert_hex_string_to_array_of_bytes(string str)
        {
            return str.HexToBytes();
        }

        [TestCase(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }, 0, 3, ExpectedResult = "010203")]
        [TestCase(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }, 2, 3, ExpectedResult = "030405")]
        [TestCase(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }, 6, 3, ExpectedResult = "070809")]
        public string Shoudl_convert_bytes_array_segment_to_hex_string(byte[] bytes, int offset, int count)
        {
            return new ArraySegment<byte>(bytes, offset, count).ToHexString();
        }

        private static IEnumerable HexStringToByteArrayTestCases
        {
            get { return GetHexStringToByteArrayTestCasesData.Select(hexBytes => new TestCaseData(hexBytes.Hex).SetCategory(hexBytes.Hex).Returns(hexBytes.Bytes)); }
        }

        private static IEnumerable ByteArrayToHexStringTestCases
        {
            get { return GetByteArrayToHexStringTestCasesData.Select(hexBytes => new TestCaseData(hexBytes).Returns(hexBytes.Hex)); }
        }

        private static IEnumerable<HexBytes> GetByteArrayToHexStringTestCasesData
        {
            get
            {
                yield return new HexBytes(string.Empty, new byte[0]);
                yield return new HexBytes("00", new byte[] {0});
                yield return new HexBytes("0000", new byte[] {0, 0});
                yield return new HexBytes("000000", new byte[] {0, 0, 0});
                yield return new HexBytes("2dafdf", new byte[] {0x2D, 0xAF, 0xDF}, false);
                yield return new HexBytes("2DAFDF", new byte[] {0x2D, 0xAF, 0xDF});
                yield return new HexBytes("0DAFDF", new byte[] {0x0D, 0xAF, 0xDF});
                yield return new HexBytes("DAFDF", new byte[] {0x0D, 0xAF, 0xDF}, trimZeros: true);
                yield return new HexBytes("00DAF2F", new byte[] {0x0D, 0xAF, 0x2F}, min: 7, trimZeros: true);
                yield return new HexBytes("000DAF2F", new byte[] {0x0D, 0xAF, 0x2F}, min: 8, trimZeros: true);
                yield return new HexBytes("0000DAF2F", new byte[] {0x0D, 0xAF, 0x2F}, min: 9, trimZeros: true);
                yield return new HexBytes("DAF2F", new byte[] {0x0D, 0xAF, 0x2F}, min: 1, trimZeros: true);
                yield return
                    new HexBytes("000102030405060708090A0B0C0D0E0F101112131415161718191A1B1C1D1E1F", Enumerable.Range(1, 31).Select(i => (byte) i).ToArray(), true, 64);
                yield return new HexBytes("000102030405060708090A0B0C0D0E0F", Enumerable.Range(1, 15).Select(i => (byte) i).ToArray(), true, 32);
                yield return new HexBytes("00 01 02 03 04 05 06 07 08 09", Enumerable.Range(1, 9).Select(i => (byte) i).ToArray(), true, 20, true);
            }
        }

        private static IEnumerable<HexBytes> GetHexStringToByteArrayTestCasesData
        {
            get
            {
                yield return new HexBytes(string.Empty, new byte[0]);
                yield return new HexBytes("0", new byte[] {0});
                yield return new HexBytes("0x0", new byte[] {0});
                yield return new HexBytes("0x00", new byte[] {0});
                yield return new HexBytes("0x0000", new byte[] {0, 0});
                yield return new HexBytes("0x00000", new byte[] {0, 0, 0});
                yield return new HexBytes("0x000000", new byte[] {0, 0, 0});
                yield return new HexBytes("0x010203040506070809", Enumerable.Range(1, 9).Select(i => (byte) i).ToArray());
                yield return new HexBytes("0x0102030405060708090A0B0C0D0E0F", Enumerable.Range(1, 15).Select(i => (byte) i).ToArray());
                yield return new HexBytes("0x0102030405060708090A0B0C0D0E0F101112131415161718191A1B1C1D1E1F", Enumerable.Range(1, 31).Select(i => (byte) i).ToArray());
                yield return new HexBytes("0x102030405060708090A0B0C0D0E0F101112131415161718191A1B1C1D1E1F", Enumerable.Range(1, 31).Select(i => (byte) i).ToArray());
                yield return new HexBytes("0xc6aeda78b0", new byte[] {0xC6, 0xAE, 0xDA, 0x78, 0xB0});
                yield return new HexBytes("c6aeda78b0", new byte[] {0xC6, 0xAE, 0xDA, 0x78, 0xB0});
                yield return new HexBytes("0xc6aeda78b", new byte[] {0x0c, 0x6A, 0xED, 0xA7, 0x8B});
                yield return new HexBytes("c6aeda78b", new byte[] {0x0c, 0x6A, 0xED, 0xA7, 0x8B});
                yield return new HexBytes("0xC6AEDA78B", new byte[] {0x0c, 0x6A, 0xED, 0xA7, 0x8B});
                yield return new HexBytes("0xC6aEDA78b", new byte[] {0x0c, 0x6A, 0xED, 0xA7, 0x8B});
                yield return new HexBytes("C6AEDA78B", new byte[] {0x0c, 0x6A, 0xED, 0xA7, 0x8B});
                yield return new HexBytes("c6AEda78B", new byte[] {0x0c, 0x6A, 0xED, 0xA7, 0x8B});
                yield return new HexBytes("0xda", new byte[] {0xDA});
                yield return new HexBytes("0xd0", new byte[] {0xD0});
                yield return new HexBytes("0xDA", new byte[] {0xDA});
                yield return new HexBytes("0x0a", new byte[] {0x0A});
            }
        }

        public class HexBytes
        {
            public HexBytes(string hex, byte[] bytes, bool caps = true, int min = 0, bool spaceEveryByte = false, bool trimZeros = false)
            {
                Hex = hex;
                Bytes = bytes;
                Caps = caps;
                Min = min;
                SpaceEveryByte = spaceEveryByte;
                TrimZeros = trimZeros;
            }

            public string Hex { get; set; }
            public byte[] Bytes { get; set; }

            public bool Caps { get; set; }
            public int Min { get; set; }
            public bool SpaceEveryByte { get; set; }
            public bool TrimZeros { get; set; }
        }
    }
}
