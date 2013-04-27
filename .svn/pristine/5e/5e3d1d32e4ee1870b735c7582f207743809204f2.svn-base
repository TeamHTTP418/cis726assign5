using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using MessageParser.Models;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CIS726_Assignment2.Tests
{
    class UnitTestHelpers
    {
        public static void testIsRequired(PropertyInfo propertyInfo)
        {
            var attribute = propertyInfo.GetCustomAttributes(typeof(RequiredAttribute), false) as RequiredAttribute[];
            Assert.IsNotNull(attribute);
        }

        public static void testStringLength(PropertyInfo propertyInfo, int maxLength)
        {
            var attr = propertyInfo.GetCustomAttributes(typeof(StringLengthAttribute), false) as StringLengthAttribute[];
            Assert.IsNotNull(attr);
            Assert.AreEqual(attr.Length, 1);
            Assert.AreEqual(attr[0].MaximumLength, maxLength);
        }

        public static void testStringLengthMin(PropertyInfo propertyInfo, int minLength)
        {
            var attr = propertyInfo.GetCustomAttributes(typeof(StringLengthAttribute), false) as StringLengthAttribute[];
            Assert.IsNotNull(attr);
            Assert.AreEqual(attr.Length, 1);
            Assert.AreEqual(attr[0].MinimumLength, minLength);
        }

        public static void testRange(PropertyInfo propertyInfo, int min, int max)
        {
            var attr = propertyInfo.GetCustomAttributes(typeof(RangeAttribute), false) as RangeAttribute[];
            Assert.IsNotNull(attr);
            Assert.AreEqual(attr.Length, 1);
            Assert.AreEqual(attr[0].Minimum, min);
            Assert.AreEqual(attr[0].Maximum, max);
        }
    }
}
