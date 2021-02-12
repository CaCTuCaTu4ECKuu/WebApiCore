using System;
using System.Linq;
using Xunit;

namespace WebApiCore.ComponentModel.Test
{
    using Models;

    public class StringEnumTest
    {
        [Fact]
        public void TestCount()
        {
            Assert.Equal(StringEnumTestModel.ENUMS_COUNT, StringEnumTestModel.GetAll().Count());
        }

        [Fact]
        public void TestEquals()
        {
            var a = StringEnumTestModel.A;
            Assert.Equal(StringEnumTestModel.A, a);
            Assert.NotEqual(StringEnumTestModel.B, a);

            Assert.True(StringEnumTestModel.A.Equals(a));
            Assert.False(StringEnumTestModel.B.Equals(a));
        }

        [Fact]
        public void TestEqualityComparer()
        {
            var b = StringEnumTestModel.B;
            Assert.True(b == StringEnumTestModel.B);
            Assert.False(b != StringEnumTestModel.B);
        }

        [Fact]
        public void TestNullEqualityComparer()
        {
            Assert.False(StringEnumTestModel.A == null);
            Assert.False(null == StringEnumTestModel.A);
            Assert.True(StringEnumTestModel.A != null);
            Assert.True(null != StringEnumTestModel.A);

            StringEnumTestModel e = null;
            Assert.True(e == null);
            Assert.False(e == StringEnumTestModel.C);
        }

        [Fact]
        public void TestFlags()
        {
            var x = StringEnumTestModel.A | StringEnumTestModel.B;

            Assert.NotEqual(StringEnumTestModel.A, x);
            Assert.NotEqual(StringEnumTestModel.B, x);

            Assert.True(x.HasFlag(StringEnumTestModel.A));
            Assert.True(x.HasFlag(StringEnumTestModel.B));
            Assert.False(x.HasFlag(StringEnumTestModel.C));
        }

        [Fact]
        public void TestFlagsEqualityComparer()
        {
            var x1 = StringEnumTestModel.A | StringEnumTestModel.B;
            var x2 = StringEnumTestModel.B | StringEnumTestModel.A;
            var x3 = StringEnumTestModel.A | StringEnumTestModel.C;
            var x4 = StringEnumTestModel.A | StringEnumTestModel.B | StringEnumTestModel.C;

            Assert.Equal(x1, x2);
            Assert.Equal(x2, x1);

            Assert.NotEqual(x1, x3);
            Assert.NotEqual(x3, x2);

            Assert.NotEqual(x2, x4);
            Assert.NotEqual(x4, x2);
            Assert.NotEqual(x3, x4);
            Assert.NotEqual(x4, x3);
        }

        [Fact]
        public void TestHashCode()
        {
            var x1 = StringEnumTestModel.A | StringEnumTestModel.B;
            var x2 = StringEnumTestModel.B | StringEnumTestModel.A;

            Assert.Equal(x1.GetHashCode(), x2.GetHashCode());

            Assert.NotEqual(StringEnumTestModel.A.GetHashCode(), x1.GetHashCode());
            Assert.NotEqual(StringEnumTestModel.B.GetHashCode(), x1.GetHashCode());
            Assert.NotEqual(StringEnumTestModel.A.GetHashCode(), x2.GetHashCode());
            Assert.NotEqual(StringEnumTestModel.B.GetHashCode(), x2.GetHashCode());
        }

        private string[] validValues = new[] { "A", "a", "b", "C" };
        private string[] invalidValues = new[] { null, "", "z", "4", "AA", "aa", "Aa", "a ", "A ", " a", " A", " A "};

        [Fact]
        public void TestValidValueCheck()
        {
            foreach (var value in validValues)
                Assert.True(StringEnumTestModel.IsValidValue(value));

            foreach (var value in invalidValues)
                Assert.False(StringEnumTestModel.IsValidValue(value));
        }

        private string[] parseOkValues = new[] { "A", "A ", " A", " A ", "a", "a ", " a", " a "};
        private string[] parseArgumentExceptionValues = new[] { null, "", " ", "z", "aa", " a a ", ",A", "A,", ",A,", " ,A," , ",A, ", " ,A, ", " , A , " };
        [Fact]
        public void ParseTest()
        {
            foreach (var value in parseOkValues)
            {
                var e = StringEnumTestModel.Parse(value);
                Assert.Equal(StringEnumTestModel.A, e);
            }
            foreach (var value in parseArgumentExceptionValues)
            {
                Assert.Throws<ArgumentException>(() =>
                {
                    StringEnumTestModel.Parse(value);
                });
            }
        }

        [Fact]
        public void TryParseTest()
        {
            foreach (var value in parseOkValues)
            {
                var parseRes = StringEnumTestModel.TryParse(value, out var res);
                Assert.True(parseRes);
                Assert.Equal(StringEnumTestModel.A, res);
            }
            foreach (var value in parseArgumentExceptionValues)
            {
                var parseRes = StringEnumTestModel.TryParse(value, out var res);
                Assert.False(parseRes);
                Assert.Null(res);
            }
        }

