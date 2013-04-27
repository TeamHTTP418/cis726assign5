using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MessageParser.Models;
using AuthParser.Models;

namespace CIS726_Assignment2.ViewModels
{
    /// <summary>
    /// This class represents a single elective list
    /// 
    /// It has a list of courses that are contained in the list
    /// It also has a list of elective courses that link it to the degree
    /// programs it is a part of
    /// </summary>
    public class PlanWithUser : IModel
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

        [Required(ErrorMessage = "A User ID is required")]
        [DisplayName("Username")]
        public int userID { get; set; }

        [Required(ErrorMessage = "A starting Semester is required")]
        [DisplayName("Starting Semester")]
        public int semesterID { get; set; }

        [DisplayName("Starting Semester")]
        public Semester semester { get; set; }

        public ICollection<PlanCourse> planCourses { get; set; }

        public string username { get; set; }
        public string realName { get; set; }

        public PlanWithUser(Plan plan, User user)
        {
            this.ID = plan.ID;
            this.planName = plan.planName;
            this.degreeProgramID = plan.degreeProgramID;
            this.degreeProgram = plan.degreeProgram;
            this.userID = plan.userID;
            this.semesterID = plan.semesterID;
            this.semester = plan.semester;
            this.planCourses = plan.planCourses;
            this.username = user.username;
            this.realName = user.realName;
        }
    }
}