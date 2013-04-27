using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CIS726_Assignment2.Tests;
using MessageParser.Models;

namespace CIS726_Assignment2.Tests
{
    [TestClass]
    public class PrerequisiteCourseTest
    {
        [TestMethod]
        public void PrerequisiteCourseIDIsRequired()
        {
            UnitTestHelpers.testIsRequired(typeof(PrerequisiteCourse).GetProperty("prerequisiteCourseID"));
        }

        [TestMethod]
        public void PrerequisiteForCourseIDIsRequired()
        {
            UnitTestHelpers.testIsRequired(typeof(PrerequisiteCourse).GetProperty("prerequisiteForCourseID"));
        }

    }
}
