using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CIS726_Assignment2.Tests;
using MessageParser.Models;

namespace CIS726_Assignment2.Tests
{
    [TestClass]
    public class ElectiveListTest
    {
        [TestMethod]
        public void ElectiveListNameIsRequired()
        {
            UnitTestHelpers.testIsRequired(typeof(ElectiveList).GetProperty("electiveListName"));
        }

        [TestMethod]
        public void ElectiveListShortNameIsRequired()
        {
            UnitTestHelpers.testIsRequired(typeof(ElectiveList).GetProperty("shortName"));
        }

        [TestMethod]
        public void ElectiveListShortNameMaxLength()
        {
            UnitTestHelpers.testStringLength(typeof(ElectiveList).GetProperty("shortName"), 20);
        }
    }
}
