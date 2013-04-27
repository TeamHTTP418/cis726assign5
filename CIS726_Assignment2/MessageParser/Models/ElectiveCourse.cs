using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace MessageParser.Models
{

    /// <summary>
    /// This class is the many-to-many join class between the Degree Program table
    /// and the Elective List table. It contains extra information about an elective such
    /// as the semester and number of credit hours needed.
    /// </summary>
    public class ElectiveCourse : IModel
    {
        [ScaffoldColumn(false)]
        public override int ID { get; set; }

        [Required(ErrorMessage = "An ElectiveList ID is required")]
        public int electiveListID { get; set; }

        [Required(ErrorMessage = "A Degree Program ID is required")]
        public int degreeProgramID { get; set; }

        [Required(ErrorMessage = "A semester is required")]
        [Range(1, 8, ErrorMessage = "The semester must be a number between 1 and 8 inclusive")]
        public int semester { get; set; }

        [Required(ErrorMessage = "The number of expected credit hours is required")]
        [Range(1, 18, ErrorMessage = "The number of expected credit hours must be between 1 and 18 inclusive")]
        public int credits { get; set; }

        public override bool Equals(object obj)
        {
            ElectiveCourse course = obj as ElectiveCourse;
            if (course != null)
            {
                if (course.ID == ID)
                {
                    return true;
                }
                return false;
            }
            return base.Equals(obj);
        }

        public virtual ElectiveList electiveList { get; set; }
        public virtual DegreeProgram degreeProgram { get; set; }
    }
}