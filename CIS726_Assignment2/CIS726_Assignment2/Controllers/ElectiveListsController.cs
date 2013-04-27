using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MessageParser.Models;
using PagedList;
using CIS726_Assignment2.Repositories;
using MessageParser;

namespace CIS726_Assignment2.Controllers
{
    public class ElectiveListsController : Controller
    {

        private IGenericRepository<ElectiveList> electiveLists;
        private IGenericRepository<Course> courses;
        private IGenericRepository<ElectiveListCourse> electiveListCourses;
        private ObjectMessageQueue messagequeue;
        /// <summary>
        /// Constructor used by the web application itself
        /// </summary>
        public ElectiveListsController()
        {
            messagequeue = new ObjectMessageQueue();
        }

        /// <summary>
        /// Constructor for UnitTesting (allows passing in a fake repository)
        /// </summary>
        public ElectiveListsController(IGenericRepository<ElectiveList> fakeElecList, IGenericRepository<Course> fakeCourse, IGenericRepository<ElectiveListCourse> fakeElecListCourse)
        {
            messagequeue = new ObjectMessageQueue();
        }

        //
        // GET: /ElectiveLists/

        public ActionResult Index(string sortOrder, int? page)
        {
            int pageSize = 100;
            int pageNumber = (page ?? 1);

            String currentSort = "";

            bool titleAsc = false;

            var electiveListsList = Request<ElectiveList>.GetAll("A", "B").AsEnumerable();

            if (sortOrder == null)
            {
                sortOrder = "title_asc";
            }

            String[] sorts = sortOrder.Split(';');

            int lastTitle = -1;

            for (int i = 0; i < sorts.Length; i++)
            {
                if (sorts[i].StartsWith("title"))
                {
                    if (lastTitle > 0)
                    {
                        sorts[lastTitle] = "";
                    }
                    else
                    {
                        lastTitle = i;
                    }
                }
            }

            foreach (string s in sorts)
            {
                if (s.Length <= 0)
                {
                    continue;
                }
                currentSort = currentSort + s + ";";
                if (s.Equals("title_asc"))
                {
                    electiveListsList = electiveListsList.OrderBy(x => x.electiveListName);
                    titleAsc = true;
                }
                if (s.Equals("title_desc"))
                {
                    electiveListsList = electiveListsList.OrderByDescending(x => x.electiveListName);
                    titleAsc = false;
                }
            }

            ViewBag.titleAsc = titleAsc;
            ViewBag.currentSort = currentSort;

            return View(electiveListsList.ToPagedList(pageNumber, pageSize));
        }

        //
        // GET: /ElectiveLists/Details/5

        public ActionResult Details(int id = 0)
        {

            ElectiveList electivelist = Request<ElectiveList>.GetItemByID(id, "A", "B");

            if (electivelist == null)
            {
                return HttpNotFound();
            }
            electivelist.courses = electivelist.courses.OrderBy(course => course.course.coursePrefix).ThenBy(course => course.course.courseNumber).ToList();
            return View(electivelist);
        }

        //
        // GET: /ElectiveLists/Create
        [Authorize(Roles = "Administrator")]
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /ElectiveLists/Create

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult Create(ElectiveList electivelist)
        {
            if (ModelState.IsValid)
            {
                int anid = Request<ElectiveList>.Add(electivelist, "A", "B");

                return RedirectToAction("Edit", new { id = anid });
            }

            return View(electivelist);
        }

        //
        // GET: /ElectiveLists/Edit/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(int id = 0)
        {
            ElectiveList electivelist = Request<ElectiveList>.GetItemByID(id, "A", "B");

            if (electivelist == null)
            {
                return HttpNotFound();
            }
            electivelist.courses = electivelist.courses.OrderBy(course => course.course.coursePrefix).ThenBy(course => course.course.courseNumber).ToList();
            return View(electivelist);
        }

