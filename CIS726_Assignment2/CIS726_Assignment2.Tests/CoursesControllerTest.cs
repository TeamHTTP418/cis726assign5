using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CIS726_Assignment2.Controllers;
using CIS726_Assignment2.Tests.Fakes;
using CIS726_Assignment2.Repositories;
using MessageParser.Models;
using System.Web.Mvc;
using PagedList;

namespace CIS726_Assignment2.Tests
{
    [TestClass]
    public class CoursesControllerTest
    {
        private CoursesController controller;
        private GenericRepository<Course> storage;
        private GenericRepository<PrerequisiteCourse> prereqs;

        [TestInitialize]
        public void Initialize()
        {
            storage = new GenericRepository<Course>(new FakeStorageContext<Course>());
            prereqs = new GenericRepository<PrerequisiteCourse>(new FakeStorageContext<PrerequisiteCourse>());
            controller = new CoursesController(storage, prereqs);
            storage.Add(new Course(){
                ID = 1,
                coursePrefix = "AAA",
                courseNumber = 123,
                courseTitle = "Test Course 1",
                courseDescription = "This is a test course for the testing framework.",
                minHours = 3,
                maxHours = 4,
                undergrad = true,
                variable = false
            });
            storage.Add(new Course()
            {
                ID = 2,
                coursePrefix = "BBB",
                courseNumber = 456,
                courseTitle = "Test Course 2",
                courseDescription = "This is a test course for the testing framework.",
                minHours = 5,
                maxHours = 6,
                graduate = true,
                variable = false
            });
            storage.Add(new Course(){
                ID = 3,
                coursePrefix = "AAA",
                courseNumber = 122,
                courseTitle = "Test Course 3",
                courseDescription = "This is a test course for the testing framework.",
                minHours = 7,
                maxHours = 8,
                undergrad = true,
                variable = true
            });
        }

        private Course createTempCourse()
        {
            Course temp = new Course()
            {
                ID = 4,
                courseNumber = 789,
                coursePrefix = "CCC",
                courseTitle = "Test Created Course",
                courseDescription = "Created Course for testing Methods",
                minHours = 3,
                maxHours = 4
            };
            return temp;
        }

