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
    public class ElectiveListsControllerTest
    {

        private ElectiveListsController controller;
        private IGenericRepository<ElectiveList> electiveLists;
        private IGenericRepository<Course> courses;
        private IGenericRepository<ElectiveListCourse> electiveListCourses;

        [TestInitialize]
        public void Initialize()
        {
            electiveLists = new GenericRepository<ElectiveList>(new FakeStorageContext<ElectiveList>());
            courses = new GenericRepository<Course>(new FakeStorageContext<Course>());
            electiveListCourses = new GenericRepository<ElectiveListCourse>(new FakeStorageContext<ElectiveListCourse>());

            controller = new ElectiveListsController(electiveLists, courses, electiveListCourses);

            courses.Add(new Course(){
                ID = 1,
                coursePrefix = "AAA",
                courseNumber = 123,
                courseTitle = "Test Course 1",
                courseDescription = "This is a test course for the testing framework.",
                minHours = 3,
                maxHours = 4,
                undergrad = true,
                variable = false,
                electiveLists = new Collection<ElectiveListCourse>(),
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
                electiveLists = new Collection<ElectiveListCourse>()
            });
            courses.Add(new Course(){
                ID = 3,
                coursePrefix = "AAA",
                courseNumber = 122,
                courseTitle = "Test Course 3",
                courseDescription = "This is a test course for the testing framework.",
                minHours = 7,
                maxHours = 8,
                undergrad = true,
                variable = true,
                electiveLists = new Collection<ElectiveListCourse>()
            });

            electiveLists.Add(new ElectiveList()
            {
                ID = 1,
                electiveListName = "Elective List 1",
                courses = new Collection<ElectiveListCourse>(),
            });
            electiveLists.Add(new ElectiveList()
            {
                ID = 2,
                electiveListName = "Elective List 2",
                courses = new Collection<ElectiveListCourse>(),
            });
            
            electiveListCourses.Add(new ElectiveListCourse()
            {
                ID = 1,
                electiveListID = 1,
                courseID = 1,
                course = courses.Find(1),
            });
            electiveListCourses.Add(new ElectiveListCourse()
            {
                ID = 2,
                electiveListID = 1,
                courseID = 2,
                course = courses.Find(2),
            });
            electiveListCourses.Add(new ElectiveListCourse()
            {
                ID = 3,
                electiveListID = 3,
                courseID = 3,
                course = courses.Find(3),
            });

            electiveLists.Find(1).courses.Add(electiveListCourses.Find(1));
            electiveLists.Find(1).courses.Add(electiveListCourses.Find(2));
            electiveLists.Find(2).courses.Add(electiveListCourses.Find(3));

            courses.Find(1).electiveLists.Add(electiveListCourses.Find(1));
            courses.Find(2).electiveLists.Add(electiveListCourses.Find(2));
            courses.Find(3).electiveLists.Add(electiveListCourses.Find(3));
        }

        private ElectiveList createTempElectiveList()
        {
            ElectiveList temp = new ElectiveList()
            {
                ID = 3,
                electiveListName = "Elective List 3",
                courses = new Collection<ElectiveListCourse>(),
            };
            return temp;
        }

        [TestMethod]
        public void ElectiveListsControllerIndexReturnsView()
        {
            var result = controller.Index("",1);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void ElectiveListsControllerIndexModelIsElectiveList()
        {
            ViewResult result = controller.Index("", 1) as ViewResult;
            Assert.IsInstanceOfType(result.Model, typeof(PagedList.IPagedList<MessageParser.Models.ElectiveList>));
        }

        [TestMethod]
        public void ElectiveListsControllerSortByElectiveListTitleAsc()
        {
            ViewResult result = controller.Index("title_asc", 1) as ViewResult;
            PagedList.IPagedList<MessageParser.Models.ElectiveList> model = result.Model as PagedList.IPagedList<MessageParser.Models.ElectiveList>;
            ElectiveList first = model[0];
            ElectiveList second = model[1];
            Assert.IsTrue(first.electiveListName.CompareTo(second.electiveListName) < 0);
        }

        [TestMethod]
        public void ElectiveListsControllerSortByElectiveListTitleDesc()
        {
            ViewResult result = controller.Index("title_desc", 1) as ViewResult;
            PagedList.IPagedList<MessageParser.Models.ElectiveList> model = result.Model as PagedList.IPagedList<MessageParser.Models.ElectiveList>;
            ElectiveList first = model[0];
            ElectiveList second = model[1];
            Assert.IsTrue(first.electiveListName.CompareTo(second.electiveListName) > 0);
        }

        [TestMethod]
        public void ElectiveListsControllerDetailsReturnsView()
        {
            var result = controller.Details(1);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void ElectiveListControllerDetailsModelIsElectiveList()
        {
            ViewResult result = controller.Details(1) as ViewResult;
            Assert.IsInstanceOfType(result.Model, typeof(MessageParser.Models.ElectiveList));
        }

        [TestMethod]
        public void ElectiveListsControllerDetailsFindByID()
        {
            ViewResult result = controller.Details(1) as ViewResult;
            ElectiveList model = result.Model as ElectiveList;
            Assert.IsTrue(model.electiveListName.Equals("Elective List 1"));
            result = controller.Details(2) as ViewResult;
            model = result.Model as ElectiveList;
            Assert.IsTrue(model.electiveListName.Equals("Elective List 2"));
        }

        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void ElectiveListsControllerDetailsFailsOnInvalidID()
        {
            ViewResult result = controller.Details(5) as ViewResult;
        }

        [TestMethod]
        public void ElectiveListsControllerCreateReturnsView()
        {
            var result = controller.Create();
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void ElectiveListsControllerCreateSuccessForward()
        {
            ElectiveList temp = createTempElectiveList();
            var result = controller.Create(temp);
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
        }

        [TestMethod]
        public void ElectiveListsControllerEditReturnsView()
        {
            var result = controller.Edit(1);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void ElectiveListsControllerEditSuccessForward()
        {
            ElectiveList temp = electiveLists.Find(1);
            var result = controller.Edit(temp, null);
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
        }

        [TestMethod]
        public void ElectiveListsControllerEditModelIsElectiveList()
        {
            ViewResult result = controller.Edit(1) as ViewResult;
            Assert.IsInstanceOfType(result.Model, typeof(MessageParser.Models.ElectiveList));
        }

        [TestMethod]
        public void ElectiveListsControllerEditFindByID()
        {
            ViewResult result = controller.Edit(1) as ViewResult;
            ElectiveList model = result.Model as ElectiveList;
            Assert.IsTrue(model.electiveListName.Equals("Elective List 1"));
            result = controller.Edit(2) as ViewResult;
            model = result.Model as ElectiveList;
            Assert.IsTrue(model.electiveListName.Equals("Elective List 2"));
        }

        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void ElectiveListsControllerEditFailsOnInvalidID()
        {
            ViewResult result = controller.Edit(5) as ViewResult;
        }

        [TestMethod]
        public void ElectiveListsControllerDeleteReturnsView()
        {
            var result = controller.Delete(1);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void ElectiveListsControllerDeleteSuccessForward()
        {
            var result = controller.DeleteConfirmed(2);
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
        }

        [TestMethod]
        public void ElectiveListsControllerDeleteModelIsElectiveList()
        {
            ViewResult result = controller.Delete(1) as ViewResult;
            Assert.IsInstanceOfType(result.Model, typeof(MessageParser.Models.ElectiveList));
        }

        [TestMethod]
        public void ElectiveListsControllerDeleteFindByID()
        {
            ViewResult result = controller.Edit(1) as ViewResult;
            ElectiveList model = result.Model as ElectiveList;
            Assert.IsTrue(model.electiveListName.Equals("Elective List 1"));
            result = controller.Edit(2) as ViewResult;
            model = result.Model as ElectiveList;
            Assert.IsTrue(model.electiveListName.Equals("Elective List 2"));
        }

        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void ElectiveListsControllerDeleteFailsOnInvalidID()
        {
            ViewResult result = controller.Delete(5) as ViewResult;
        }
    }
}
