// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MathUtilsFacts.cs">
//   Copyright (c) 2013 Alexander Logger. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections;
using BigMath.Utils;
using FluentAssertions;
using NUnit.Framework;

namespace BigMath.Tests
{
    [TestFixture]
    public class MathUtilsFacts
    {
        [TestCase(new[] {0U}, new[] {1U}, new[] {0U}, new[] {0U}, TestName = "0 / 1 = 0 (0)")]
        [TestCase(new[] {1U}, new[] {1U}, new[] {1U}, new[] {0U}, TestName = "1 / 1 = 1 (0)")]
        [TestCase(new[] {10U}, new[] {2U}, new[] {5U}, new[] {0U}, TestName = "10 / 2 = 5 (0)")]
        [TestCase(new[] {2U}, new[] {10U}, new[] {0U}, new[] {2U}, TestName = "2 / 10 = 0 (2)")]
        [TestCase(new[] {0xFFFFU, 0xFFFFFFFFU, 0xFFFFFFFFU}, new[] {0xAAAAU, 0xAAAAAAAAU, 0xAAAAAAAAU}, new[] {1U}, new[] {0x5555U, 0x55555555U, 0x55555555U},
            TestName = "0xFFFFFFFFFFFFFFFFFFFF / 0xAAAAAAAAAAAAAAAAAAAA = 0x1 (0x55555555555555555555)")]
        [TestCase(new[] {0xA26B95A5U, 0x7DBFE009U, 0xEB2DD171U, 0x23256C2U}, new[] {0x1A08F981U, 0x17ED4894U}, new[] {0x981CB2B3U, 0x17809C32U},
            new[] {0x4E8D6E72U, 0x16BA20FU}, TestName = "0x23256C2EB2DD1717DBFE009A26B95A5 / 0x17ED48941A08F981 = 0x17809C32981CB2B3 (0x16BA20F4E8D6E72)")]
        [TestCase(new[] {0U, 0x10U}, new[] {0x2U}, new[] {0U, 0x8U}, new[] {0U}, TestName = "0x10U<<32/2 = 0x8U<<32(0)")]
        public void Should_divide_with_reminder(uint[] dividend, uint[] divisor, uint[] expectedQuotient, uint[] expectedRemainder)
        {
            uint[] actualQuotient;
            uint[] actualRemainder;

            MathUtils.DivModUnsigned(dividend, divisor, out actualQuotient, out actualRemainder);

            actualQuotient.ShouldAllBeEquivalentTo(expectedQuotient, "Quotient is not as expected.");
            actualRemainder.ShouldAllBeEquivalentTo(expectedRemainder, "Reminder is not as expected.");
        }

        [TestCase("8633", "89", "97")]
        [TestCase("10967535067", "104723", "104729")]
        [TestCase("0x17ED48941A08F981", "0x494C553B", "0x53911073")]
        [TestCase("2098829548919646817849", "715827883", "2932031007403")]
        [TestCase("999900000000999999990001", "99990001", "9999999900000001")]
        [TestCase("550195573271929345661963833", "715827883", "768614336404564651")]
        [TestCase("5533469875130886419280184847", "99990001", "55340232221128654847")]
        //[TestCase("2852213850458175921094913949697", "51539607551", "55340232221128654847")]
        public void Should_get_prime_multipliers_for_Int128(string pqS, string expectedPs, string expectedQs)
        {
            Int128 pq = Int128.Parse(pqS);
            Int128 expectedP = Int128.Parse(expectedPs);
            Int128 expectedQ = Int128.Parse(expectedQs);
            Int128 p, q;

            pq.GetPrimeMultipliers(out p, out q);

            p.Should().Be(expectedP);
            q.Should().Be(expectedQ);
        }