        [TestMethod]
        public void CoursesControllerIndexReturnsView()
        {
            var result = controller.Index("",1,"");
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void CoursesControllerIndexModelIsCourse()
        {
            ViewResult result = controller.Index("", 1, "") as ViewResult;
            Assert.IsInstanceOfType(result.Model, typeof(PagedList.IPagedList<MessageParser.Models.Course>));
        }

        [TestMethod]
        public void CoursesControllerSortByCourseNumberAsc()
        {
            ViewResult result = controller.Index("num_asc", 1, "") as ViewResult;
            PagedList.IPagedList<MessageParser.Models.Course> model = result.Model as PagedList.IPagedList<MessageParser.Models.Course>;
            Course first = model[0];
            Course second = model[1];
            Assert.IsTrue(first.courseCatalogNumber.CompareTo(second.courseCatalogNumber) < 0);
            first = model[1];
            second = model[2];
            Assert.IsTrue(first.courseCatalogNumber.CompareTo(second.courseCatalogNumber) < 0);
        }

        [TestMethod]
        public void CoursesControllerSortByCourseNumberDesc()
        {
            ViewResult result = controller.Index("num_desc", 1, "") as ViewResult;
            PagedList.IPagedList<MessageParser.Models.Course> model = result.Model as PagedList.IPagedList<MessageParser.Models.Course>;
            Course first = model[0];
            Course second = model[1];
            Assert.IsTrue(first.courseCatalogNumber.CompareTo(second.courseCatalogNumber) > 0);
            first = model[1];
            second = model[2];
            Assert.IsTrue(first.courseCatalogNumber.CompareTo(second.courseCatalogNumber) > 0);
        }

        [TestMethod]
        public void CoursesControllerSortByCourseTitleAsc()
        {
            ViewResult result = controller.Index("title_asc", 1, "") as ViewResult;
            PagedList.IPagedList<MessageParser.Models.Course> model = result.Model as PagedList.IPagedList<MessageParser.Models.Course>;
            Course first = model[0];
            Course second = model[1];
            Assert.IsTrue(first.courseTitle.CompareTo(second.courseTitle) < 0);
            first = model[1];
            second = model[2];
            Assert.IsTrue(first.courseTitle.CompareTo(second.courseTitle) < 0);
        }

        [TestMethod]
        public void CoursesControllerSortByCourseTitleDesc()
        {
            ViewResult result = controller.Index("title_desc", 1, "") as ViewResult;
            PagedList.IPagedList<MessageParser.Models.Course> model = result.Model as PagedList.IPagedList<MessageParser.Models.Course>;
            Course first = model[0];
            Course second = model[1];
            Assert.IsTrue(first.courseTitle.CompareTo(second.courseTitle) > 0);
            first = model[1];
            second = model[2];
            Assert.IsTrue(first.courseTitle.CompareTo(second.courseTitle) > 0);
        }

        [TestMethod]
        public void CoursesControllerSortByCourseHoursAsc()
        {
            ViewResult result = controller.Index("hours_asc", 1, "") as ViewResult;
            PagedList.IPagedList<MessageParser.Models.Course> model = result.Model as PagedList.IPagedList<MessageParser.Models.Course>;
            Course first = model[0];
            Course second = model[1];
            Assert.IsTrue(first.courseHours.CompareTo(second.courseHours) < 0);
            first = model[1];
            second = model[2];
            Assert.IsTrue(first.courseHours.CompareTo(second.courseHours) < 0);
        }

        [TestMethod]
        public void CoursesControllerSortByCourseHoursDesc()
        {
            ViewResult result = controller.Index("hours_desc", 1, "") as ViewResult;
            PagedList.IPagedList<MessageParser.Models.Course> model = result.Model as PagedList.IPagedList<MessageParser.Models.Course>;
            Course first = model[0];
            Course second = model[1];
            Assert.IsTrue(first.courseHours.CompareTo(second.courseHours) > 0);
            first = model[1];
            second = model[2];
            Assert.IsTrue(first.courseHours.CompareTo(second.courseHours) > 0);
        }

        [TestMethod]
        public void CoursesControllerFilterByPrefix()
        {
            ViewResult result = controller.Index("", 1, "prefix:AAA;") as ViewResult;
            PagedList.IPagedList<MessageParser.Models.Course> model = result.Model as PagedList.IPagedList<MessageParser.Models.Course>;
            Assert.IsTrue(model.Count == 2);
            result = controller.Index("", 1, "prefix:BBB;") as ViewResult;
            model = result.Model as PagedList.IPagedList<MessageParser.Models.Course>;
            Assert.IsTrue(model.Count == 1);
        }

        [TestMethod]
        public void CoursesControllerFilterByNumber()
        {
            ViewResult result = controller.Index("", 1, "minNum:121;maxNum:124") as ViewResult;
            PagedList.IPagedList<MessageParser.Models.Course> model = result.Model as PagedList.IPagedList<MessageParser.Models.Course>;
            Assert.IsTrue(model.Count == 2);
            result = controller.Index("", 1, "minNum:125;maxNum:998") as ViewResult;
            model = result.Model as PagedList.IPagedList<MessageParser.Models.Course>;
            Assert.IsTrue(model.Count == 1);
        }

        [TestMethod]
        public void CoursesControllerFilterByHours()
        {
            ViewResult result = controller.Index("", 1, "minHrs:3;maxHrs:6") as ViewResult;
            PagedList.IPagedList<MessageParser.Models.Course> model = result.Model as PagedList.IPagedList<MessageParser.Models.Course>;
            Assert.IsTrue(model.Count == 2);
            result = controller.Index("", 1, "minHrs:7;maxHrs:8") as ViewResult;
            model = result.Model as PagedList.IPagedList<MessageParser.Models.Course>;
            Assert.IsTrue(model.Count == 1);
        }

        [TestMethod]
        public void CoursesControllerFilterByCatalog()
        {
            ViewResult result = controller.Index("", 1, "ugrad") as ViewResult;
            PagedList.IPagedList<MessageParser.Models.Course> model = result.Model as PagedList.IPagedList<MessageParser.Models.Course>;
            Assert.IsTrue(model.Count == 2);
            result = controller.Index("", 1, "grad") as ViewResult;
            model = result.Model as PagedList.IPagedList<MessageParser.Models.Course>;
            Assert.IsTrue(model.Count == 1);
        }

        [TestMethod]
        public void CoursesControllerDetailsReturnsView()
        {
            var result = controller.Details(1);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void CoursesControllerDetailsModelIsCourse()
        {
            ViewResult result = controller.Details(1) as ViewResult;
            Assert.IsInstanceOfType(result.Model, typeof(MessageParser.Models.Course));
        }

        [TestMethod]
        public void CoursesControllerDetailsFindByID()
        {
            ViewResult result = controller.Details(1) as ViewResult;
            Course model = result.Model as Course;
            Assert.IsTrue(model.courseTitle.Equals("Test Course 1"));
            result = controller.Details(2) as ViewResult;
            model = result.Model as Course;
            Assert.IsTrue(model.courseTitle.Equals("Test Course 2"));
            result = controller.Details(3) as ViewResult;
            model = result.Model as Course;
            Assert.IsTrue(model.courseTitle.Equals("Test Course 3"));
        }

        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void CoursesControllerDetailsFailsOnInvalidID()
        {
            ViewResult result = controller.Details(5) as ViewResult;
        }

        [TestMethod]
        public void CoursesControllerCreateReturnsView()
        {
            var result = controller.Create();
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void CoursesControllerCreateSuccessForward()
        {
            Course temp = createTempCourse();
            var result = controller.Create(temp);
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
        }

        [TestMethod]
        public void CoursesControllerEditReturnsView()
        {
            var result = controller.Edit(1);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void CoursesControllerEditSuccessForward()
        {
            Course temp = storage.Find(1);
            var result = controller.Edit(temp, null);
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
        }

        [TestMethod]
        public void CoursesControllerEditModelIsCourse()
        {
            ViewResult result = controller.Edit(1) as ViewResult;
            Assert.IsInstanceOfType(result.Model, typeof(MessageParser.Models.Course));
        }

        [TestMethod]
        public void CoursesControllerEditFindByID()
        {
            ViewResult result = controller.Edit(1) as ViewResult;
            Course model = result.Model as Course;
            Assert.IsTrue(model.courseTitle.Equals("Test Course 1"));
            result = controller.Edit(2) as ViewResult;
            model = result.Model as Course;
            Assert.IsTrue(model.courseTitle.Equals("Test Course 2"));
            result = controller.Edit(3) as ViewResult;
            model = result.Model as Course;
            Assert.IsTrue(model.courseTitle.Equals("Test Course 3"));
        }

        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void CoursesControllerEditFailsOnInvalidID()
        {
            ViewResult result = controller.Edit(5) as ViewResult;
        }

        [TestMethod]
        public void CoursesControllerDeleteReturnsView()
        {
            var result = controller.Delete(1);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void CoursesControllerDeleteSuccessForward()
        {
            var result = controller.DeleteConfirmed(3);
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
        }

        [TestMethod]
        public void CoursesControllerDeleteModelIsCourse()
        {
            ViewResult result = controller.Delete(1) as ViewResult;
            Assert.IsInstanceOfType(result.Model, typeof(MessageParser.Models.Course));
        }

        [TestMethod]
        public void CoursesControllerDeleteFindByID()
        {
            ViewResult result = controller.Delete(1) as ViewResult;
            Course model = result.Model as Course;
            Assert.IsTrue(model.courseTitle.Equals("Test Course 1"));
            result = controller.Delete(2) as ViewResult;
            model = result.Model as Course;
            Assert.IsTrue(model.courseTitle.Equals("Test Course 2"));
            result = controller.Delete(3) as ViewResult;
            model = result.Model as Course;
            Assert.IsTrue(model.courseTitle.Equals("Test Course 3"));
        }

        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void CoursesControllerDeleteFailsOnInvalidID()
        {
            ViewResult result = controller.Delete(5) as ViewResult;
        }
    }
}
