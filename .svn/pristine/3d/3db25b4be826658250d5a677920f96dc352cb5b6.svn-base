using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MessageParser.Models
{
    /// <summary>
    /// This class represents a single elective list
    /// 
    /// It has a list of courses that are contained in the list
    /// It also has a list of elective courses that link it to the degree
    /// programs it is a part of
    /// </summary>
    public class Plan : IModel
    {
        [ScaffoldColumn(false)]
        public override int ID { get; set; }

        [DisplayName("Plan of Study Name")]
        [Required(ErrorMessage = "The Plan of Study Name is required")]
        public String planName { get; set; }

        [Required(ErrorMessage = "A Degree Program ID is required")]
        [DisplayName("Degree Program")]
        public int degreeProgramID { get; set; }

        public virtual DegreeProgram degreeProgram { get; set; }

        [Required(ErrorMessage="A User ID is required")]
        [DisplayName("Username")]
        public int userID { get; set; }

        [Required(ErrorMessage="A starting Semester is required")]
        [DisplayName("Starting Semester")]
        public int semesterID { get; set; }

        [DisplayName("Starting Semester")]
        public virtual Semester semester { get; set; }

        public virtual ICollection<PlanCourse> planCourses { get; set; }
    }
}