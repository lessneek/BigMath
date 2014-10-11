// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Int256Facts.cs">
//   Copyright (c) 2013 Alexander Logger. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Globalization;
using FluentAssertions;
using NUnit.Framework;

namespace BigMath.Tests
{
    [TestFixture]
    public class Int256Facts
    {
        [TestCase(1, 1, 0)]
        [TestCase(3, 2, 1)]
        [TestCase(1, 2, -1)]
        [TestCase(-1, 1, -2)]
        [TestCase(-1, -2, 1)]
        public void Should_substruct_correctly(int x, int y, int z)
        {
            var i1 = (Int256) x;
            var i2 = (Int256) y;

            Int256 i3 = i1 - i2;

            ((int) i3).Should().Be(z);
            i3.ToString().Should().Be(z.ToString(CultureInfo.InvariantCulture));
        }

        [TestCase("0x11111111111111111111111111111111", "0x11111111111111111111111111111111", "0x00000000000000000000000000000000")]
        [TestCase("0x11111111111111111111111111111111", "0x11111111111111111111111111111110", "0x00000000000000000000000000000001")]
        [TestCase("0x11111111111111111111111111111110", "0x11111111111111111111111111111111", "0xFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF")]
        [TestCase("0xFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF", "0x00000000000000000123456789ABCDEF", "0xFFFFFFFFFFFFFFFFFEDCBA9876543210")]
        [TestCase("0xFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF", "0xFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF", "0x00000000000000000000000000000000")]
        [TestCase("0x0FEDCBA9876543210FEDCBA987654321", "0x0123456789ABCDEF0123456789ABCDEF", "0x0ECA8641FDB975320ECA8641FDB97532")]
        public void Should_substruct_big_numbers_correctly(string x, string y, string z)
        {
            Int256 i1 = Int256.Parse(x);
            Int256 i2 = Int256.Parse(y);

            Int256 i3 = i1 - i2;

            ("0x" + i3.ToString("X32")).Should().Be(z);
        }

        [TestCase(1, -1, 0)]
        [TestCase(3, 2, 5)]
        [TestCase(1, 2, 3)]
        [TestCase(-1, 1, 0)]
        [TestCase(-1, -2, -3)]
        public void Should_sum_correctly(int x, int y, int z)
        {
            var i1 = (Int256) x;
            var i2 = (Int256) y;

            Int256 i3 = i1 + i2;

            ((int) i3).Should().Be(z);
            i3.ToString().Should().Be(z.ToString(CultureInfo.InvariantCulture));
        }

        [TestCase("0x11111111111111111111111111111111", "0x11111111111111111111111111111111", "0x22222222222222222222222222222222")]
        [TestCase("0x11111111111111111111111111111111", "0x11111111111111111111111111111110", "0x22222222222222222222222222222221")]
        [TestCase("0x11111111111111111111111111111110", "0x11111111111111111111111111111111", "0x22222222222222222222222222222221")]
        [TestCase("0xFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF", "0x00000000000000000123456789ABCDEF", "0x100000000000000000123456789ABCDEE")]
        [TestCase("0xFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF", "0xFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF", "0x1FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFE")]
        [TestCase("0x0FEDCBA9876543210FEDCBA987654321", "0x0123456789ABCDEF0123456789ABCDEF", "0x11111111111111101111111111111110")]
        public void Should_sum_big_numbers_correctly(string x, string y, string z)
        {
            Int256 i1 = Int256.Parse(x);
            Int256 i2 = Int256.Parse(y);

            Int256 i3 = i1 + i2;

            ("0x" + i3.ToString("X32")).Should().Be(z);
        }

        [TestCase(1, -1, -1)]
        [TestCase(3, 2, 6)]
        [TestCase(1, 2, 2)]
        [TestCase(-1, 1, -1)]
        [TestCase(-1, -2, 2)]
        public void Should_multiply_correctly(int x, int y, int z)
        {
            var i1 = (Int256) x;
            var i2 = (Int256) y;

            Int256 i3 = i1*i2;

            ((int) i3).Should().Be(z);
            i3.ToString().Should().Be(z.ToString(CultureInfo.InvariantCulture));
        }

