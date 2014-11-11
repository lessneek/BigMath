// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BigIntegerTest.cs">
//   Copyright (c) 2014 Alexander Logger. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

#region [The Bouncy Castle License] Base license of partial code used in this file.
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2000-2011 The Legion Of The Bouncy Castle (http://www.bouncycastle.org)
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software 
// and associated documentation files (the "Software"), to deal in the Software without restriction, 
// including without limitation the rights to use, copy, modify, merge, publish, distribute, 
// sub license, and/or sell copies of the Software, and to permit persons to whom the Software 
// is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
// PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.
#endregion

using System;
using BigMath.Utils;
using FluentAssertions;
using NUnit.Framework;

namespace BigMath.Tests
{
    [TestFixture]
    public class BigIntegerTest
    {
        private static readonly Random Rnd = new Random();

        private static BigInteger Val(long n)
        {
            return BigInteger.ValueOf(n);
        }

        private static BigInteger Mersenne(int e)
        {
            return Two.Pow(e).Subtract(One);
        }

        private static readonly BigInteger MinusTwo = BigInteger.Two.Negate();
        private static readonly BigInteger MinusOne = BigInteger.One.Negate();
        private static readonly BigInteger Zero = BigInteger.Zero;
        private static readonly BigInteger One = BigInteger.One;
        private static readonly BigInteger Two = BigInteger.Two;
        private static readonly BigInteger Three = BigInteger.Three;

        private static readonly int[] FirstPrimes = {2, 3, 5, 7, 11, 13, 17, 19, 23, 29};
        private static readonly int[] NonPrimes = {0, 1, 4, 10, 20, 21, 22, 25, 26, 27};

        private static readonly int[] MersennePrimeExponents = {2, 3, 5, 7, 13, 17, 19, 31, 61, 89};
        private static readonly int[] NonPrimeExponents = {1, 4, 6, 9, 11, 15, 23, 29, 37, 41};

        [Test]
        public void MonoBug81857()
        {
            var b = new BigInteger("18446744073709551616");
            var mod = new BigInteger("48112959837082048697");
            var expected = new BigInteger("4970597831480284165");

            BigInteger manual = b.Multiply(b).Mod(mod);
            Assert.AreEqual(expected, manual, "b * b % mod");
        }

        [Test]
        public void TestAbs()
        {
            Assert.AreEqual(Zero, Zero.Abs());

            Assert.AreEqual(One, One.Abs());
            Assert.AreEqual(One, MinusOne.Abs());

            Assert.AreEqual(Two, Two.Abs());
            Assert.AreEqual(Two, MinusTwo.Abs());
        }

        [Test]
        public void TestAdd()
        {
            for (int i = -10; i <= 10; ++i)
            {
                for (int j = -10; j <= 10; ++j)
                {
                    Assert.AreEqual(Val(i + j), Val(i).Add(Val(j)), "Problem: " + i + ".Add(" + j + ") should be " + (i + j));
                }
            }
        }

        [Test]
        public void TestAnd()
        {
            for (int i = -10; i <= 10; ++i)
            {
                for (int j = -10; j <= 10; ++j)
                {
                    Assert.AreEqual(Val(i & j), Val(i).And(Val(j)), "Problem: " + i + " AND " + j + " should be " + (i & j));
                }
            }
        }

        [Test]
        public void TestAndNot()
        {
            for (int i = -10; i <= 10; ++i)
            {
                for (int j = -10; j <= 10; ++j)
                {
                    Assert.AreEqual(Val(i & ~j), Val(i).AndNot(Val(j)), "Problem: " + i + " AND NOT " + j + " should be " + (i & ~j));
                }
            }
        }

        [Test]
        public void TestBitCount()
        {
            Assert.AreEqual(0, Zero.BitCount);
            Assert.AreEqual(1, One.BitCount);
            Assert.AreEqual(0, MinusOne.BitCount);
            Assert.AreEqual(1, Two.BitCount);
            Assert.AreEqual(1, MinusTwo.BitCount);

            for (int i = 0; i < 100; ++i)
            {
                BigInteger pow2 = One.ShiftLeft(i);

                Assert.AreEqual(1, pow2.BitCount);
                Assert.AreEqual(i, pow2.Negate().BitCount);
            }

            for (int i = 0; i < 10; ++i)
            {
                var test = new BigInteger(128, 0, Rnd);
                int bitCount = 0;

                for (int bit = 0; bit < test.BitLength; ++bit)
                {
                    if (test.TestBit(bit))
                    {
                        ++bitCount;
                    }
                }

                Assert.AreEqual(bitCount, test.BitCount);
            }
        }

        [Test]
        public void TestBitLength()
        {
            Assert.AreEqual(0, Zero.BitLength);
            Assert.AreEqual(1, One.BitLength);
            Assert.AreEqual(0, MinusOne.BitLength);
            Assert.AreEqual(2, Two.BitLength);
            Assert.AreEqual(1, MinusTwo.BitLength);

            for (int i = 0; i < 100; ++i)
            {
                int bit = i + Rnd.Next(64);
                BigInteger odd = new BigInteger(bit, Rnd).SetBit(bit + 1).SetBit(0);
                BigInteger pow2 = One.ShiftLeft(bit);

                Assert.AreEqual(bit + 2, odd.BitLength);
                Assert.AreEqual(bit + 2, odd.Negate().BitLength);
                Assert.AreEqual(bit + 1, pow2.BitLength);
                Assert.AreEqual(bit, pow2.Negate().BitLength);
            }
        }

        [Test]
        public void TestClearBit()
        {
            Assert.AreEqual(Zero, Zero.ClearBit(0));
            Assert.AreEqual(Zero, One.ClearBit(0));
            Assert.AreEqual(Two, Two.ClearBit(0));

            Assert.AreEqual(Zero, Zero.ClearBit(1));
            Assert.AreEqual(One, One.ClearBit(1));
            Assert.AreEqual(Zero, Two.ClearBit(1));

            // TODO Tests for clearing bits in negative numbers

            // TODO Tests for clearing extended bits

            for (int i = 0; i < 10; ++i)
            {
                var n = new BigInteger(128, Rnd);

                for (int j = 0; j < 10; ++j)
                {
                    int pos = Rnd.Next(128);
                    BigInteger m = n.ClearBit(pos);
                    bool test = m.ShiftRight(pos).Remainder(Two).Equals(One);

                    Assert.IsFalse(test);
                }
            }

            for (int i = 0; i < 100; ++i)
            {
                BigInteger pow2 = One.ShiftLeft(i);
                BigInteger minusPow2 = pow2.Negate();

                Assert.AreEqual(Zero, pow2.ClearBit(i));
                Assert.AreEqual(minusPow2.ShiftLeft(1), minusPow2.ClearBit(i));

                BigInteger bigI = BigInteger.ValueOf(i);
                BigInteger negI = bigI.Negate();

                for (int j = 0; j < 10; ++j)
                {
                    string data = "i=" + i + ", j=" + j;
                    Assert.AreEqual(bigI.AndNot(One.ShiftLeft(j)), bigI.ClearBit(j), data);
                    Assert.AreEqual(negI.AndNot(One.ShiftLeft(j)), negI.ClearBit(j), data);
                }
            }
        }

        [Test]
        public void TestCompareTo()
        {
            Assert.AreEqual(0, MinusTwo.CompareTo(MinusTwo));
            Assert.AreEqual(-1, MinusTwo.CompareTo(MinusOne));
            Assert.AreEqual(-1, MinusTwo.CompareTo(Zero));
            Assert.AreEqual(-1, MinusTwo.CompareTo(One));
            Assert.AreEqual(-1, MinusTwo.CompareTo(Two));

            Assert.AreEqual(1, MinusOne.CompareTo(MinusTwo));
            Assert.AreEqual(0, MinusOne.CompareTo(MinusOne));
            Assert.AreEqual(-1, MinusOne.CompareTo(Zero));
            Assert.AreEqual(-1, MinusOne.CompareTo(One));
            Assert.AreEqual(-1, MinusOne.CompareTo(Two));

            Assert.AreEqual(1, Zero.CompareTo(MinusTwo));
            Assert.AreEqual(1, Zero.CompareTo(MinusOne));
            Assert.AreEqual(0, Zero.CompareTo(Zero));
            Assert.AreEqual(-1, Zero.CompareTo(One));
            Assert.AreEqual(-1, Zero.CompareTo(Two));

            Assert.AreEqual(1, One.CompareTo(MinusTwo));
            Assert.AreEqual(1, One.CompareTo(MinusOne));
            Assert.AreEqual(1, One.CompareTo(Zero));
            Assert.AreEqual(0, One.CompareTo(One));
            Assert.AreEqual(-1, One.CompareTo(Two));

            Assert.AreEqual(1, Two.CompareTo(MinusTwo));
            Assert.AreEqual(1, Two.CompareTo(MinusOne));
            Assert.AreEqual(1, Two.CompareTo(Zero));
            Assert.AreEqual(1, Two.CompareTo(One));
            Assert.AreEqual(0, Two.CompareTo(Two));
        }

        [Test]
        public void TestConstructors()
        {
            Assert.AreEqual(BigInteger.Zero, new BigInteger(new byte[] {0}));
            Assert.AreEqual(BigInteger.Zero, new BigInteger(new byte[] {0, 0}));

            for (int i = 0; i < 10; ++i)
            {
                Assert.IsTrue(new BigInteger(i + 3, 0, Rnd).TestBit(0));
            }

            // TODO Other constructors
        }

        [Test]
        public void TestDivide()
        {
            for (int i = -5; i <= 5; ++i)
            {
                try
                {
                    Val(i).Divide(Zero);
                    Assert.Fail("expected ArithmeticException");
                }
                catch (ArithmeticException)
                {
                }
            }

            const int product = 1*2*3*4*5*6*7*8*9;
            const int productPlus = product + 1;

            BigInteger bigProduct = Val(product);
            BigInteger bigProductPlus = Val(productPlus);

            for (int divisor = 1; divisor < 10; ++divisor)
            {
                // Exact division
                BigInteger expected = Val(product/divisor);

                Assert.AreEqual(expected, bigProduct.Divide(Val(divisor)));
                Assert.AreEqual(expected.Negate(), bigProduct.Negate().Divide(Val(divisor)));
                Assert.AreEqual(expected.Negate(), bigProduct.Divide(Val(divisor).Negate()));
                Assert.AreEqual(expected, bigProduct.Negate().Divide(Val(divisor).Negate()));

                expected = Val((product + 1)/divisor);

                Assert.AreEqual(expected, bigProductPlus.Divide(Val(divisor)));
                Assert.AreEqual(expected.Negate(), bigProductPlus.Negate().Divide(Val(divisor)));
                Assert.AreEqual(expected.Negate(), bigProductPlus.Divide(Val(divisor).Negate()));
                Assert.AreEqual(expected, bigProductPlus.Negate().Divide(Val(divisor).Negate()));
            }

            for (int rep = 0; rep < 10; ++rep)
            {
                var a = new BigInteger(100 - rep, 0, Rnd);
                var b = new BigInteger(100 + rep, 0, Rnd);
                var c = new BigInteger(10 + rep, 0, Rnd);
                BigInteger d = a.Multiply(b).Add(c);
                BigInteger e = d.Divide(a);

                Assert.AreEqual(b, e);
            }

            // Special tests for power of two since uses different code path internally
            for (int i = 0; i < 100; ++i)
            {
                int shift = Rnd.Next(64);
                BigInteger a = One.ShiftLeft(shift);
                var b = new BigInteger(64 + Rnd.Next(64), Rnd);
                BigInteger bShift = b.ShiftRight(shift);

                string data = "shift=" + shift + ", b=" + b.ToString(16);

                Assert.AreEqual(bShift, b.Divide(a), data);
                Assert.AreEqual(bShift.Negate(), b.Divide(a.Negate()), data);
                Assert.AreEqual(bShift.Negate(), b.Negate().Divide(a), data);
                Assert.AreEqual(bShift, b.Negate().Divide(a.Negate()), data);
            }

            // Regression
            {
                int shift = 63;
                BigInteger a = One.ShiftLeft(shift);
                var b = new BigInteger(1, "2504b470dc188499".HexToBytes());
                BigInteger bShift = b.ShiftRight(shift);

                string data = "shift=" + shift + ", b=" + b.ToString(16);

                Assert.AreEqual(bShift, b.Divide(a), data);
                Assert.AreEqual(bShift.Negate(), b.Divide(a.Negate()), data);
                //				Assert.AreEqual(bShift.Negate(), b.Negate().Divide(a), data);
                Assert.AreEqual(bShift, b.Negate().Divide(a.Negate()), data);
            }
        }

        [Test]
        public void TestDivideAndRemainder()
        {
            // TODO More basic tests

            var n = new BigInteger(48, Rnd);
            BigInteger[] qr = n.DivideAndRemainder(One);
            Assert.AreEqual(n, qr[0]);
            Assert.AreEqual(Zero, qr[1]);

            for (int rep = 0; rep < 10; ++rep)
            {
                var a = new BigInteger(100 - rep, 0, Rnd);
                var b = new BigInteger(100 + rep, 0, Rnd);
                var c = new BigInteger(10 + rep, 0, Rnd);
                BigInteger d = a.Multiply(b).Add(c);
                BigInteger[] es = d.DivideAndRemainder(a);

                Assert.AreEqual(b, es[0]);
                Assert.AreEqual(c, es[1]);
            }

            // Special tests for power of two since uses different code path internally
            for (int i = 0; i < 100; ++i)
            {
                int shift = Rnd.Next(64);
                BigInteger a = One.ShiftLeft(shift);
                var b = new BigInteger(64 + Rnd.Next(64), Rnd);
                BigInteger bShift = b.ShiftRight(shift);
                BigInteger bMod = b.And(a.Subtract(One));

                string data = "shift=" + shift + ", b=" + b.ToString(16);

                qr = b.DivideAndRemainder(a);
                Assert.AreEqual(bShift, qr[0], data);
                Assert.AreEqual(bMod, qr[1], data);

                qr = b.DivideAndRemainder(a.Negate());
                Assert.AreEqual(bShift.Negate(), qr[0], data);
                Assert.AreEqual(bMod, qr[1], data);

                qr = b.Negate().DivideAndRemainder(a);
                Assert.AreEqual(bShift.Negate(), qr[0], data);
                Assert.AreEqual(bMod.Negate(), qr[1], data);

                qr = b.Negate().DivideAndRemainder(a.Negate());
                Assert.AreEqual(bShift, qr[0], data);
                Assert.AreEqual(bMod.Negate(), qr[1], data);
            }
        }

        [Test]
        public void TestFlipBit()
        {
            for (int i = 0; i < 10; ++i)
            {
                var a = new BigInteger(128, 0, Rnd);
                BigInteger b = a;

                for (int x = 0; x < 100; ++x)
                {
                    // Note: Intentionally greater than initial size
                    int pos = Rnd.Next(256);

                    a = a.FlipBit(pos);
                    b = b.TestBit(pos) ? b.ClearBit(pos) : b.SetBit(pos);
                }

                Assert.AreEqual(a, b);
            }

            for (int i = 0; i < 100; ++i)
            {
                BigInteger pow2 = One.ShiftLeft(i);
                BigInteger minusPow2 = pow2.Negate();

                Assert.AreEqual(Zero, pow2.FlipBit(i));
                Assert.AreEqual(minusPow2.ShiftLeft(1), minusPow2.FlipBit(i));

                BigInteger bigI = BigInteger.ValueOf(i);
                BigInteger negI = bigI.Negate();

                for (int j = 0; j < 10; ++j)
                {
                    string data = "i=" + i + ", j=" + j;
                    Assert.AreEqual(bigI.Xor(One.ShiftLeft(j)), bigI.FlipBit(j), data);
                    Assert.AreEqual(negI.Xor(One.ShiftLeft(j)), negI.FlipBit(j), data);
                }
            }
        }

        [Test]
        public void TestGcd()
        {
            for (int i = 0; i < 10; ++i)
            {
                BigInteger fac = new BigInteger(32, Rnd).Add(Two);
                BigInteger p1 = BigInteger.ProbablePrime(63, Rnd);
                BigInteger p2 = BigInteger.ProbablePrime(64, Rnd);

                BigInteger gcd = fac.Multiply(p1).Gcd(fac.Multiply(p2));

                Assert.AreEqual(fac, gcd);
            }
        }

        [Test]
        public void TestGetLowestSetBit()
        {
            for (int i = 0; i < 10; ++i)
            {
                BigInteger test = new BigInteger(128, 0, Rnd).Add(One);
                int bit1 = test.GetLowestSetBit();
                Assert.AreEqual(test, test.ShiftRight(bit1).ShiftLeft(bit1));
                int bit2 = test.ShiftLeft(i + 1).GetLowestSetBit();
                Assert.AreEqual(i + 1, bit2 - bit1);
                int bit3 = test.ShiftLeft(13*i + 1).GetLowestSetBit();
                Assert.AreEqual(13*i + 1, bit3 - bit1);
            }
        }

        [Test]
        public void TestIntValue()
        {
            int[] tests = {int.MinValue, -1234, -10, -1, 0, ~0, 1, 10, 5678, int.MaxValue};

            foreach (int test in tests)
            {
                Assert.AreEqual(test, Val(test).IntValue);
            }

            // TODO Tests for large numbers
        }

        [Test]
        public void TestIsProbablePrime()
        {
            Assert.IsFalse(Zero.IsProbablePrime(100));
            Assert.IsFalse(Zero.IsProbablePrime(100));
            Assert.IsTrue(Zero.IsProbablePrime(0));
            Assert.IsTrue(Zero.IsProbablePrime(-10));
            Assert.IsFalse(MinusOne.IsProbablePrime(100));
            Assert.IsTrue(MinusTwo.IsProbablePrime(100));
            Assert.IsTrue(Val(-17).IsProbablePrime(100));
            Assert.IsTrue(Val(67).IsProbablePrime(100));
            Assert.IsTrue(Val(773).IsProbablePrime(100));

            foreach (int p in FirstPrimes)
            {
                Assert.IsTrue(Val(p).IsProbablePrime(100));
                Assert.IsTrue(Val(-p).IsProbablePrime(100));
            }

            foreach (int c in NonPrimes)
            {
                Assert.IsFalse(Val(c).IsProbablePrime(100));
                Assert.IsFalse(Val(-c).IsProbablePrime(100));
            }

            foreach (int e in MersennePrimeExponents)
            {
                Assert.IsTrue(Mersenne(e).IsProbablePrime(100));
                Assert.IsTrue(Mersenne(e).Negate().IsProbablePrime(100));
            }

            foreach (int e in NonPrimeExponents)
            {
                Assert.IsFalse(Mersenne(e).IsProbablePrime(100));
                Assert.IsFalse(Mersenne(e).Negate().IsProbablePrime(100));
            }

            // TODO Other examples of 'tricky' values?
        }

        [Test]
        public void TestLongValue()
        {
            long[] tests = {long.MinValue, -1234, -10, -1, 0L, ~0L, 1, 10, 5678, long.MaxValue};

            foreach (long test in tests)
            {
                Assert.AreEqual(test, Val(test).LongValue);
            }

            // TODO Tests for large numbers
        }

        [Test]
        public void TestMax()
        {
            for (int i = -10; i <= 10; ++i)
            {
                for (int j = -10; j <= 10; ++j)
                {
                    Assert.AreEqual(Val(Math.Max(i, j)), Val(i).Max(Val(j)));
                }
            }
        }

        [Test]
        public void TestMin()
        {
            for (int i = -10; i <= 10; ++i)
            {
                for (int j = -10; j <= 10; ++j)
                {
                    Assert.AreEqual(Val(Math.Min(i, j)), Val(i).Min(Val(j)));
                }
            }
        }

        [Test]
        public void TestMod()
        {
            // TODO Basic tests

            for (int rep = 0; rep < 100; ++rep)
            {
                int diff = Rnd.Next(25);
                var a = new BigInteger(100 - diff, 0, Rnd);
                var b = new BigInteger(100 + diff, 0, Rnd);
                var c = new BigInteger(10 + diff, 0, Rnd);

                BigInteger d = a.Multiply(b).Add(c);
                BigInteger e = d.Mod(a);
                Assert.AreEqual(c, e);

                BigInteger pow2 = One.ShiftLeft(Rnd.Next(128));
                Assert.AreEqual(b.And(pow2.Subtract(One)), b.Mod(pow2));
            }
        }

        [Test]
        public void TestModInverse()
        {
            for (int i = 0; i < 10; ++i)
            {
                BigInteger p = BigInteger.ProbablePrime(64, Rnd);
                BigInteger q = new BigInteger(63, Rnd).Add(One);
                BigInteger inv = q.ModInverse(p);
                BigInteger inv2 = inv.ModInverse(p);

                Assert.AreEqual(q, inv2);
                Assert.AreEqual(One, q.Multiply(inv).Mod(p));
            }
        }

        [Test]
        public void TestModPow()
        {
            try
            {
                Two.ModPow(One, Zero);
                Assert.Fail("expected ArithmeticException");
            }
            catch (ArithmeticException)
            {
            }

            Assert.AreEqual(Zero, Zero.ModPow(Zero, One));
            Assert.AreEqual(One, Zero.ModPow(Zero, Two));
            Assert.AreEqual(Zero, Two.ModPow(One, One));
            Assert.AreEqual(One, Two.ModPow(Zero, Two));

            for (int i = 0; i < 10; ++i)
            {
                BigInteger m = BigInteger.ProbablePrime(10 + i*3, Rnd);
                var x = new BigInteger(m.BitLength - 1, Rnd);

                Assert.AreEqual(x, x.ModPow(m, m));
                if (x.Sign != 0)
                {
                    Assert.AreEqual(Zero, Zero.ModPow(x, m));
                    Assert.AreEqual(One, x.ModPow(m.Subtract(One), m));
                }

                var y = new BigInteger(m.BitLength - 1, Rnd);
                var n = new BigInteger(m.BitLength - 1, Rnd);
                BigInteger n3 = n.ModPow(Three, m);

                BigInteger resX = n.ModPow(x, m);
                BigInteger resY = n.ModPow(y, m);
                BigInteger res = resX.Multiply(resY).Mod(m);
                BigInteger res3 = res.ModPow(Three, m);

                Assert.AreEqual(res3, n3.ModPow(x.Add(y), m));

                BigInteger a = x.Add(One); // Make sure it's not zero
                BigInteger b = y.Add(One); // Make sure it's not zero

                Assert.AreEqual(a.ModPow(b, m).ModInverse(m), a.ModPow(b.Negate(), m));
            }
        }

        [Test]
        public void TestMultiply()
        {
            BigInteger one = BigInteger.One;

            Assert.AreEqual(one, one.Negate().Multiply(one.Negate()));

            for (int i = 0; i < 100; ++i)
            {
                int aLen = 64 + Rnd.Next(64);
                int bLen = 64 + Rnd.Next(64);

                BigInteger a = new BigInteger(aLen, Rnd).SetBit(aLen);
                BigInteger b = new BigInteger(bLen, Rnd).SetBit(bLen);
                var c = new BigInteger(32, Rnd);

                BigInteger ab = a.Multiply(b);
                BigInteger bc = b.Multiply(c);

                Assert.AreEqual(ab.Add(bc), a.Add(c).Multiply(b));
                Assert.AreEqual(ab.Subtract(bc), a.Subtract(c).Multiply(b));
            }

            // Special tests for power of two since uses different code path internally
            for (int i = 0; i < 100; ++i)
            {
                int shift = Rnd.Next(64);
                BigInteger a = one.ShiftLeft(shift);
                var b = new BigInteger(64 + Rnd.Next(64), Rnd);
                BigInteger bShift = b.ShiftLeft(shift);

                Assert.AreEqual(bShift, a.Multiply(b));
                Assert.AreEqual(bShift.Negate(), a.Multiply(b.Negate()));
                Assert.AreEqual(bShift.Negate(), a.Negate().Multiply(b));
                Assert.AreEqual(bShift, a.Negate().Multiply(b.Negate()));

                Assert.AreEqual(bShift, b.Multiply(a));
                Assert.AreEqual(bShift.Negate(), b.Multiply(a.Negate()));
                Assert.AreEqual(bShift.Negate(), b.Negate().Multiply(a));
                Assert.AreEqual(bShift, b.Negate().Multiply(a.Negate()));
            }
        }

        [Test]
        public void TestNegate()
        {
            for (int i = -10; i <= 10; ++i)
            {
                Assert.AreEqual(Val(-i), Val(i).Negate());
            }
        }

        [Test]
        public void TestNextProbablePrime()
        {
            BigInteger firstPrime = BigInteger.ProbablePrime(32, Rnd);
            BigInteger nextPrime = firstPrime.NextProbablePrime();

            Assert.IsTrue(firstPrime.IsProbablePrime(10));
            Assert.IsTrue(nextPrime.IsProbablePrime(10));

            BigInteger check = firstPrime.Add(One);

            while (check.CompareTo(nextPrime) < 0)
            {
                Assert.IsFalse(check.IsProbablePrime(10));
                check = check.Add(One);
            }
        }

        [Test]
        public void TestNot()
        {
            for (int i = -10; i <= 10; ++i)
            {
                Assert.AreEqual(Val(~i), Val(i).Not(), "Problem: ~" + i + " should be " + ~i);
            }
        }

        [Test]
        public void TestOr()
        {
            for (int i = -10; i <= 10; ++i)
            {
                for (int j = -10; j <= 10; ++j)
                {
                    Assert.AreEqual(Val(i | j), Val(i).Or(Val(j)), "Problem: " + i + " OR " + j + " should be " + (i | j));
                }
            }
        }

        [Test]
        public void TestPow()
        {
            Assert.AreEqual(One, Zero.Pow(0));
            Assert.AreEqual(Zero, Zero.Pow(123));
            Assert.AreEqual(One, One.Pow(0));
            Assert.AreEqual(One, One.Pow(123));

            var n = new BigInteger("1234567890987654321");
            BigInteger result = One;

            for (int i = 0; i < 10; ++i)
            {
                try
                {
                    Val(i).Pow(-1);
                    Assert.Fail("expected ArithmeticException");
                }
                catch (ArithmeticException)
                {
                }

                Assert.AreEqual(result, n.Pow(i));

                result = result.Multiply(n);
            }
        }

        [Test]
        public void TestRemainder()
        {
            // TODO Basic tests

            for (int rep = 0; rep < 10; ++rep)
            {
                var a = new BigInteger(100 - rep, 0, Rnd);
                var b = new BigInteger(100 + rep, 0, Rnd);
                var c = new BigInteger(10 + rep, 0, Rnd);
                BigInteger d = a.Multiply(b).Add(c);
                BigInteger e = d.Remainder(a);

                Assert.AreEqual(c, e);
            }
        }

        [Test]
        public void TestSetBit()
        {
            Assert.AreEqual(One, Zero.SetBit(0));
            Assert.AreEqual(One, One.SetBit(0));
            Assert.AreEqual(Three, Two.SetBit(0));

            Assert.AreEqual(Two, Zero.SetBit(1));
            Assert.AreEqual(Three, One.SetBit(1));
            Assert.AreEqual(Two, Two.SetBit(1));

            // TODO Tests for setting bits in negative numbers

            // TODO Tests for setting extended bits

            for (int i = 0; i < 10; ++i)
            {
                var n = new BigInteger(128, Rnd);

                for (int j = 0; j < 10; ++j)
                {
                    int pos = Rnd.Next(128);
                    BigInteger m = n.SetBit(pos);
                    bool test = m.ShiftRight(pos).Remainder(Two).Equals(One);

                    Assert.IsTrue(test);
                }
            }

            for (int i = 0; i < 100; ++i)
            {
                BigInteger pow2 = One.ShiftLeft(i);
                BigInteger minusPow2 = pow2.Negate();

                Assert.AreEqual(pow2, pow2.SetBit(i));
                Assert.AreEqual(minusPow2, minusPow2.SetBit(i));

                BigInteger bigI = BigInteger.ValueOf(i);
                BigInteger negI = bigI.Negate();

                for (int j = 0; j < 10; ++j)
                {
                    string data = "i=" + i + ", j=" + j;
                    Assert.AreEqual(bigI.Or(One.ShiftLeft(j)), bigI.SetBit(j), data);
                    Assert.AreEqual(negI.Or(One.ShiftLeft(j)), negI.SetBit(j), data);
                }
            }
        }

        [Test]
        public void TestShiftLeft()
        {
            for (int i = 0; i < 100; ++i)
            {
                int shift = Rnd.Next(128);

                BigInteger a = new BigInteger(128 + i, Rnd).Add(One);
                int bits = a.BitCount; // Make sure nBits is set

                BigInteger negA = a.Negate();
                bits = negA.BitCount; // Make sure nBits is set

                BigInteger b = a.ShiftLeft(shift);
                BigInteger c = negA.ShiftLeft(shift);

                Assert.AreEqual(a.BitCount, b.BitCount);
                Assert.AreEqual(negA.BitCount + shift, c.BitCount);
                Assert.AreEqual(a.BitLength + shift, b.BitLength);
                Assert.AreEqual(negA.BitLength + shift, c.BitLength);

                int j = 0;
                for (; j < shift; ++j)
                {
                    Assert.IsFalse(b.TestBit(j));
                }

                for (; j < b.BitLength; ++j)
                {
                    Assert.AreEqual(a.TestBit(j - shift), b.TestBit(j));
                }
            }
        }

        [Test]
        public void TestShiftRight()
        {
            for (int i = 0; i < 10; ++i)
            {
                int shift = Rnd.Next(128);
                BigInteger a = new BigInteger(256 + i, Rnd).SetBit(256 + i);
                BigInteger b = a.ShiftRight(shift);

                Assert.AreEqual(a.BitLength - shift, b.BitLength);

                for (int j = 0; j < b.BitLength; ++j)
                {
                    Assert.AreEqual(a.TestBit(j + shift), b.TestBit(j));
                }
            }
        }

        [Test]
        public void TestSignValue()
        {
            for (int i = -10; i <= 10; ++i)
            {
                Assert.AreEqual(i < 0 ? -1 : i > 0 ? 1 : 0, Val(i).Sign);
            }
        }

        [Test]
        public void TestSubtract()
        {
            for (int i = -10; i <= 10; ++i)
            {
                for (int j = -10; j <= 10; ++j)
                {
                    Assert.AreEqual(Val(i - j), Val(i).Subtract(Val(j)), "Problem: " + i + ".Subtract(" + j + ") should be " + (i - j));
                }
            }
        }

        [Test]
        public void TestTestBit()
        {
            for (int i = 0; i < 10; ++i)
            {
                var n = new BigInteger(128, Rnd);

                Assert.IsFalse(n.TestBit(128));
                Assert.IsTrue(n.Negate().TestBit(128));

                for (int j = 0; j < 10; ++j)
                {
                    int pos = Rnd.Next(128);
                    bool test = n.ShiftRight(pos).Remainder(Two).Equals(One);

                    Assert.AreEqual(test, n.TestBit(pos));
                }
            }
        }

        [Test]
        public void TestToByteArray()
        {
            byte[] z = BigInteger.Zero.ToByteArray();
            z.ShouldAllBeEquivalentTo(new byte[1]);

            for (int i = 16; i <= 48; ++i)
            {
                BigInteger x = BigInteger.ProbablePrime(i, Rnd);
                byte[] b = x.ToByteArray();
                Assert.AreEqual((i/8 + 1), b.Length);
                var y = new BigInteger(b);
                Assert.AreEqual(x, y);

                x = x.Negate();
                b = x.ToByteArray();
                Assert.AreEqual((i/8 + 1), b.Length);
                y = new BigInteger(b);
                Assert.AreEqual(x, y);
            }
        }

        [Test]
        public void TestToByteArrayUnsigned()
        {
            byte[] z = BigInteger.Zero.ToByteArrayUnsigned();
            z.ShouldAllBeEquivalentTo(new byte[0]);

            for (int i = 16; i <= 48; ++i)
            {
                BigInteger x = BigInteger.ProbablePrime(i, Rnd);
                byte[] b = x.ToByteArrayUnsigned();
                Assert.AreEqual((i + 7)/8, b.Length);
                var y = new BigInteger(1, b);
                Assert.AreEqual(x, y);

                x = x.Negate();
                b = x.ToByteArrayUnsigned();
                Assert.AreEqual(i/8 + 1, b.Length);
                y = new BigInteger(b);
                Assert.AreEqual(x, y);
            }
        }

        [Test]
        public void TestToString()
        {
            string s = "12345667890987654321";

            Assert.AreEqual(s, new BigInteger(s).ToString());
            Assert.AreEqual(s, new BigInteger(s, 10).ToString(10));
            Assert.AreEqual(s, new BigInteger(s, 16).ToString(16));

            for (int i = 0; i < 100; ++i)
            {
                var n = new BigInteger(i, Rnd);

                Assert.AreEqual(n, new BigInteger(n.ToString(2), 2));
                Assert.AreEqual(n, new BigInteger(n.ToString(10), 10));
                Assert.AreEqual(n, new BigInteger(n.ToString(16), 16));
            }
        }

        [Test]
        public void TestValueOf()
        {
            Assert.AreEqual(-1, BigInteger.ValueOf(-1).Sign);
            Assert.AreEqual(0, BigInteger.ValueOf(0).Sign);
            Assert.AreEqual(1, BigInteger.ValueOf(1).Sign);

            for (long i = -5; i < 5; ++i)
            {
                Assert.AreEqual(i, BigInteger.ValueOf(i).IntValue);
            }
        }

        [Test]
        public void TestXor()
        {
            for (int i = -10; i <= 10; ++i)
            {
                for (int j = -10; j <= 10; ++j)
                {
                    Assert.AreEqual(Val(i ^ j), Val(i).Xor(Val(j)), "Problem: " + i + " XOR " + j + " should be " + (i ^ j));
                }
            }
        }
    }
}
