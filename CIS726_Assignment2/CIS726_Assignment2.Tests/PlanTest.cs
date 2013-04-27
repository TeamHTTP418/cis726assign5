using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CIS726_Assignment2.Tests;
using MessageParser.Models;


namespace CIS726_Assignment2.Tests
{
    [TestClass]
    public class PlanTest
    {
        [TestMethod]
        public void PlanNameIsRequired()
        {
            UnitTestHelpers.testIsRequired(typeof(Plan).GetProperty("planName"));
        }

        [TestMethod]
        public void PlanDegreeProgramIDIsRequired()
        {
            UnitTestHelpers.testIsRequired(typeof(Plan).GetProperty("degreeProgramID"));
        }

        [TestMethod]
        public void PlanUserIDIsRequired()
        {
            UnitTestHelpers.testIsRequired(typeof(Plan).GetProperty("userID"));
        }

        [TestMethod]
        public void PlanSemesterIDIsRequired()
        {
            UnitTestHelpers.testIsRequired(typeof(Plan).GetProperty("semesterID"));
        }
    }
}
