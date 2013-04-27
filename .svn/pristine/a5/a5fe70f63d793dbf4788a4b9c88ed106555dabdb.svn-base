using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using MessageParser.Models;

namespace AuthParser.Models
{
    [Table("Roles")]
    public class Role : IModel
    {
        [ScaffoldColumn(false)]
        [Key]
        public override int ID { get; set; }

        [DisplayName("Role Name")]
        [Required(ErrorMessage = "The Role name is required")]
        public String rolename { get; set; }
    }
}