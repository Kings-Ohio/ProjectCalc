using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectCalc;
using ProjectCalc.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectCalc.Tests
{
    [TestClass()]
    public class MainPageTests
    {
        [TestMethod()]
        public void testableSubTest()
        {
            Fraction_class Accumulator = new Fraction_class();
            Fraction_class InBound = new Fraction_class();

            Accumulator.Parse("100 1/2");

            InBound.Parse("50 1/4");    
            
            Accumulator.Subtract(InBound);

            Assert.IsTrue(Accumulator.ToDisplayFraction() == "50 1/4");

            Accumulator.Subtract(InBound);

            Assert.IsTrue(Accumulator.ToDisplayFraction() == "0");


        }
    }
}