        [TestCase("0x11111111111111111111111111111111", "0x11111111111111111111111111111111", "0x123456789ABCDF0123456789ABCDF0120FEDCBA987654320FEDCBA987654321")]
        [TestCase("0x11111111111111111111111111111111", "0x11111111111111111111111111111110", "0x123456789ABCDF0123456789ABCDF010FEDCBA987654320FEDCBA9876543210")]
        [TestCase("0xFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF", "0x00000000000000000123456789ABCDEF", "0x123456789ABCDEEFFFFFFFFFFFFFFFFFEDCBA9876543211")]
        [TestCase("0xFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF", "0xFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF", "0xFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFE00000000000000000000000000000001")]
        [TestCase("0x0FEDCBA9876543210FEDCBA987654321", "0x0123456789ABCDEF0123456789ABCDEF", "0x121FA00AD77D742247ACC9140513B74458FAB20783AF1222236D88FE5618CF")]
        [TestCase("0xFFFFFFFFFFFFFFF279EFEC87FED69879", "0x0000000000000000000000011CA789F3", "0x11CA789F2FFFFFFF0F66C6C554FBF9B9B703A7BDB")]
        [TestCase("0xFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF", "0xFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF", "0x1")]
        public void Should_multiply_big_numbers_correctly(string x, string y, string z)
        {
            Int256 i1 = Int256.Parse(x);
            Int256 i2 = Int256.Parse(y);

            Int256 i3 = i1*i2;

            ("0x" + i3.ToString("X")).Should().Be(z);
        }

        [TestCase(1, -1, -1)]
        [TestCase(4, 2, 2)]
        [TestCase(2, 2, 1)]
        [TestCase(-1, 1, -1)]
        [TestCase(-4, -2, 2)]
        [TestCase(1000, -50, -20)]
        public void Should_divide_correctly(int x, int y, int z)
        {
            var i1 = (Int256) x;
            var i2 = (Int256) y;

            Int256 i3 = i1/i2;

            ((int) i3).Should().Be(z);
            i3.ToString().Should().Be(z.ToString(CultureInfo.InvariantCulture));
        }

        [TestCase("0x11111111111111111111111111111111", "0x11111111111111111111111111111111", "0x00000000000000000000000000000001")]
        [TestCase("0x11111111111111111111111111111111", "0x00000000000000000000000111111110", "0x00000000100000001000000010000000")]
        [TestCase("0xFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF", "0x00000000000000000123456789ABCDEF", "0x00000000000000E1000000000000D3D1")]
        [TestCase("0x7FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF", "0x00000000000000000123456789ABCDEF", "0x000000000000007080000000000069E8")]
        [TestCase("0xFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF", "0xFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF", "0x00000000000000000000000000000001")]
        [TestCase("0x0FEDCBA9876543210FEDCBA987654321", "0x0123456789ABCDEF0123456789ABCDEF", "0x0000000000000000000000000000000E")]
        [TestCase("0x59FE45F3CACCE58279EFEC87FED69879", "0x00000000000000000937F5E11CA789F3", "0x0000000000000009C31BD9DCA1E0BEDC")]
        public void Should_devide_big_numbers_correctly(string x, string y, string z)
        {
            Int256 i1 = Int256.Parse(x);
            Int256 i2 = Int256.Parse(y);

            Int256 i3 = i1/i2;

            ("0x" + i3.ToString("X32")).Should().Be(z);
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

            var i1 = (Int256) x;
            var i2 = (Int256) y;

            bool value = z.HasValue && z.Value;

            (i1 > i2).Should().Be(value);
            (i1 < i2).Should().Be(z.HasValue && !value);
            (i1 == i2).Should().Be(!z.HasValue);
            (i1 != i2).Should().Be(z.HasValue);
        }

