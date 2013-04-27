using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace MessageParser.Models
{
    /// <summary>
    /// This class represents a required course in a degree program, and is the many-to-many link between
    /// the Degree Programs and the Courses.
    /// 
    /// It contains extra information such as the semester the class should be taken.
    /// </summary>
    public class RequiredCourse : IModel
    {
        [ScaffoldColumn(false)]
        public override int ID { get; set; }

        [Required(ErrorMessage="A Course ID is required")]
        public int courseID { get; set; }

        [Required(ErrorMessage = "A Degree Program ID is required")]
        public int degreeProgramID { get; set; }

        [Required(ErrorMessage="A semester is required")]
        [DisplayName("Semester")]
        [Range(1, 8, ErrorMessage="The semester must be a number between 1 and 8 inclusive")]
        public int semester { get; set; }

        public virtual Course course { get; set; }
        public virtual DegreeProgram degreeProgram { get; set; }

        public override bool Equals(object obj)
        {
            RequiredCourse course = obj as RequiredCourse;
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
    }
}