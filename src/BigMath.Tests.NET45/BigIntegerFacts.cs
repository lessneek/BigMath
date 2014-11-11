// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BigIntegerFacts.cs">
//   Copyright (c) 2014 Alexander Logger. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Globalization;
using BigMath.Utils;
using FluentAssertions;
using NUnit.Framework;

namespace BigMath.Tests
{
    [TestFixture]
    public class BigIntegerFacts
    {
        [TestCase(1, 1, 0)]
        [TestCase(3, 2, 1)]
        [TestCase(1, 2, -1)]
        [TestCase(-1, 1, -2)]
        [TestCase(-1, -2, 1)]
        public void Should_substruct_correctly(int x, int y, int z)
        {
            var i1 = new BigInteger(x);
            var i2 = new BigInteger(y);

            BigInteger i3 = i1 - i2;

            i3.ToString().Should().Be(z.ToString(CultureInfo.InvariantCulture));
        }

        [TestCase("11111111111111111111111111111111", "11111111111111111111111111111111", "00000000000000000000000000000000")]
        [TestCase("11111111111111111111111111111111", "11111111111111111111111111111110", "00000000000000000000000000000001")]
        [TestCase("11111111111111111111111111111110", "11111111111111111111111111111111", "-1")]
        [TestCase("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF", "00000000000000000123456789ABCDEF", "FFFFFFFFFFFFFFFFFEDCBA9876543210")]
        [TestCase("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF", "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF", "00000000000000000000000000000000")]
        [TestCase("0FEDCBA9876543210FEDCBA987654321", "0123456789ABCDEF0123456789ABCDEF", "0ECA8641FDB975320ECA8641FDB97532")]
        public void Should_substruct_big_numbers_correctly(string x, string y, string z)
        {
            var i1 = new BigInteger(x, 16);
            var i2 = new BigInteger(y, 16);
            var expectedResult = new BigInteger(z, 16);

            BigInteger result = i1 - i2;

            result.ShouldBeEquivalentTo(expectedResult);
            result.ToString("X").Should().Be(expectedResult.ToString("X"));
        }

        [TestCase(1, -1, 0)]
        [TestCase(3, 2, 5)]
        [TestCase(1, 2, 3)]
        [TestCase(-1, 1, 0)]
        [TestCase(-1, -2, -3)]
        public void Should_sum_correctly(int x, int y, int z)
        {
            var i1 = (BigInteger) x;
            var i2 = (BigInteger) y;

            BigInteger i3 = i1 + i2;

            ((int) i3).Should().Be(z);
            i3.ToString().Should().Be(z.ToString(CultureInfo.InvariantCulture));
        }

        [TestCase("11111111111111111111111111111111", "11111111111111111111111111111111", "22222222222222222222222222222222")]
        [TestCase("11111111111111111111111111111111", "11111111111111111111111111111110", "22222222222222222222222222222221")]
        [TestCase("11111111111111111111111111111110", "11111111111111111111111111111111", "22222222222222222222222222222221")]
        [TestCase("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF", "00000000000000000123456789ABCDEF", "100000000000000000123456789ABCDEE")]
        [TestCase("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF", "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF", "1FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFE")]
        [TestCase("0FEDCBA9876543210FEDCBA987654321", "0123456789ABCDEF0123456789ABCDEF", "11111111111111101111111111111110")]
        public void Should_sum_big_numbers_correctly(string x, string y, string z)
        {
            var i1 = new BigInteger(x, 16);
            var i2 = new BigInteger(y, 16);

            BigInteger i3 = i1 + i2;

            i3.ToString("X32").Should().Be(z);
        }

        [TestCase(1, -1, -1)]
        [TestCase(3, 2, 6)]
        [TestCase(1, 2, 2)]
        [TestCase(-1, 1, -1)]
        [TestCase(-1, -2, 2)]
        public void Should_multiply_correctly(int x, int y, int z)
        {
            var i1 = (BigInteger) x;
            var i2 = (BigInteger) y;

            BigInteger i3 = i1*i2;

            ((int) i3).Should().Be(z);
            i3.ToString().Should().Be(z.ToString(CultureInfo.InvariantCulture));
        }

        [TestCase("11111111111111111111111111111111", "11111111111111111111111111111111", "123456789ABCDF0123456789ABCDF0120FEDCBA987654320FEDCBA987654321")]
        [TestCase("11111111111111111111111111111111", "11111111111111111111111111111110", "123456789ABCDF0123456789ABCDF010FEDCBA987654320FEDCBA9876543210")]
        [TestCase("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF", "00000000000000000123456789ABCDEF", "123456789ABCDEEFFFFFFFFFFFFFFFFFEDCBA9876543211")]
        [TestCase("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF", "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF", "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFE00000000000000000000000000000001")]
        [TestCase("0FEDCBA9876543210FEDCBA987654321", "0123456789ABCDEF0123456789ABCDEF", "121FA00AD77D742247ACC9140513B74458FAB20783AF1222236D88FE5618CF")]
        [TestCase("FFFFFFFFFFFFFFF279EFEC87FED69879", "0000000000000000000000011CA789F3", "11CA789F2FFFFFFF0F66C6C554FBF9B9B703A7BDB")]
        [TestCase("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF", "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF",
            "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFE0000000000000000000000000000000000000000000000000000000000000001")]
        public void Should_multiply_big_numbers_correctly(string x, string y, string z)
        {
            var i1 = new BigInteger(x, 16);
            var i2 = new BigInteger(y, 16);

            BigInteger i3 = i1*i2;

            i3.ToString("X").Should().Be(z);
        }

