using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using MessageParser.Models;

namespace AuthParser.Models
{

    /// <summary>
    /// This class is the many-to-many link between the Elective List class and the courses
    /// that are part of that elective list. It does not contain any other important
    /// information about the link, but is included for simplicity in dealing with the database.
    /// </summary>
    public class UserRoles : IModel
    {
        [ScaffoldColumn(false)]
        public override int ID { get; set; }

        [Required(ErrorMessage = "A user ID is required")]
        public int userID { get; set; }

        [Required(ErrorMessage = "An role ID is required")]
        public int roleID { get; set; }

        public virtual User user { get; set; }
        public virtual Role role { get; set; }
    }
}