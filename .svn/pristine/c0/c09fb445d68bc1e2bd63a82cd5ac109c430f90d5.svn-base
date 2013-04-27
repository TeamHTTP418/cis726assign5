using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MessageParser.Models;
using System.ComponentModel.DataAnnotations;
using CIS726_Assignment2.Tests;

namespace CIS726_Assignment2.Tests
{
    [TestClass]
    public class CourseTest
    {
        [TestMethod]
        public void CoursePrefixIsRequired()
        {
            UnitTestHelpers.testIsRequired(typeof(Course).GetProperty("coursePrefix"));
        }

        [TestMethod]
        public void CoursePrefixMaximumLength()
        {
            UnitTestHelpers.testStringLength(typeof(Course).GetProperty("coursePrefix"), 5);
        }

        [TestMethod]
        public void CourseNumberIsRequired()
        {
            UnitTestHelpers.testIsRequired(typeof(Course).GetProperty("courseNumber"));
        }

        [TestMethod]
        public void CourseNumberRange()
        {
            UnitTestHelpers.testRange(typeof(Course).GetProperty("courseNumber"), 0, 999);
        }

        [TestMethod]
        public void CourseTitleIsRequired()
        {
            UnitTestHelpers.testIsRequired(typeof(Course).GetProperty("courseTitle"));
        }

        [TestMethod]
        public void CourseDescriptionIsRequired()
        {
            UnitTestHelpers.testIsRequired(typeof(Course).GetProperty("courseDescription"));
        }

        [TestMethod]
        public void CourseMinCreditHoursIsRequired()
        {
            UnitTestHelpers.testIsRequired(typeof(Course).GetProperty("minHours"));
        }

        [TestMethod]
        public void CourseMinCreditHoursRange()
        {
            UnitTestHelpers.testRange(typeof(Course).GetProperty("minHours"), 0, 18);
        }

        [TestMethod]
        public void CourseMaxCreditHoursIsRequired()
        {
            UnitTestHelpers.testIsRequired(typeof(Course).GetProperty("maxHours"));
        }

        [TestMethod]
        public void CourseMaxCreditHoursRange()
        {
            UnitTestHelpers.testRange(typeof(Course).GetProperty("maxHours"), 0, 18);
        }

        [TestMethod]
        public void CourseHeaderFormattedProperly()
        {
            Course course = new Course(){
                courseNumber = 101,
                coursePrefix = "ABCDE",
                courseTitle = "Lorem Ipsum Title Test"
            };
            String courseHeader = course.coursePrefix + " " + course.courseNumber + " - " + course.courseTitle;
            Assert.AreEqual(courseHeader, course.courseHeader);
        }

        [TestMethod]
        public void CourseCatalogNumberFormattedProperly()
        {
            Course course = new Course()
            {
                courseNumber = 101,
                coursePrefix = "ABCDE",
            };
            String courseCatalogNumber = course.coursePrefix + " " + course.courseNumber;
            Assert.AreEqual(courseCatalogNumber, course.courseCatalogNumber);
        }

        [TestMethod]
        public void CourseHoursCalculatesVariableProperly()
        {
            Course course = new Course()
            {
                minHours = 1,
                maxHours = 1,
                variable = true
            };
            Assert.AreEqual("Variable", course.courseHours);
        }

        [TestMethod]
        public void CourseHoursCalculatesSingleHoursProperly()
        {
            Course course = new Course()
            {
                minHours = 1,
                maxHours = 1,
                variable = false
            };
            Assert.AreEqual("1", course.courseHours);
        }

        [TestMethod]
        public void CourseHoursCalculatesMultipleHoursProperly()
        {
            Course course = new Course()
            {
                minHours = 1,
                maxHours = 2,
                variable = false
            };
            Assert.AreEqual("1-2", course.courseHours);
        }
    }
}
