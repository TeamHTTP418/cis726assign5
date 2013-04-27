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
using CIS726_Assignment2.ViewModels;
using MessageParser;

namespace CIS726_Assignment2.Controllers
{
    public class DegreeProgramsController : Controller
    {

        private ObjectMessageQueue messagequeue;

        /// <summary>
        /// Constructor used by the web application itself
        /// </summary>
        public DegreeProgramsController()
        {

            messagequeue = new ObjectMessageQueue();
        }

        /// <summary>
        /// Constructor for UnitTesting (allows passing in a fake repository)
        /// </summary>
        public DegreeProgramsController(IGenericRepository<DegreeProgram> fakeDegree, IGenericRepository<RequiredCourse> fakeRequired, IGenericRepository<ElectiveCourse> fakeElecCourse, IGenericRepository<ElectiveList> fakeElecList, IGenericRepository<Course> fakeCourse)
        {

            messagequeue = new ObjectMessageQueue();
        }

        //
        // GET: /DegreePrograms/

        public ActionResult Index(string sortOrder, int? page)
        {
            int pageSize = 100;
            int pageNumber = (page ?? 1);

            String currentSort = "";

            bool titleAsc = false;

            var degreeProgramList = Request<DegreeProgram>.GetAll("A", "B").AsEnumerable();

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
                    degreeProgramList = degreeProgramList.OrderBy(x => x.degreeProgramName);
                    titleAsc = true;
                }
                if (s.Equals("title_desc"))
                {
                    degreeProgramList = degreeProgramList.OrderByDescending(x => x.degreeProgramName);
                    titleAsc = false;
                }
            }

            ViewBag.titleAsc = titleAsc;
            ViewBag.currentSort = currentSort;

