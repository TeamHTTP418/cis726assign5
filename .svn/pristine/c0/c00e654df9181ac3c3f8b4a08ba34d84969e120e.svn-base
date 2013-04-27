using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CIS726_Assignment2.Tests;
using MessageParser.Models;


namespace CIS726_Assignment2.Tests
{
    [TestClass]
    public class SemesterTest
    {
        [TestMethod]
        public void SemesterTitleIsRequired()
        {
            UnitTestHelpers.testIsRequired(typeof(Semester).GetProperty("semesterTitle"));
        }

        [TestMethod]
        public void SemesterYearIsRequired()
        {
            UnitTestHelpers.testIsRequired(typeof(Semester).GetProperty("semesterYear"));
        }

        [TestMethod]
        public void SemesterNameDisplaysProperly()
        {
            Semester sem = new Semester()
            {
                semesterYear = 2345,
                semesterTitle = "A Big Title"
            };
            Assert.AreEqual(sem.semesterName, "A Big Title 2345");
        }
    }
}
