// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExtendedBitConverterFacts.cs">
//   Copyright (c) 2013 Alexander Logger. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Linq;
using BigMath.Utils;
using FluentAssertions;
using NUnit.Framework;

namespace BigMath.Tests
{
    [TestFixture]
    public class ExtendedBitConverterFacts
    {
        private const string Int128Value = "0x000102030405060708090A0B0C0D0E0F";
        private const string Int128LessValue = "0x001020304";
        private const string Int128ValueLittleEndian = "0x0F0E0D0C0B0A09080706050403020100";
        private const string Int128LessValueLittleEndian = "0x403020100";

        [TestCase("0x000000000000", "0x0", "0x0")]
        [TestCase("0x000001020304", "0x01020304", "0x000001020304")]
        [TestCase("0x000102030400", "0x0102030400", "0x0001020304")]
        [TestCase("0x010203040000", "0x010203040000", "0x01020304")]
        public void Should_trim_zeros(string bytesS, string expectedTrimmedBigEndianBytesS, string expectedTrimmedLittleEndianBytesS)
        {
            byte[] bytes = bytesS.HexToBytes();
            byte[] expectedTrimmedBigEndianBytes = expectedTrimmedBigEndianBytesS.HexToBytes();
            byte[] expectedTrimmedLittleEndianBytes = expectedTrimmedLittleEndianBytesS.HexToBytes();

            byte[] actualTrimmedBigEndianBytes = bytes.TrimZeros(false);
            byte[] actualTrimmedLittleEndianBytes = bytes.TrimZeros(true);

            actualTrimmedBigEndianBytes.Should().BeEquivalentTo(expectedTrimmedBigEndianBytes);
            actualTrimmedLittleEndianBytes.Should().BeEquivalentTo(expectedTrimmedLittleEndianBytes);
        }

        [Test]
        public void Should_convert_bytes_to_int128()
        {
            Int128 expectedBigEndian = Int128.Parse(Int128Value);
            Int128 expectedLittleEndian = Int128.Parse(Int128ValueLittleEndian);

            byte[] bytes = Int128Value.HexToBytes();

            bytes.ToInt128(0, false).Should().Be(expectedBigEndian);
            bytes.ToInt128(0, true).Should().Be(expectedLittleEndian);
        }

        [Test]
        public void Should_convert_bytes_to_int128_less_than_size_of_int128()
        {
            Int128 expectedBigEndian = Int128.Parse(Int128LessValue);
            Int128 expectedLittleEndian = Int128.Parse(Int128LessValueLittleEndian);

            byte[] bytes = Int128LessValue.HexToBytes();

            Int128 actualBigEndian = bytes.ToInt128(0, false);
            actualBigEndian.Should().Be(expectedBigEndian);

            Int128 actualLittleEndian = bytes.ToInt128(0, true);
            actualLittleEndian.Should().Be(expectedLittleEndian);
        }

        [Test]
        public void Should_convert_int128_to_bytes()
        {
            Int128 i = Int128.Parse(Int128Value);
            byte[] expectedBytes = Int128Value.HexToBytes();

            byte[] actualBytes = i.ToBytes(false);
            actualBytes.ShouldAllBeEquivalentTo(expectedBytes);

            actualBytes = i.ToBytes(true);
            actualBytes.ShouldAllBeEquivalentTo(expectedBytes.Reverse());
        }
    }
}
