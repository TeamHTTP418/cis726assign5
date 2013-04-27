using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel.DataAnnotations;
using MessageParser.Models;
using CIS726_Assignment2.Tests;

namespace CIS726_Assignment2.Tests
{
    [TestClass]
    public class DegreeProgramTest
    {
        [TestMethod]
        public void DegreeProgramNameIsRequired()
        {
            UnitTestHelpers.testIsRequired(typeof(DegreeProgram).GetProperty("degreeProgramName"));
        }
    }
}
