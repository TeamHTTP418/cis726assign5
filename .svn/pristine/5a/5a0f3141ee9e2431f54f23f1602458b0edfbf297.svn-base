using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel.DataAnnotations;
using MessageParser.Models;

namespace CIS726_Assignment2.Tests
{
    [TestClass]
    public class ElectiveListCourseTest
    {
        [TestMethod]
        public void ElectiveListCourseCourseIDisRequired()
        {
            UnitTestHelpers.testIsRequired(typeof(ElectiveListCourse).GetProperty("courseID"));
        }

        [TestMethod]
        public void ElectiveListCourseElectiveListIDisRequired()
        {
            UnitTestHelpers.testIsRequired(typeof(ElectiveListCourse).GetProperty("electiveListID"));
        }

        [TestMethod]
        public void ElectiveListCourseEquality()
        {
            ElectiveListCourse course1 = new ElectiveListCourse()
            {
                ID = 1,
                electiveListID = 1
            };
            ElectiveListCourse course2 = new ElectiveListCourse()
            {
                ID = 1,
                electiveListID = 2
            };
            Assert.AreEqual(course1, course2);
        }
    }
}
