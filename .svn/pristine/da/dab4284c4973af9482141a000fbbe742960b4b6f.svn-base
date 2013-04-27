namespace MessageParser.Migrations
{
    using MessageParser;
    using MessageParser.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Xml;
    using System.Reflection;
    using WebMatrix.WebData;
    using System.Web.Security;


    internal sealed class Configuration : DbMigrationsConfiguration<MessageParser.Models.CourseDBContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(MessageParser.Models.CourseDBContext context)
        {
            bool seedAll = true;

            if (seedAll)
            {

                string[] semesterTitles = { "January", "Spring", "May", "Summer", "August", "Fall" };

                int semCount = 1;
                for (int i = 2013; i < 2025; i++)
                {
                    for (int k = 0; k < semesterTitles.Length; k++)
                    {
                        if (semesterTitles[k].Equals("Spring") || semesterTitles[k].Equals("Fall"))
                        {
                            context.Semesters.AddOrUpdate(c => c.ID, new Semester()
                            {
                                ID = semCount++,
                                semesterYear = i,
                                semesterTitle = semesterTitles[k],
                                standard = true
                            });
                        }
                        else
                        {
                            context.Semesters.AddOrUpdate(c => c.ID, new Semester()
                            {
                                ID = semCount++,
                                semesterYear = i,
                                semesterTitle = semesterTitles[k],
                                standard = false
                            });
                        }
                    }
                }

                context.SaveChanges();

                String resourceName = "MessageParser.SeedData.";

                XmlTextReader reader = new XmlTextReader(Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName + "ksu_courses.xml"));
                Course course = null;
                String property = null;
                int count = 0;
                Boolean done = false;
                while (reader.Read() && !done)
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element: // The node is an element.
                            if (reader.Name.Equals("htmlparse.Course"))
                            {
                                course = new Course();
                            }
                            else
                            {
                                property = reader.Name;
                            }
                            break;
                        case XmlNodeType.Text: //Display the text in each element.
                            if (property.Equals("id"))
                            {
                                course.ID = Int32.Parse(reader.Value);
                            }
                            else if (property.Equals("prefix"))
                            {
                                course.coursePrefix = reader.Value;
                            }
                            else if (property.Equals("number"))
                            {
                                course.courseNumber = Int32.Parse(reader.Value);
                            }
                            else if (property.Equals("title"))
                            {
                                course.courseTitle = reader.Value;
                            }
                            else if (property.Equals("description"))
                            {
                                course.courseDescription = reader.Value;
                            }
                            else if (property.Equals("minHours"))
                            {
                                course.minHours = int.Parse(reader.Value);
                            }
                            else if (property.Equals("maxHours"))
                            {
                                course.maxHours = int.Parse(reader.Value);
                            }
                            else if (property.Equals("variableHours"))
                            {
                                if (reader.Value.Equals("false"))
                                {
                                    course.variable = false;
                                }
                                else
                                {
                                    course.variable = true;
                                }
                            }
                            else if (property.Equals("ugrad"))
                            {
                                if (reader.Value.Equals("false"))
                                {
                                    course.undergrad = false;
                                }
                                else
                                {
                                    course.undergrad = true;
                                }
                            }
                            else if (property.Equals("grad"))
                            {
                                if (reader.Value.Equals("false"))
                                {
                                    course.graduate = false;
                                }
                                else
                                {
                                    course.graduate = true;
                                }
                            }
                            break;
                        case XmlNodeType.EndElement: //Display the end of the element.
                            if (reader.Name.Equals("htmlparse.Course"))
                            {
                                try
                                {
                                    context.Courses.AddOrUpdate(i => i.ID, course);
                                    //if (count++ > 500)
                                    //{
                                    //    done = true;
                                    //}
                                }
                                catch (System.Data.Entity.Validation.DbEntityValidationException ex)
                                {
                                    var sb = new System.Text.StringBuilder();
                                    foreach (var failure in ex.EntityValidationErrors)
                                    {
                                        sb.AppendFormat("{0} failed validation", failure.Entry.Entity.GetType());
                                        foreach (var error in failure.ValidationErrors)
                                        {
                                            sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                                            sb.AppendLine();
                                        }
                                    }
                                    throw new Exception(sb.ToString());
                                }

                            }
                            break;
                    }
                }
                reader.Close();
                context.SaveChanges();

                reader = new XmlTextReader(Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName + "ksu_electivelists.xml"));
                ElectiveList elist = null;
                property = null;
                count = 0;
                done = false;
                while (reader.Read() && !done)
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element: // The node is an element.
                            if (reader.Name.Equals("htmlparse.ElectiveList"))
                            {
                                elist = new ElectiveList();
                            }
                            else
                            {
                                property = reader.Name;
                            }
                            break;
                        case XmlNodeType.Text: //Display the text in each element.
                            if (property.Equals("id"))
                            {
                                elist.ID = Int32.Parse(reader.Value);
                            }
                            else if (property.Equals("name"))
                            {
                                elist.electiveListName = reader.Value;
                            }
                            else if (property.Equals("shortname"))
                            {
                                elist.shortName = reader.Value;
                            }
                            break;
                        case XmlNodeType.EndElement: //Display the end of the element.
                            if (reader.Name.Equals("htmlparse.ElectiveList"))
                            {
                                try
                                {
                                    context.ElectiveLists.AddOrUpdate(i => i.ID, elist);
                                    //if (count++ > 500)
                                    //{
                                    //    done = true;
                                    //}
                                }
                                catch (System.Data.Entity.Validation.DbEntityValidationException ex)
                                {
                                    var sb = new System.Text.StringBuilder();
                                    foreach (var failure in ex.EntityValidationErrors)
                                    {
                                        sb.AppendFormat("{0} failed validation", failure.Entry.Entity.GetType());
                                        foreach (var error in failure.ValidationErrors)
                                        {
                                            sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                                            sb.AppendLine();
                                        }
                                    }
                                    throw new Exception(sb.ToString());
                                }

                            }
                            break;
                    }
                }
                reader.Close();
                context.SaveChanges();

                reader = new XmlTextReader(Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName + "ksu_degreeprograms.xml"));
                DegreeProgram dp = null;
                property = null;
                count = 0;
                done = false;
                while (reader.Read() && !done)
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element: // The node is an element.
                            if (reader.Name.Equals("htmlparse.DegreeProgram"))
                            {
                                dp = new DegreeProgram();
                            }
                            else
                            {
                                property = reader.Name;
                            }
                            break;
                        case XmlNodeType.Text: //Display the text in each element.
                            if (property.Equals("id"))
                            {
                                dp.ID = Int32.Parse(reader.Value);
                            }
                            else if (property.Equals("name"))
                            {
                                dp.degreeProgramName = reader.Value;
                            }
                            else if (property.Equals("description"))
                            {
                                dp.degreeProgramDescription = reader.Value;
                            }
                            break;
                        case XmlNodeType.EndElement: //Display the end of the element.
                            if (reader.Name.Equals("htmlparse.DegreeProgram"))
                            {
                                try
                                {
                                    context.DegreePrograms.AddOrUpdate(i => i.ID, dp);
                                    //if (count++ > 500)
                                    //{
                                    //    done = true;
                                    //}
                                }
                                catch (System.Data.Entity.Validation.DbEntityValidationException ex)
                                {
                                    var sb = new System.Text.StringBuilder();
                                    foreach (var failure in ex.EntityValidationErrors)
                                    {
                                        sb.AppendFormat("{0} failed validation", failure.Entry.Entity.GetType());
                                        foreach (var error in failure.ValidationErrors)
                                        {
                                            sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                                            sb.AppendLine();
                                        }
                                    }
                                    throw new Exception(sb.ToString());
                                }

                            }
                            break;
                    }
                }
                reader.Close();
                context.SaveChanges();

                reader = new XmlTextReader(Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName + "ksu_electivelistcourses.xml"));
                ElectiveListCourse ec = null;
                property = null;
                count = 0;
                done = false;
                while (reader.Read() && !done)
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element: // The node is an element.
                            if (reader.Name.Equals("htmlparse.ElectiveListCourse"))
                            {
                                ec = new ElectiveListCourse();
                            }
                            else
                            {
                                property = reader.Name;
                            }
                            break;
                        case XmlNodeType.Text: //Display the text in each element.
                            if (property.Equals("id"))
                            {
                                ec.ID = Int32.Parse(reader.Value);
                            }
                            else if (property.Equals("course"))
                            {
                                ec.courseID = Int32.Parse(reader.Value);
                            }
                            else if (property.Equals("electivelist"))
                            {
                                ec.electiveListID = Int32.Parse(reader.Value);
                            }
                            break;
                        case XmlNodeType.EndElement: //Display the end of the element.
                            if (reader.Name.Equals("htmlparse.ElectiveListCourse"))
                            {
                                try
                                {
                                    context.ElectiveListCourses.AddOrUpdate(i => i.ID, ec);
                                    //if (count++ > 500)
                                    //{
                                    //    done = true;
                                    //}
                                }
                                catch (System.Data.Entity.Validation.DbEntityValidationException ex)
                                {
                                    var sb = new System.Text.StringBuilder();
                                    foreach (var failure in ex.EntityValidationErrors)
                                    {
                                        sb.AppendFormat("{0} failed validation", failure.Entry.Entity.GetType());
                                        foreach (var error in failure.ValidationErrors)
                                        {
                                            sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                                            sb.AppendLine();
                                        }
                                    }
                                    throw new Exception(sb.ToString());
                                }

                            }
                            break;
                    }
                }
                reader.Close();
                context.SaveChanges();

                reader = new XmlTextReader(Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName + "ksu_requiredcourses.xml"));
                RequiredCourse rq = null;
                property = null;
                count = 0;
                done = false;
                while (reader.Read() && !done)
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element: // The node is an element.
                            if (reader.Name.Equals("htmlparse.RequiredCourse"))
                            {
                                rq = new RequiredCourse();
                            }
                            else
                            {
                                property = reader.Name;
                            }
                            break;
                        case XmlNodeType.Text: //Display the text in each element.
                            if (property.Equals("id"))
                            {
                                rq.ID = Int32.Parse(reader.Value);
                            }
                            else if (property.Equals("course"))
                            {
                                rq.courseID = Int32.Parse(reader.Value);
                            }
                            else if (property.Equals("degree"))
                            {
                                rq.degreeProgramID = Int32.Parse(reader.Value);
                            }
                            else if (property.Equals("semester"))
                            {
                                rq.semester = Int32.Parse(reader.Value);
                            }
                            break;
                        case XmlNodeType.EndElement: //Display the end of the element.
                            if (reader.Name.Equals("htmlparse.RequiredCourse"))
                            {
                                try
                                {
                                    context.RequiredCourses.AddOrUpdate(i => i.ID, rq);
                                    //if (count++ > 500)
                                    //{
                                    //    done = true;
                                    //}
                                }
                                catch (System.Data.Entity.Validation.DbEntityValidationException ex)
                                {
                                    var sb = new System.Text.StringBuilder();
                                    foreach (var failure in ex.EntityValidationErrors)
                                    {
                                        sb.AppendFormat("{0} failed validation", failure.Entry.Entity.GetType());
                                        foreach (var error in failure.ValidationErrors)
                                        {
                                            sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                                            sb.AppendLine();
                                        }
                                    }
                                    throw new Exception(sb.ToString());
                                }

                            }
                            break;
                    }
                }
                reader.Close();
                context.SaveChanges();

                reader = new XmlTextReader(Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName + "ksu_electivecourses.xml"));
                ElectiveCourse ecr = null;
                property = null;
                count = 0;
                done = false;
                while (reader.Read() && !done)
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element: // The node is an element.
                            if (reader.Name.Equals("htmlparse.ElectiveCourse"))
                            {
                                ecr = new ElectiveCourse();
                            }
                            else
                            {
                                property = reader.Name;
                            }
                            break;
                        case XmlNodeType.Text: //Display the text in each element.
                            if (property.Equals("id"))
                            {
                                ecr.ID = Int32.Parse(reader.Value);
                            }
                            else if (property.Equals("degree"))
                            {
                                ecr.degreeProgramID = Int32.Parse(reader.Value);
                            }
                            else if (property.Equals("electivelist"))
                            {
                                ecr.electiveListID = Int32.Parse(reader.Value);
                            }
                            else if (property.Equals("semester"))
                            {
                                ecr.semester = Int32.Parse(reader.Value);
                            }
                            else if (property.Equals("credits"))
                            {
                                ecr.credits = Int32.Parse(reader.Value);
                            }
                            break;
                        case XmlNodeType.EndElement: //Display the end of the element.
                            if (reader.Name.Equals("htmlparse.ElectiveCourse"))
                            {
                                try
                                {
                                    context.ElectiveCourses.AddOrUpdate(i => i.ID, ecr);
                                    //if (count++ > 500)
                                    //{
                                    //    done = true;
                                    //}
                                }
                                catch (System.Data.Entity.Validation.DbEntityValidationException ex)
                                {
                                    var sb = new System.Text.StringBuilder();
                                    foreach (var failure in ex.EntityValidationErrors)
                                    {
                                        sb.AppendFormat("{0} failed validation", failure.Entry.Entity.GetType());
                                        foreach (var error in failure.ValidationErrors)
                                        {
                                            sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                                            sb.AppendLine();
                                        }
                                    }
                                    throw new Exception(sb.ToString());
                                }

                            }
                            break;
                    }
                }
                reader.Close();
                context.SaveChanges();

                reader = new XmlTextReader(Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName + "ksu_prerequisitecourses.xml"));
                PrerequisiteCourse pre = null;
                property = null;
                count = 0;
                done = false;
                while (reader.Read() && !done)
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element: // The node is an element.
                            if (reader.Name.Equals("htmlparse.PrerequisiteCourse"))
                            {
                                pre = new PrerequisiteCourse();
                            }
                            else
                            {
                                property = reader.Name;
                            }
                            break;
                        case XmlNodeType.Text: //Display the text in each element.
                            if (property.Equals("id"))
                            {
                                pre.ID = Int32.Parse(reader.Value);
                            }
                            else if (property.Equals("prerequisiteCourse"))
                            {
                                pre.prerequisiteCourseID = Int32.Parse(reader.Value);
                            }
                            else if (property.Equals("prerequisiteFor"))
                            {
                                pre.prerequisiteForCourseID = Int32.Parse(reader.Value);
                            }
                            break;
                        case XmlNodeType.EndElement: //Display the end of the element.
                            if (reader.Name.Equals("htmlparse.PrerequisiteCourse"))
                            {
                                try
                                {
                                    context.PrerequisiteCourses.AddOrUpdate(i => i.ID, pre);
                                    //if (count++ > 500)
                                    //{
                                    //    done = true;
                                    //}
                                }
                                catch (System.Data.Entity.Validation.DbEntityValidationException ex)
                                {
                                    var sb = new System.Text.StringBuilder();
                                    foreach (var failure in ex.EntityValidationErrors)
                                    {
                                        sb.AppendFormat("{0} failed validation", failure.Entry.Entity.GetType());
                                        foreach (var error in failure.ValidationErrors)
                                        {
                                            sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                                            sb.AppendLine();
                                        }
                                    }
                                    throw new Exception(sb.ToString());
                                }

                            }
                            break;
                    }
                }
                reader.Close();
                context.SaveChanges();
            }
        }
    }
}
