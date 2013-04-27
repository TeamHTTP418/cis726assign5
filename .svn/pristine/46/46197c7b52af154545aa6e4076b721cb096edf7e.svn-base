using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace MessageParser.Models
{
    /// <summary>
    /// This class represents a single elective list
    /// 
    /// It has a list of courses that are contained in the list
    /// It also has a list of elective courses that link it to the degree
    /// programs it is a part of
    /// </summary>
    public class ElectiveList : IModel
    {
        [ScaffoldColumn(false)]
        public override int ID { get; set; }

        [DisplayName("Elective List Name")]
        [Required(ErrorMessage="The Elective List Name is required")]
        public String electiveListName { get; set; }

        [DisplayName("Short Name for Flowchart")]
        [Required(ErrorMessage="The short name is required")]
        [StringLength(20, ErrorMessage="The short name can only include up to 20 characters")]
        public String shortName { get; set; }

        public virtual ICollection<ElectiveListCourse> courses { get; set; }
        public virtual ICollection<ElectiveCourse> electiveCourses { get; set; }
    }
}