        [TestCase(1, -1, -1)]
        [TestCase(4, 2, 2)]
        [TestCase(2, 2, 1)]
        [TestCase(-1, 1, -1)]
        [TestCase(-4, -2, 2)]
        [TestCase(1000, -50, -20)]
        public void Should_divide_correctly(int x, int y, int z)
        {
            var i1 = (BigInteger) x;
            var i2 = (BigInteger) y;

            BigInteger i3 = i1/i2;

            ((int) i3).Should().Be(z);
            i3.ToString().Should().Be(z.ToString(CultureInfo.InvariantCulture));
        }

        [TestCase("11111111111111111111111111111111", "11111111111111111111111111111111", "00000000000000000000000000000001")]
        [TestCase("11111111111111111111111111111111", "00000000000000000000000111111110", "00000000100000001000000010000000")]
        [TestCase("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF", "00000000000000000123456789ABCDEF", "00000000000000E1000000000000D3D1")]
        [TestCase("7FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF", "00000000000000000123456789ABCDEF", "000000000000007080000000000069E8")]
        [TestCase("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF", "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF", "00000000000000000000000000000001")]
        [TestCase("0FEDCBA9876543210FEDCBA987654321", "0123456789ABCDEF0123456789ABCDEF", "0000000000000000000000000000000E")]
        [TestCase("59FE45F3CACCE58279EFEC87FED69879", "00000000000000000937F5E11CA789F3", "0000000000000009C31BD9DCA1E0BEDC")]
        public void Should_devide_big_numbers_correctly(string x, string y, string z)
        {
            var i1 = new BigInteger(x, 16);
            var i2 = new BigInteger(y, 16);

            BigInteger i3 = i1/i2;

            i3.ToString("X32").Should().Be(z);
        }

        [TestCase(1, -1, true)]
        [TestCase(4, 2, true)]
        [TestCase(2, 2, null)]
        [TestCase(-1, 1, false)]
        [TestCase(-4, -2, false)]
        [TestCase(1000, -50, true)]
        public void Should_compare_correctly(int x, int y, bool? z)
        {
            // z == null means that numbers are equal.

            var i1 = (BigInteger) x;
            var i2 = (BigInteger) y;

            bool value = z.HasValue && z.Value;

            (i1 > i2).Should().Be(value);
            (i1 < i2).Should().Be(z.HasValue && !value);
            (i1 == i2).Should().Be(!z.HasValue);
            (i1 != i2).Should().Be(z.HasValue);
        }

        [TestCase("11111111111111111111111111111111", "11111111111111111111111111111111", null)]
        [TestCase("11111111111111111111111111111111", "00000000000000000000000111111110", true)]
        [TestCase("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF", "00000000000000000123456789ABCDEF", false)]
        [TestCase("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF", "00000000000000000123456789ABCDEF", false)]
        [TestCase("7FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF", "00000000000000000123456789ABCDEF", true)]
        [TestCase("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF", "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF", null)]
        [TestCase("0FEDCBA9876543210FEDCBA987654321", "0123456789ABCDEF0123456789ABCDEF", true)]
        [TestCase("59FE45F3CACCE58279EFEC87FED69879", "00000000000000000937F5E11CA789F3", true)]
        public void Should_compare_big_numbers_correctly(string x, string y, bool? z)
        {
            // z == null means that numbers are equal.

            var i1 = new BigInteger(x.HexToBytes());
            var i2 = new BigInteger(y.HexToBytes());

            bool value = z.HasValue && z.Value;

            (i1 > i2).Should().Be(value);
            (i1 < i2).Should().Be(z.HasValue && !value);
            (i1 == i2).Should().Be(!z.HasValue);
            (i1 != i2).Should().Be(z.HasValue);
        }

        [TestCase(1001, 10, 2003, 1825)]
        [TestCase(91523, 9, 4234, 173)]
        [TestCase(99593, 99, 9234, 6155)]
        [TestCase(95993, 71, 72993, 3767)]
        public void Should_mod_pow(int x, int e, int m, int expectedResult)
        {
            var bx = new BigInteger(x);
            var bm = new BigInteger(m);
            var be = new BigInteger(e);

            BigInteger result = bx.ModPow(be, bm);
            var actualResult = (int) result;
            actualResult.Should().Be(expectedResult);
        }
    }
}
