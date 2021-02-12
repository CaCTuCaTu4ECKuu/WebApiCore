using System;
using Xunit;

namespace WebApiCore.ComponentModel.Test
{
    using Models;

    public class DeterminativePropertySetTest
    {
        [Fact]
        public void BasicWorksCorrect()
        {
            var propSet = new TestDeterminativePropertySet();

            var propA = "hello";
            var propB = 123;
            var propC = DateTime.Now;

            propSet.PropertyA = propA;
            propSet.PropertyB0 = propB;
            propSet.PropertyB = propB;
            propSet.PropertyC = propC;

            Assert.Equal(propA, propSet.PropertyA);
            Assert.Equal(propB, propSet.PropertyB0);
            Assert.Equal(propB, propSet.PropertyB);
            Assert.Equal(propC, propSet.PropertyC);
        }

        [Fact]
        public void InitializatorFillCorrect()
        {
            var propA = "abc";
            var propSet = new TestDeterminativePropertySet()
            {
                PropertyA = propA
            };

            Assert.Equal(propA, propSet.PropertyA);
        }

        [Fact]
        public void DefinedValuesChanges()
        {
            var propA0 = "prop1";
            var propA1 = "prop2";

            var propSet = new TestDeterminativePropertySet()
            {
                PropertyA = propA0
            };

            propSet.PropertyA = propA1;
            Assert.Equal(propA1, propSet.PropertyA);
        }

        [Fact]
        public void IsDefinedWorksCorrect()
        {
            var propSet = new TestDeterminativePropertySet();

            Assert.False(propSet.IsDefined(e => e.PropertyA));
            Assert.False(propSet.IsDefined(e => e.PropertyB0));
            Assert.False(propSet.IsDefined(e => e.PropertyB));
            Assert.False(propSet.IsDefined(e => e.PropertyC));

            propSet.PropertyB = 123;
            Assert.False(propSet.IsDefined(e => e.PropertyA));
            Assert.False(propSet.IsDefined(e => e.PropertyB0));
            Assert.True(propSet.IsDefined(e => e.PropertyB));
            Assert.False(propSet.IsDefined(e => e.PropertyC));
        }
    }
}
