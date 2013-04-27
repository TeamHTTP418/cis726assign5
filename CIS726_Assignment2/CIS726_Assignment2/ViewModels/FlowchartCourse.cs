using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CIS726_Assignment2.ViewModels
{
    public class FlowchartCourse
    {
        public int pcourseID { get; set; }
        public int courseID { get; set; }
        public string courseTitle { get; set; }
        public string courseName { get; set; }
        public int elistID { get; set; }
        public string elistName { get; set; }
        public int semester { get; set; }
        public int order { get; set; }
        public string hours { get; set; }
        public int[] prereq { get; set; }
    }
}