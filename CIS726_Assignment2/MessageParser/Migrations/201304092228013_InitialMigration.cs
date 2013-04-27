namespace MessageParser.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Courses",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        coursePrefix = c.String(nullable: false, maxLength: 5),
                        courseNumber = c.Int(nullable: false),
                        courseTitle = c.String(nullable: false),
                        courseDescription = c.String(nullable: false),
                        minHours = c.Int(nullable: false),
                        maxHours = c.Int(nullable: false),
                        undergrad = c.Boolean(nullable: false),
                        graduate = c.Boolean(nullable: false),
                        variable = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ElectiveListCourses",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        courseID = c.Int(nullable: false),
                        electiveListID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ElectiveLists", t => t.electiveListID, cascadeDelete: true)
                .ForeignKey("dbo.Courses", t => t.courseID, cascadeDelete: true)
                .Index(t => t.electiveListID)
                .Index(t => t.courseID);
            
            CreateTable(
                "dbo.ElectiveLists",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        electiveListName = c.String(nullable: false),
                        shortName = c.String(nullable: false, maxLength: 20),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ElectiveCourses",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        electiveListID = c.Int(nullable: false),
                        degreeProgramID = c.Int(nullable: false),
                        semester = c.Int(nullable: false),
                        credits = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ElectiveLists", t => t.electiveListID, cascadeDelete: true)
                .ForeignKey("dbo.DegreePrograms", t => t.degreeProgramID, cascadeDelete: true)
                .Index(t => t.electiveListID)
                .Index(t => t.degreeProgramID);
            
            CreateTable(
                "dbo.DegreePrograms",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        degreeProgramName = c.String(nullable: false),
                        degreeProgramDescription = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.RequiredCourses",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        courseID = c.Int(nullable: false),
                        degreeProgramID = c.Int(nullable: false),
                        semester = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.DegreePrograms", t => t.degreeProgramID, cascadeDelete: true)
                .ForeignKey("dbo.Courses", t => t.courseID, cascadeDelete: true)
                .Index(t => t.degreeProgramID)
                .Index(t => t.courseID);
            
            CreateTable(
                "dbo.PrerequisiteCourses",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        prerequisiteCourseID = c.Int(nullable: false),
                        prerequisiteForCourseID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Courses", t => t.prerequisiteForCourseID, cascadeDelete: true)
                .ForeignKey("dbo.Courses", t => t.prerequisiteCourseID, cascadeDelete: false)
                .Index(t => t.prerequisiteForCourseID)
                .Index(t => t.prerequisiteCourseID);
            
            CreateTable(
                "dbo.Plans",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        planName = c.String(nullable: false),
                        degreeProgramID = c.Int(nullable: false),
                        userID = c.Int(nullable: false),
                        semesterID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.DegreePrograms", t => t.degreeProgramID, cascadeDelete: true)
                .ForeignKey("dbo.Semesters", t => t.semesterID, cascadeDelete: true)
                .Index(t => t.degreeProgramID)
                .Index(t => t.semesterID);
            
            CreateTable(
                "dbo.Semesters",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        semesterTitle = c.String(nullable: false),
                        semesterYear = c.Int(nullable: false),
                        standard = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.PlanCourses",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        notes = c.String(),
                        planID = c.Int(nullable: false),
                        courseID = c.Int(),
                        electiveListID = c.Int(),
                        semesterID = c.Int(nullable: false),
                        order = c.Int(nullable: false),
                        credits = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Courses", t => t.courseID)
                .ForeignKey("dbo.ElectiveLists", t => t.electiveListID)
                .ForeignKey("dbo.Semesters", t => t.semesterID, cascadeDelete: false)
                .ForeignKey("dbo.Plans", t => t.planID, cascadeDelete: true)
                .Index(t => t.courseID)
                .Index(t => t.electiveListID)
                .Index(t => t.semesterID)
                .Index(t => t.planID);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.PlanCourses", new[] { "planID" });
            DropIndex("dbo.PlanCourses", new[] { "semesterID" });
            DropIndex("dbo.PlanCourses", new[] { "electiveListID" });
            DropIndex("dbo.PlanCourses", new[] { "courseID" });
            DropIndex("dbo.Plans", new[] { "semesterID" });
            DropIndex("dbo.Plans", new[] { "degreeProgramID" });
            DropIndex("dbo.PrerequisiteCourses", new[] { "prerequisiteCourseID" });
            DropIndex("dbo.PrerequisiteCourses", new[] { "prerequisiteForCourseID" });
            DropIndex("dbo.RequiredCourses", new[] { "courseID" });
            DropIndex("dbo.RequiredCourses", new[] { "degreeProgramID" });
            DropIndex("dbo.ElectiveCourses", new[] { "degreeProgramID" });
            DropIndex("dbo.ElectiveCourses", new[] { "electiveListID" });
            DropIndex("dbo.ElectiveListCourses", new[] { "courseID" });
            DropIndex("dbo.ElectiveListCourses", new[] { "electiveListID" });
            DropForeignKey("dbo.PlanCourses", "planID", "dbo.Plans");
            DropForeignKey("dbo.PlanCourses", "semesterID", "dbo.Semesters");
            DropForeignKey("dbo.PlanCourses", "electiveListID", "dbo.ElectiveLists");
            DropForeignKey("dbo.PlanCourses", "courseID", "dbo.Courses");
            DropForeignKey("dbo.Plans", "semesterID", "dbo.Semesters");
            DropForeignKey("dbo.Plans", "degreeProgramID", "dbo.DegreePrograms");
            DropForeignKey("dbo.PrerequisiteCourses", "prerequisiteCourseID", "dbo.Courses");
            DropForeignKey("dbo.PrerequisiteCourses", "prerequisiteForCourseID", "dbo.Courses");
            DropForeignKey("dbo.RequiredCourses", "courseID", "dbo.Courses");
            DropForeignKey("dbo.RequiredCourses", "degreeProgramID", "dbo.DegreePrograms");
            DropForeignKey("dbo.ElectiveCourses", "degreeProgramID", "dbo.DegreePrograms");
            DropForeignKey("dbo.ElectiveCourses", "electiveListID", "dbo.ElectiveLists");
            DropForeignKey("dbo.ElectiveListCourses", "courseID", "dbo.Courses");
            DropForeignKey("dbo.ElectiveListCourses", "electiveListID", "dbo.ElectiveLists");
            DropTable("dbo.PlanCourses");
            DropTable("dbo.Semesters");
            DropTable("dbo.Plans");
            DropTable("dbo.PrerequisiteCourses");
            DropTable("dbo.RequiredCourses");
            DropTable("dbo.DegreePrograms");
            DropTable("dbo.ElectiveCourses");
            DropTable("dbo.ElectiveLists");
            DropTable("dbo.ElectiveListCourses");
            DropTable("dbo.Courses");
        }
    }
}