            return View(degreeProgramList.ToPagedList(pageNumber, pageSize));
        }

        //
        // GET: /DegreePrograms/Details/5

        public ActionResult Details(int id = 0)
        {

            DegreeProgram degreeprogram = Request<DegreeProgram>.GetItemByID(id, "A", "B");
            
            if (degreeprogram == null)
            {
                return HttpNotFound();
            }
            degreeprogram.requiredCourses = degreeprogram.requiredCourses.OrderBy(reqcourse => reqcourse.semester).ThenBy(reqcourse => reqcourse.course.coursePrefix).ThenBy(reqcourse => reqcourse.course.courseNumber).ToList();
            degreeprogram.electiveCourses = degreeprogram.electiveCourses.OrderBy(eleccourse => eleccourse.semester).ThenBy(eleccourse => eleccourse.electiveList.electiveListName).ToList();
            return View(degreeprogram);
        }

        //
        // GET: /DegreePrograms/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /DegreePrograms/Create

        [HttpPost]
        public ActionResult Create(DegreeProgram degreeprogram)
        {
            if (ModelState.IsValid)
            {
                int anid = Request<DegreeProgram>.Add(degreeprogram, "A", "B");

                return RedirectToAction("Edit", new { id = anid });
            }

            return View(degreeprogram);
        }

        //
        // GET: /DegreePrograms/Edit/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(int id = 0)
        {

            DegreeProgram degreeprogram = Request<DegreeProgram>.GetItemByID(id, "A", "B");
            if (degreeprogram == null)
            {
                return HttpNotFound();
            }
            degreeprogram.requiredCourses = degreeprogram.requiredCourses.OrderBy(reqcourse => reqcourse.semester).ThenBy(reqcourse => reqcourse.course.coursePrefix).ThenBy(reqcourse => reqcourse.course.courseNumber).ToList();
            degreeprogram.electiveCourses = degreeprogram.electiveCourses.OrderBy(eleccourse => eleccourse.semester).ThenBy(eleccourse => eleccourse.electiveList.electiveListName).ToList();
            return View(degreeprogram);
        }

        //
        // POST: /DegreePrograms/Edit/5

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(DegreeProgram degreeprogram, IEnumerable<RequiredCourse> RequiredCourses, IEnumerable<ElectiveCourse> ElectiveCourses)
        {
            if (ModelState.IsValid)
            {
                DegreeProgram degreeAttached = Request<DegreeProgram>.GetItemByID(degreeprogram.ID, "A", "B");
                degreeprogram.requiredCourses = degreeAttached.requiredCourses;
                degreeprogram.electiveCourses = degreeAttached.electiveCourses;

                if(RequiredCourses == null){
                    RequiredCourses = new List<RequiredCourse>();
                }

                if (ElectiveCourses == null)
                {
                    ElectiveCourses = new List<ElectiveCourse>();
                }

                //figures out which courses were deleted from the form
                LinkedList<RequiredCourse> toRemove = new LinkedList<RequiredCourse>();
                foreach (RequiredCourse course in degreeAttached.requiredCourses)
                {
                    if (course.ID > 0)
                    {
                        if (!RequiredCourses.Contains(course))
                        {
                            RequiredCourse reqcourseAttached = Request<RequiredCourse>.GetItemByID(course.ID, "A", "B");
                            toRemove.AddFirst(reqcourseAttached);
                        }
                    }
                }
                //deletes those courses from the list
                while(toRemove.Count > 0){
                    RequiredCourse removeme = toRemove.First();
                    toRemove.RemoveFirst();
                    Request<RequiredCourse>.Delete(removeme.ID, "A", "B");
                }
                //clears the list
                degreeprogram.requiredCourses = new List<RequiredCourse>();
                //adds the courses that exist to the list
                foreach (RequiredCourse reqcourse in RequiredCourses)
                {
                    RequiredCourse reqcourseAttached = null; ;
                    if (reqcourse.ID > 0)
                    {
                        reqcourseAttached = Request<RequiredCourse>.GetItemByID(reqcourse.ID, "A", "B");
                        Request<RequiredCourse>.Update(reqcourseAttached, reqcourse, "A", "B");
                    }
                    else
                    {
                        if (Request<Course>.GetItemByID(reqcourse.courseID, "A", "B") != null)
                        {
                            int id = Request<RequiredCourse>.Add(reqcourse, "A", "B");
                            reqcourseAttached = Request<RequiredCourse>.GetItemByID(id, "A", "B");
                        }
                    }
                    if (reqcourseAttached != null)
                    {
                        degreeprogram.requiredCourses.Add(reqcourseAttached);
                    }
                }

                //figures out which elective lists were deleted from the list
                LinkedList<ElectiveCourse> toRemoveMe = new LinkedList<ElectiveCourse>();
                foreach (ElectiveCourse course in degreeAttached.electiveCourses)
                {
                    if (!ElectiveCourses.Contains(course))
                    {
                        ElectiveCourse elcourseAttached = Request<ElectiveCourse>.GetItemByID(course.ID, "A", "B");
                        toRemoveMe.AddFirst(elcourseAttached);
                    }
                }
                //remove those elective lists from the list
                while (toRemoveMe.Count > 0)
                {
                    ElectiveCourse removeme = toRemoveMe.First();
                    toRemoveMe.RemoveFirst();
                    Request<ElectiveCourse>.Delete(removeme.ID, "A", "B");
                }
                //clear the list
                degreeprogram.electiveCourses = new List<ElectiveCourse>();
                //add the existing elective lists to the list
                foreach (ElectiveCourse elcourse in ElectiveCourses)
                {
                    ElectiveCourse elcourseAttached = null; ;
                    if (elcourse.ID > 0)
                    {
                        elcourseAttached = Request<ElectiveCourse>.GetItemByID(elcourse.ID, "A", "B");
                        Request<ElectiveCourse>.Update(elcourseAttached, elcourse, "A", "B");
                    }
                    else
                    {
                        if (Request<ElectiveList>.GetItemByID(elcourse.electiveListID, "A", "B") != null)
                        {
                            int id = Request<ElectiveCourse>.Add(elcourse, "A", "B");
                            elcourseAttached = Request<ElectiveCourse>.GetItemByID(id, "A", "B");
                        }
                    }
                    if (elcourseAttached != null)
                    {
                        degreeprogram.electiveCourses.Add(elcourseAttached);
                    }
                }

                Request<DegreeProgram>.Update(degreeAttached, degreeprogram, "A", "B");
                return RedirectToAction("Index");
            }
            if (RequiredCourses != null)
            {
                foreach (RequiredCourse course in RequiredCourses)
                {
                    if (course.courseID > 0)
                    {
                        course.course = Request<Course>.GetItemByID(course.courseID, "A", "B");
                    }
                }
            }
            degreeprogram.requiredCourses = RequiredCourses.ToList();

            if (ElectiveCourses != null)
            {
                foreach (ElectiveCourse course in ElectiveCourses)
                {
                    if (course.electiveListID > 0)
                    {
                        course.electiveList = Request<ElectiveList>.GetItemByID(course.electiveListID, "A", "B");
                    }
                }
            }
            degreeprogram.electiveCourses = ElectiveCourses.ToList();

            return View(degreeprogram);
        }

        /// <summary>
        /// @russfeld
        /// This uses AJAX to return the HTML code for a new required course row on the form
        /// Mad props to http://ivanz.com/2011/06/16/editing-variable-length-reorderable-collections-in-asp-net-mvc-part-1/
        /// </summary>
        /// <returns></returns>
        public ActionResult RequiredCourseRow(int id = 0)
        {
            if (id > 0)
            {
                RequiredCourse reqcourse = new RequiredCourse()
                {
                    degreeProgramID = id,
                    courseID = -1,
                    semester = 1
                };
                return PartialView("RequiredCourseFormPartial", reqcourse);
            }
            else
            {
                return PartialView("RequiredCourseFormPartial");
            }
        }

        /// <summary>
        /// @russfeld
        /// This uses AJAX to return the HTML code for a new elective course row on the form
        /// Mad props to http://ivanz.com/2011/06/16/editing-variable-length-reorderable-collections-in-asp-net-mvc-part-1/
        /// </summary>
        /// <returns></returns>
        public ActionResult ElectiveCourseRow(int id = 0)
        {
            if (id > 0)
            {
                ElectiveCourse elcourse = new ElectiveCourse()
                {
                    degreeProgramID = id,
                    electiveListID = -1,
                    semester = 1,
                    credits = 3,
                };
                return PartialView("ElectiveCourseFormPartial", elcourse);
            }
            else
            {
                return PartialView("ElectiveCourseFormPartial");
            }
        }

        //
        // GET: /DegreePrograms/Delete/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(int id = 0)
        {

            DegreeProgram degreeprogram = Request<DegreeProgram>.GetItemByID(id, "A", "B");
   
            if (degreeprogram == null)
            {
                return HttpNotFound();
            }
            degreeprogram.requiredCourses = degreeprogram.requiredCourses.OrderBy(reqcourse => reqcourse.semester).ThenBy(reqcourse => reqcourse.course.coursePrefix).ThenBy(reqcourse => reqcourse.course.courseNumber).ToList();
            degreeprogram.electiveCourses = degreeprogram.electiveCourses.OrderBy(eleccourse => eleccourse.semester).ThenBy(eleccourse => eleccourse.electiveList.electiveListName).ToList();
            return View(degreeprogram);
        }

        //
        // POST: /DegreePrograms/Delete/5

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Administrator")]
        public ActionResult DeleteConfirmed(int id)
        {
            Request<DegreeProgram>.Delete(id, "A", "B");

            return RedirectToAction("Index");
        }

        public JsonResult GetCourses(int id)
        {
            DegreeProgram degreeProgram = Request<DegreeProgram>.GetItemByID(id, "A", "B");

            if (degreeProgram != null)
            {
                List<FlowchartCourse> results = new List<FlowchartCourse>();
                Dictionary<int, int> semesterOrders = new Dictionary<int, int>();
                for (int i = 1; i <= 8; i++)
                {
                    semesterOrders[i] = 0;
                }

                List<RequiredCourse> requirements = degreeProgram.requiredCourses.ToList();
                foreach (RequiredCourse req in requirements)
                {
                    FlowchartCourse pcourse = new FlowchartCourse();
                    pcourse.pcourseID = req.ID;
                    pcourse.courseID = req.courseID;
                    pcourse.courseTitle = req.course.courseCatalogNumber;
                    pcourse.courseName = req.course.courseTitle;
                    int order = semesterOrders[req.semester];
                    pcourse.order = order;
                    semesterOrders[req.semester] = order + 1;
                    pcourse.semester = req.semester;
                    pcourse.hours = req.course.shortHours;
                    pcourse.prereq = new int[req.course.prerequisites.Count];
                    int place = 0;
                    foreach (PrerequisiteCourse prereq in req.course.prerequisites)
                    {
                        pcourse.prereq[place++] = prereq.prerequisiteCourseID;
                    }
                    results.Add(pcourse);
                }

                List<ElectiveCourse> elects = degreeProgram.electiveCourses.ToList();
                foreach (ElectiveCourse elect in elects)
                {
                    FlowchartCourse pcourse = new FlowchartCourse();
                    pcourse.pcourseID = elect.ID;
                    pcourse.elistID = elect.electiveListID;
                    pcourse.elistName = elect.electiveList.shortName;
                    int order = semesterOrders[elect.semester];
                    pcourse.order = order;
                    semesterOrders[elect.semester] = order + 1;
                    pcourse.semester = elect.semester;
                    pcourse.hours = elect.credits.ToString();
                    results.Add(pcourse);
                }
                return Json(results.ToArray(), JsonRequestBehavior.AllowGet);
            }
            return Json(null);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}