        //
        // POST: /ElectiveLists/Edit/5

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(ElectiveList electivelist, IEnumerable<ElectiveListCourse> ElectiveListCourses)
        {
            if (ModelState.IsValid)
            {
                ElectiveList elistAttached = Request<ElectiveList>.GetItemByID(electivelist.ID, "A", "B");

                electivelist.courses = elistAttached.courses;

                if (ElectiveListCourses == null)
                {
                    ElectiveListCourses = new List<ElectiveListCourse>();
                }

                //figure out the courses that were removed from the list
                LinkedList<ElectiveListCourse> toRemove = new LinkedList<ElectiveListCourse>();
                foreach (ElectiveListCourse course in elistAttached.courses)
                {
                    if (course.ID > 0)
                    {
                        if (!ElectiveListCourses.Contains(course))
                        {
                            ElectiveListCourse elcourseAttached = Request<ElectiveListCourse>.GetItemByID(course.ID, "A", "B");
                            toRemove.AddFirst(elcourseAttached);
                        }
                    }
                }
                //remove those courses from the database
                while (toRemove.Count > 0)
                {
                    ElectiveListCourse removeme = toRemove.First();
                    toRemove.RemoveFirst();
                    Request<ElectiveListCourse>.Delete(removeme.ID, "A", "B");
                }
                //clear the list
                electivelist.courses = new List<ElectiveListCourse>();
                //add the existing courses to the database
                foreach (ElectiveListCourse elcourse in ElectiveListCourses)
                {
                    ElectiveListCourse elcourseAttached = null; ;
                    if (elcourse.ID > 0)
                    {
                        elcourseAttached = Request<ElectiveListCourse>.GetItemByID(elcourse.ID, "A", "B");
                        Request<ElectiveListCourse>.Update(elcourseAttached, elcourse, "A", "B");
                    }
                    else
                    {
                        if (courses.Find(elcourse.courseID) != null)
                        {
                            int id = Request<ElectiveListCourse>.Add(elcourse, "A", "B");
                            elcourseAttached = Request<ElectiveListCourse>.GetItemByID(id, "A", "B");
                        }
                    }
                    if (elcourseAttached != null)
                    {
                        electivelist.courses.Add(elcourseAttached);
                    }
                }
                Request<ElectiveList>.Update(elistAttached, electivelist, "A", "B");
                return RedirectToAction("Index");
            }
            if (ElectiveListCourses != null)
            {
                foreach (ElectiveListCourse course in ElectiveListCourses)
                {
                    if (course.courseID > 0)
                    {
                        course.course = Request<Course>.GetItemByID(course.courseID, "A", "B");
                    }
                }
            }
            electivelist.courses = ElectiveListCourses.ToList();
            return View(electivelist);
        }


        /// <summary>
        /// @russfeld
        /// This uses AJAX to return the HTML code for a new course row on the form
        /// Mad props to http://ivanz.com/2011/06/16/editing-variable-length-reorderable-collections-in-asp-net-mvc-part-1/
        /// </summary>
        /// <returns></returns>
        public ActionResult ElectiveListCourseRow(int id = 0)
        {
            if (id > 0)
            {
                ElectiveListCourse reqcourse = new ElectiveListCourse()
                {
                    electiveListID = id,
                    courseID = -1,
                };
                return PartialView("ElectiveListCourseFormPartial", reqcourse);
            }
            else
            {
                return PartialView("ElectiveListCourseFormPartial");
            }
        }

        //
        // GET: /ElectiveLists/Delete/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(int id = 0)
        {
            ElectiveList electivelist = Request<ElectiveList>.GetItemByID(id, "A", "B");

            if (electivelist == null)
            {
                return HttpNotFound();
            }
            electivelist.courses = electivelist.courses.OrderBy(course => course.course.coursePrefix).ThenBy(course => course.course.courseNumber).ToList();
            return View(electivelist);
        }

        //
        // POST: /ElectiveLists/Delete/5

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Administrator")]
        public ActionResult DeleteConfirmed(int id)
        {
            Request<ElectiveList>.Delete(id, "A", "B");
            return RedirectToAction("Index");
        }

        /// <summary>
        /// This is called by the autocomplete javascript function to search the elective lists
        /// </summary>
        /// <param name="term">The term to search for</param>
        /// <returns></returns>
        public JsonResult SearchElectiveLists(string term)
        {
            var keywords = Request<ElectiveList>.GetAll("A", "B").AsEnumerable();

            string[] terms = term.Split(' ');
            foreach (string t in terms)
            {
                keywords = keywords.Where(elelist => elelist.electiveListName.Contains(t));
            }
            var results = keywords.Select(elelist => new { elelist.ID, elelist.electiveListName });
            return Json(results, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}