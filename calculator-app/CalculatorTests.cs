using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalculatorApp;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace calculator_app
{
    [TestClass]
    internal class CalculatorTests
    {
        [TestMethod]
        public void TestAdd()
        {
            Calculator calculator = new();
            Assert.Equals(calculator.Add(3, 4), 7);
        }

        [TestMethod]
        public void TestMultiply()
        {
            Calculator calculator = new();
            Assert.Equals(calculator.Multiply(3, 4), 12);
        }

    }
}