        [TestCase("0x11111111111111111111111111111111", "0x11111111111111111111111111111111", null)]
        [TestCase("0x11111111111111111111111111111111", "0x00000000000000000000000111111110", true)]
        [TestCase("0xFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF", "0x00000000000000000123456789ABCDEF", true)]
        [TestCase("0xFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF", "0x00000000000000000123456789ABCDEF", false)]
        [TestCase("0x7FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF", "0x00000000000000000123456789ABCDEF", true)]
        [TestCase("0xFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF", "0xFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF", null)]
        [TestCase("0x0FEDCBA9876543210FEDCBA987654321", "0x0123456789ABCDEF0123456789ABCDEF", true)]
        [TestCase("0x59FE45F3CACCE58279EFEC87FED69879", "0x00000000000000000937F5E11CA789F3", true)]
        public void Should_compare_big_numbers_correctly(string x, string y, bool? z)
        {
            // z == null means that numbers are equal.

            Int256 i1 = Int256.Parse(x);
            Int256 i2 = Int256.Parse(y);

            bool value = z.HasValue && z.Value;

            (i1 > i2).Should().Be(value);
            (i1 < i2).Should().Be(z.HasValue && !value);
            (i1 == i2).Should().Be(!z.HasValue);
            (i1 != i2).Should().Be(z.HasValue);
        }

        [TestCase("0x0000000000000000000000000000000007FFFFFFFFFFFFFFFFFFFFFFFFFFFFFF", "1", "0x03FFFFFFFFFFFFFFFFFFFFFFFFFFFFFF")]
        [TestCase("0xFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF", "1", "0xFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF")]     //test sign, -1 >> 1 should stay -1
        [TestCase("0x000000000000000000000000000000003A3A3A3A3A3A3A3A0000000000000000", "64", "0x00000000000000003A3A3A3A3A3A3A3A")]
        [TestCase("0x00000000000000003A3A3A3A3A3A3A3A00000000000000000000000000000000", "128", "0x00000000000000003A3A3A3A3A3A3A3A")]
        [TestCase("0xFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF0F", "4", "0xFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF0")]
        [TestCase("0x000000000000000000000000000000003A3A3A3A3A3A3A3A3A3A3A3A3A3A3A3A", "2", "0x0E8E8E8E8E8E8E8E8E8E8E8E8E8E8E8E")]
        public void Should_rightShift_correctly(string x, string y, string z)
        {
            //
            // X >> Y should = z
            //
            Int256 i1 = Int256.Parse(x);
            int shifthBy = int.Parse(y);
            Int256 i3 = i1 >> shifthBy;

            ("0x" + i3.ToString("X32")).Should().Be(z);
        }


        [TestCase("0x0000000000000000000000000000000003FFFFFFFFFFFFFFFFFFFFFFFFFFFFFF", "1", "0x07FFFFFFFFFFFFFFFFFFFFFFFFFFFFFE")]
        [TestCase("0xFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF", "1", "0xFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFE")]     //test sign, -1 >> 1 should stay -1
        [TestCase("0x0000000000000000000000000000000000000000000000003A3A3A3A3A3A3A3A", "64", "0x3A3A3A3A3A3A3A3A0000000000000000")]
        [TestCase("0xFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF0F", "4", "0xFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF0F0")]   //should do a shift of 4
        [TestCase("0x0000000000000000000000000000000000FFFFFFFFFFFFFFFFFFFFFFFFFFFFFF", "4", "0x0FFFFFFFFFFFFFFFFFFFFFFFFFFFFFF0")]   //should do a shift of 4
        [TestCase("0x000000000000000000000000000000003A3A3A3A3A3A3A3A3A3A3A3A3A3A3A3A", "2", "0xE8E8E8E8E8E8E8E8E8E8E8E8E8E8E8E8")]
        public void Should_leftShift_correctly(string x, string y, string z)
        {
            //
            // X >> Y should = z
            //
            Int256 i1 = Int256.Parse(x);
            int shifthBy = int.Parse(y);
            Int256 i3 = i1 << shifthBy;

            ("0x" + i3.ToString("X32")).Should().Be(z);
        }
    }
}
