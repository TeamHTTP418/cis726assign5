using MessageParser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace CIS726_Assignment2.Models
{
    [DataContract]
    [KnownType(typeof(Course))]
    [KnownType(typeof(DegreeProgram))]
    public class TestResponse
    {
        [DataMember]
        public Guid ID { get; set; }
        [DataMember]
        public bool Success { get; set; }
        [DataMember]
        public object Result { get; set; }
    }
}