        [TestCase("8633", "89", "97")]
        [TestCase("10967535067", "104723", "104729")]
        [TestCase("0x17ED48941A08F981", "0x494C553B", "0x53911073")]
        [TestCase("2098829548919646817849", "715827883", "2932031007403")]
        [TestCase("999900000000999999990001", "99990001", "9999999900000001")]
        [TestCase("550195573271929345661963833", "715827883", "768614336404564651")]
        [TestCase("5533469875130886419280184847", "99990001", "55340232221128654847")]
        //[TestCase("2852213850458175921094913949697", "51539607551", "55340232221128654847")]
        //[TestCase("2253601067072664030639173111353", "2932031007403", "768614336404564651")]
        //[TestCase("154866286100907105216716400854538488352313", "768614336404564651", "201487636602438195784363")]
        //[TestCase("170277282318432095373149951383568233665261047797485113", "201487636602438195784363", "845100400152152934331135470251")]
        //[TestCase("47928794074934470746074693488053803551576675688093033978263006055993", "845100400152152934331135470251", "56713727820156410577229101238628035243")]
        public void Should_get_prime_multipliers_for_Int256(string pqS, string expectedPs, string expectedQs)
        {
            Int256 pq = Int256.Parse(pqS);
            Int256 expectedP = Int256.Parse(expectedPs);
            Int256 expectedQ = Int256.Parse(expectedQs);
            Int256 p, q;

            pq.GetPrimeMultipliers(out p, out q);

            p.Should().Be(expectedP);
            q.Should().Be(expectedQ);
        }

        [Test, TestCaseSource(typeof (MathUtilsTestCases), "ShiftTestCases")]
        public ulong[] Should_shift(ulong[] bits, int shift)
        {
            ulong[] shiftedBits = MathUtils.Shift(bits, shift);
            return shiftedBits;
        }
    }

    public class MathUtilsTestCases
    {
        public static IEnumerable ShiftTestCases
        {
            get
            {
                yield return new TestCaseData(new[] {0x1UL, 0x0UL}, -127).Returns(new[] {0x0UL, 0x1UL << 63}).SetName("1 << 127");
                yield return new TestCaseData(new[] {0x1UL, 0x0UL}, 1).Returns(new[] {0UL, 0UL}).SetName("1 >> 1");
                yield return new TestCaseData(new[] {0x0UL, 0x1UL << 63}, 127).Returns(new[] {1UL, 0UL}).SetName("(1<<127) >> 127");
                yield return new TestCaseData(new[] {0x0UL, 0x1UL << 63}, -1).Returns(new[] {0UL, 0UL}).SetName("(1<<127) << 1");
                yield return
                    new TestCaseData(new[] {0x0UL, 0x0102030405060708UL}, 32).Returns(new[] {0x0506070800000000UL, 0x01020304UL})
                        .SetName("(0x0102030405060708UL<<64) >> 32");
                yield return
                    new TestCaseData(new[] {0xA9AAABACADAEAFA0UL, 0xA1A2A3A4A5A6A7A8UL}, 32).Returns(new[] {0xA5A6A7A8A9AAABACUL, 0xA1A2A3A4UL})
                        .SetName("(0xA1A2A3A4A5A6A7A8A9AAABACADAEAFA0UL) >> 32");
                yield return
                    new TestCaseData(new[] {0x0UL, 0xA9AAABACADAEAFA0UL, 0xA1A2A3A4A5A6A7A8UL}, 32).Returns(new[]
                    {
                        0xADAEAFA000000000UL,
                        0xA5A6A7A8A9AAABACUL,
                        0xA1A2A3A4UL
                    }).SetName("(0xA1A2A3A4A5A6A7A8A9AAABACADAEAFA0UL<<64) >> 32");
                yield return
                    new TestCaseData(new[] {0xA9AAABACADAEAFA0UL, 0xA1A2A3A4A5A6A7A8UL, 0x0UL}, -32).Returns(new[]
                    {
                        0xADAEAFA000000000UL,
                        0xA5A6A7A8A9AAABACUL,
                        0xA1A2A3A4UL
                    }).SetName("(0xA1A2A3A4A5A6A7A8A9AAABACADAEAFA0UL) << 32");
            }
        }
    }
}