        private string[] parseValidFlags = new[] { "a,B", "A,b", "A, b", "A , b ", " A , b", " A , b " };
        private string[] parseArgumentExceptionFlags = new[] { ",A , b", "A , b,", "a,z", "z,a", ",a,z", "a,z,", "a,a", "a,b,a" };

        [Fact]
        public void ParseFlagsTest()
        {
            var eExpected = StringEnumTestModel.A | StringEnumTestModel.B;
            foreach (var value in parseValidFlags)
                Assert.Equal(eExpected, StringEnumTestModel.Parse(value));

            foreach (var value in parseArgumentExceptionFlags)
            {
                Assert.Throws<ArgumentException>(() =>
                {
                    StringEnumTestModel.Parse(value);
                });
            }
        }

        [Fact]
        public void TryParseFlagsTest()
        {
            var eExpected = StringEnumTestModel.A | StringEnumTestModel.B;
            foreach (var value in parseValidFlags)
            {
                var parseRes = StringEnumTestModel.TryParse(value, out var res);
                Assert.True(parseRes);
                Assert.Equal(eExpected, res);
            }

            foreach (var value in parseArgumentExceptionFlags)
            {
                var parseRes = StringEnumTestModel.TryParse(value, out var res);
                Assert.False(parseRes);
                Assert.Null(res);
            }
        }

        [Fact]
        public void FlagsNameTest()
        {
            var x1 = StringEnumTestModel.A | StringEnumTestModel.B;
            Assert.Equal("a,b", x1.Name);

            var x2 = StringEnumTestModel.B | StringEnumTestModel.A;
            Assert.Equal("b,a", x2.Name);

            var owo = StringEnumTestModel.X | StringEnumTestModel.OwO | StringEnumTestModel.B;
            Assert.Equal("X,OwO,b", owo.Name);
        }

        [Fact]
        public void ToStringTest()
        {
            Assert.Equal(StringEnumTestModel.A.Name, StringEnumTestModel.A.ToString());
            Assert.Equal(StringEnumTestModel.X.Name, StringEnumTestModel.X.ToString());
            Assert.Equal(StringEnumTestModel.OwO.Name, StringEnumTestModel.OwO.ToString());

            var x1 = StringEnumTestModel.A | StringEnumTestModel.B;
            Assert.Equal(x1.Name, x1.ToString());

            var x2 = StringEnumTestModel.B | StringEnumTestModel.A;
            Assert.Equal(x2.Name, x2.ToString());

            var owo = StringEnumTestModel.X | StringEnumTestModel.OwO | StringEnumTestModel.B;
            Assert.Equal(owo.Name, owo.ToString());
        }

        [Fact]
        public void ParseNameTest()
        {
            var x = StringEnumTestModel.Parse("x");
            Assert.Equal("X", x.Name);

            var owo1 = StringEnumTestModel.Parse("owo");
            var owo2 = StringEnumTestModel.Parse("OWO");

            Assert.Equal("OwO", owo1.Name);
            Assert.Equal("OwO", owo2.Name);
        }

        [Fact]
        public void TestParsedEquals()
        {
            var x1 = StringEnumTestModel.Parse("a");
            var x2 = StringEnumTestModel.Parse("owo");
            var x3 = StringEnumTestModel.Parse("a,x");

            Assert.True(StringEnumTestModel.A.Equals(x1));
            Assert.True(StringEnumTestModel.OwO.Equals(x2));
            Assert.True((StringEnumTestModel.A | StringEnumTestModel.X).Equals(x3));
        }

        [Fact]
        public void TestFlagsModify()
        {
            var a = StringEnumTestModel.A;
            var x1 = a | StringEnumTestModel.B;

            Assert.NotEqual(a, x1);
            Assert.Equal(StringEnumTestModel.A | StringEnumTestModel.B, x1);

            var x2 = x1 | StringEnumTestModel.OwO;

            Assert.Equal(StringEnumTestModel.OwO | x1, x2);
        }

        [Fact]
        public void TestObjectEquals()
        {
            var x = StringEnumTestModel.A | StringEnumTestModel.B;
            string[] someVals = new[] { "a", "b" };

            Assert.False(StringEnumTestModel.A.Equals(someVals));
            Assert.False(x.Equals(someVals));

            object xo = StringEnumTestModel.B | StringEnumTestModel.A;

            Assert.True(x.Equals(xo));
        }

        [Fact]
        public void HasAnyFlagsTest()
        {
            var x1 = StringEnumTestModel.A | StringEnumTestModel.B | StringEnumTestModel.C;
            var x2 = StringEnumTestModel.A | StringEnumTestModel.C;

            Assert.True(x1.HasAny(StringEnumTestModel.A));
            Assert.True(x1.HasAny(StringEnumTestModel.B));
            Assert.True(x1.HasAny(StringEnumTestModel.C));
            Assert.False(x1.HasAny(StringEnumTestModel.X));
            Assert.False(x1.HasAny(StringEnumTestModel.OwO));

            Assert.True(x1.HasAny(x2));
            Assert.True(x2.HasAny(x1));
        }
    }
}
