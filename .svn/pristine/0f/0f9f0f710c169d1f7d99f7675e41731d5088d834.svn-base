using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CIS726_Assignment2.Controllers;
using CIS726_Assignment2.Tests.Fakes;
using MessageParser.Models;
using CIS726_Assignment2.Repositories;
using System.Web.Mvc;
using PagedList;

namespace CIS726_Assignment2.Tests
{
    [TestClass]
    public class DegreeProgramsControllerTest
    {

        private DegreeProgramsController controller;
        private IGenericRepository<DegreeProgram> degreePrograms;
        private IGenericRepository<RequiredCourse> requiredCourses;
        private IGenericRepository<ElectiveCourse> electiveCourses;
        private IGenericRepository<ElectiveList> electiveLists;
        private IGenericRepository<Course> courses;


        [TestInitialize]
        public void Initialize()
        {
            degreePrograms = new GenericRepository<DegreeProgram>(new FakeStorageContext<DegreeProgram>());
            requiredCourses = new GenericRepository<RequiredCourse>(new FakeStorageContext<RequiredCourse>());
            electiveCourses = new GenericRepository<ElectiveCourse>(new FakeStorageContext<ElectiveCourse>());
            electiveLists = new GenericRepository<ElectiveList>(new FakeStorageContext<ElectiveList>());
            courses = new GenericRepository<Course>(new FakeStorageContext<Course>());

            controller = new DegreeProgramsController(degreePrograms, requiredCourses, electiveCourses, electiveLists, courses);

            courses.Add(new Course()
            {
                ID = 1,
                coursePrefix = "AAA",
                courseNumber = 123,
                courseTitle = "Test Course 1",
                courseDescription = "This is a test course for the testing framework.",
                minHours = 3,
                maxHours = 4,
                undergrad = true,
                variable = false,
            });
            courses.Add(new Course()
            {
                ID = 2,
                coursePrefix = "BBB",
                courseNumber = 456,
                courseTitle = "Test Course 2",
                courseDescription = "This is a test course for the testing framework.",
                minHours = 5,
                maxHours = 6,
                graduate = true,
                variable = false,
            });
            courses.Add(new Course()
            {
                ID = 3,
                coursePrefix = "AAA",
                courseNumber = 122,
                courseTitle = "Test Course 3",
                courseDescription = "This is a test course for the testing framework.",
                minHours = 7,
                maxHours = 8,
                undergrad = true,
                variable = true,
            });

            electiveLists.Add(new ElectiveList()
            {
                ID = 1,
                electiveListName = "Elective List 1",
            });
            electiveLists.Add(new ElectiveList()
            {
                ID = 2,
                electiveListName = "Elective List 2",
            });

            degreePrograms.Add(new DegreeProgram()
            {
                ID = 1,
                degreeProgramName = "Degree Program 1",
                requiredCourses = new Collection<RequiredCourse>(),
                electiveCourses = new Collection<ElectiveCourse>(),
            });
            degreePrograms.Add(new DegreeProgram()
            {
                ID = 2,
                degreeProgramName = "Degree Program 2",
                requiredCourses = new Collection<RequiredCourse>(),
                electiveCourses = new Collection<ElectiveCourse>(),
            });

            requiredCourses.Add(new RequiredCourse()
            {
                ID = 1,
                courseID = 1,
                course = courses.Find(1),
                degreeProgramID = 1,
                semester = 3,
            });
            requiredCourses.Add(new RequiredCourse()
            {
                ID = 2,
                courseID = 2,
                course = courses.Find(2),
                degreeProgramID = 1,
                semester = 2,
            });
            requiredCourses.Add(new RequiredCourse()
            {
                ID = 3,
                courseID = 3,
                course = courses.Find(3),
                degreeProgramID = 1,
                semester = 1,
            });

            degreePrograms.Find(1).requiredCourses.Add(requiredCourses.Find(1));
            degreePrograms.Find(1).requiredCourses.Add(requiredCourses.Find(2));
            degreePrograms.Find(1).requiredCourses.Add(requiredCourses.Find(3));

            electiveCourses.Add(new ElectiveCourse()
            {
                ID = 1,
                electiveListID = 1,
                electiveList = electiveLists.Find(1),
                degreeProgramID = 1
            });
            electiveCourses.Add(new ElectiveCourse()
            {
                ID = 2,
                electiveListID = 2,
                electiveList = electiveLists.Find(2),
                degreeProgramID = 1
            });

            degreePrograms.Find(1).electiveCourses.Add(electiveCourses.Find(1));
            degreePrograms.Find(1).electiveCourses.Add(electiveCourses.Find(2));
        }

        private DegreeProgram createTempDegreeProgram()
        {
            DegreeProgram temp = new DegreeProgram()
            {
                ID = 3,
                degreeProgramName = "Degree Program 3",
                requiredCourses = new Collection<RequiredCourse>(),
                electiveCourses = new Collection<ElectiveCourse>(),
            };
            return temp;
        }

