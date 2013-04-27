using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace MessageParser.Models
{
    public class CourseDBContext : DbContext
    {
        public DbSet<Course> Courses { get; set; }
        public DbSet<ElectiveList> ElectiveLists { get; set; }
        public DbSet<DegreeProgram> DegreePrograms { get; set; }
        public DbSet<RequiredCourse> RequiredCourses { get; set; }
        public DbSet<ElectiveCourse> ElectiveCourses { get; set; }
        public DbSet<ElectiveListCourse> ElectiveListCourses { get; set; }
        public DbSet<PrerequisiteCourse> PrerequisiteCourses { get; set; }
        public DbSet<Plan> Plans { get; set; }
        public DbSet<PlanCourse> PlanCourses { get; set; }
        public DbSet<Semester> Semesters { get; set; }

        public CourseDBContext()
        {
            this.Configuration.ProxyCreationEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>().HasMany(p => p.electiveLists).WithRequired(i => i.course);
            modelBuilder.Entity<Course>().HasMany(p => p.degreePrograms).WithRequired(i => i.course);
            modelBuilder.Entity<Course>().HasMany(p => p.prerequisites).WithRequired(i => i.prerequisiteForCourse);
            modelBuilder.Entity<Course>().HasMany(p => p.prerequisiteFor).WithRequired(i => i.prerequisiteCourse);
            modelBuilder.Entity<DegreeProgram>().HasMany(p => p.requiredCourses).WithRequired(i => i.degreeProgram);
            modelBuilder.Entity<DegreeProgram>().HasMany(p => p.electiveCourses).WithRequired(i => i.degreeProgram);
            modelBuilder.Entity<ElectiveList>().HasMany(p => p.courses).WithRequired(i => i.electiveList);
            modelBuilder.Entity<Plan>().HasMany(p => p.planCourses).WithRequired(i => i.plan);
            modelBuilder.Entity<Plan>().HasRequired(p => p.semester);
            modelBuilder.Entity<PlanCourse>().HasRequired(p => p.semester);
            modelBuilder.Entity<PlanCourse>().HasOptional(p => p.electiveList).WithMany().HasForeignKey(r => r.electiveListID);
            modelBuilder.Entity<PlanCourse>().HasOptional(p => p.course).WithMany().HasForeignKey(r => r.courseID);
        }
    }
}