        [TestMethod]
        public void DegreeProgramsControllerIndexReturnsView()
        {
            var result = controller.Index("", 1);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void DegreeProgramsControllerIndexModelIsDegreeProgram()
        {
            ViewResult result = controller.Index("", 1) as ViewResult;
            Assert.IsInstanceOfType(result.Model, typeof(PagedList.IPagedList<MessageParser.Models.DegreeProgram>));
        }

        [TestMethod]
        public void DegreeProgramsControllerSortByDegreeProgramTitleAsc()
        {
            ViewResult result = controller.Index("title_asc", 1) as ViewResult;
            PagedList.IPagedList<MessageParser.Models.DegreeProgram> model = result.Model as PagedList.IPagedList<MessageParser.Models.DegreeProgram>;
            DegreeProgram first = model[0];
            DegreeProgram second = model[1];
            Assert.IsTrue(first.degreeProgramName.CompareTo(second.degreeProgramName) < 0);
        }

        [TestMethod]
        public void DegreeProgramsControllerSortByDegreeProgramTitleDesc()
        {
            ViewResult result = controller.Index("title_desc", 1) as ViewResult;
            PagedList.IPagedList<MessageParser.Models.DegreeProgram> model = result.Model as PagedList.IPagedList<MessageParser.Models.DegreeProgram>;
            DegreeProgram first = model[0];
            DegreeProgram second = model[1];
            Assert.IsTrue(first.degreeProgramName.CompareTo(second.degreeProgramName) > 0);
        }

        [TestMethod]
        public void DegreeProgramsControllerDetailsReturnsView()
        {
            var result = controller.Details(1);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void CoursesControllerDetailsModelIsDegreeProgram()
        {
            ViewResult result = controller.Details(1) as ViewResult;
            Assert.IsInstanceOfType(result.Model, typeof(MessageParser.Models.DegreeProgram));
        }

        [TestMethod]
        public void DegreeProgramsControllerDetailsFindByID()
        {
            ViewResult result = controller.Details(1) as ViewResult;
            DegreeProgram model = result.Model as DegreeProgram;
            Assert.IsTrue(model.degreeProgramName.Equals("Degree Program 1"));
            result = controller.Details(2) as ViewResult;
            model = result.Model as DegreeProgram;
            Assert.IsTrue(model.degreeProgramName.Equals("Degree Program 2"));
        }

        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void DegreeProgramsControllerDetailsFailsOnInvalidID()
        {
            ViewResult result = controller.Details(5) as ViewResult;
        }

        [TestMethod]
        public void DegreeProgramsControllerCreateReturnsView()
        {
            var result = controller.Create();
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void DegreeProgramsControllerCreateSuccessForward()
        {
            DegreeProgram temp = createTempDegreeProgram();
            var result = controller.Create(temp);
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
        }

        [TestMethod]
        public void DegreeProgramsControllerEditReturnsView()
        {
            var result = controller.Edit(1);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void DegreeProgramsControllerEditSuccessForward()
        {
            DegreeProgram temp = degreePrograms.Find(1);
            var result = controller.Edit(temp, null, null);
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
        }

        [TestMethod]
        public void DegreeProgramsControllerEditModelIsDegreeProgram()
        {
            ViewResult result = controller.Edit(1) as ViewResult;
            Assert.IsInstanceOfType(result.Model, typeof(MessageParser.Models.DegreeProgram));
        }

        [TestMethod]
        public void DegreeProgramsControllerEditFindByID()
        {
            ViewResult result = controller.Edit(1) as ViewResult;
            DegreeProgram model = result.Model as DegreeProgram;
            Assert.IsTrue(model.degreeProgramName.Equals("Degree Program 1"));
            result = controller.Edit(2) as ViewResult;
            model = result.Model as DegreeProgram;
            Assert.IsTrue(model.degreeProgramName.Equals("Degree Program 2"));
        }

        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void DegreeProgramsControllerEditFailsOnInvalidID()
        {
            ViewResult result = controller.Edit(5) as ViewResult;
        }

        [TestMethod]
        public void DegreeProgramsControllerDeleteReturnsView()
        {
            var result = controller.Delete(1);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void DegreeProgramsControllerDeleteSuccessForward()
        {
            var result = controller.DeleteConfirmed(2);
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
        }

        [TestMethod]
        public void DegreeProgramsControllerDeleteModelIsDegreeProgram()
        {
            ViewResult result = controller.Delete(1) as ViewResult;
            Assert.IsInstanceOfType(result.Model, typeof(MessageParser.Models.DegreeProgram));
        }

        [TestMethod]
        public void DegreeProgramsControllerDeleteFindByID()
        {
            ViewResult result = controller.Edit(1) as ViewResult;
            DegreeProgram model = result.Model as DegreeProgram;
            Assert.IsTrue(model.degreeProgramName.Equals("Degree Program 1"));
            result = controller.Edit(2) as ViewResult;
            model = result.Model as DegreeProgram;
            Assert.IsTrue(model.degreeProgramName.Equals("Degree Program 2"));
        }

        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void DegreeProgramsControllerDeleteFailsOnInvalidID()
        {
            ViewResult result = controller.Delete(5) as ViewResult;
        }
    }
}
