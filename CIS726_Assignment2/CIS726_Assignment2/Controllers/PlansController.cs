using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MessageParser.Models;
using AuthParser.Models;
using CIS726_Assignment2.ViewModels;
using CIS726_Assignment2.Repositories;
using PagedList;
using System.Net;
using MessageParser;

namespace CIS726_Assignment2.Controllers
{
    public class PlansController : Controller
    {

        private IRoles roles;
        private IWebSecurity webSecurity;


        ObjectMessageQueue messagequeue;

        public PlansController()
        {


            roles = new RolesImpl();
            webSecurity = new WebSecurityImpl();

            messagequeue = new ObjectMessageQueue();
        }


        public PlansController(IGenericRepository<Plan> fakePlan, IGenericRepository<PlanCourse> fakePlanCourse, IGenericRepository<Semester> fakeSem, IGenericRepository<User> fakeUser, IGenericRepository<DegreeProgram> fakeDegree, IRoles fakeRoles, IWebSecurity fakeWebSecurity)
        {

            roles = fakeRoles;
            webSecurity = fakeWebSecurity;

            messagequeue = new ObjectMessageQueue();
        }
      
        //
        // GET: /Plans/
        [Authorize]
        public ActionResult Index(string sortOrder, int? page)
        {
            int pageSize = 100;
            int pageNumber = (page ?? 1);

            String currentSort = "";

            bool titleAsc = false;


            IEnumerable<Plan> plansList2 = Request<Plan>.GetAll("A", "B").AsEnumerable();
            IQueryable<User> users = Request<User>.GetAllUserRoles("A", "B").AsQueryable();
            List<PlanWithUser> plansList3 = new List<PlanWithUser>();


            foreach (Plan plan in plansList2)
            {
                User user = users.Where(u => u.ID == plan.userID).FirstOrDefault();
                plansList3.Add(new PlanWithUser(plan, user));
            }

            IEnumerable<PlanWithUser> plansList = plansList3.AsEnumerable();

            if (!webSecurity.CurrentUser.IsInRole("Advisor"))
            {
                int id = webSecurity.CurrentUserId;
                plansList = plansList.Where(s => s.userID == id);
            }

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
                    //todo
                    //plansList = plansList.OrderBy(x => x.user.username);
                    titleAsc = true;
                }
                if (s.Equals("title_desc"))
                {
                    //todo
                    //plansList = plansList.OrderByDescending(x => x.user.username);
                    titleAsc = false;
                }
            }

            ViewBag.titleAsc = titleAsc;
            ViewBag.currentSort = currentSort;

            return View(plansList.ToPagedList(pageNumber, pageSize));
        }

        //
        // GET: /Plans/Details/5
        [Authorize]
        public ActionResult Details(int id = 0)
        {

            Plan plan = Request<Plan>.GetItemByID(id, "A", "B");
            PlanWithUser planwu = new PlanWithUser(plan, Request<User>.GetUserByID(plan.userID, "A", "B"));
            if (plan == null)
            {
                return HttpNotFound();
            }
            if (webSecurity.CurrentUser.IsInRole("Advisor") || plan.userID == webSecurity.CurrentUserId)
            {
               
                var semesterList = Request<Semester>.GetAll("A", "B").Where(s => s.ID > plan.semesterID);
                ViewBag.semesterID = new SelectList(semesterList.AsEnumerable(), "ID", "semesterName");
                return View(planwu);
            }
            else
            {
                return HttpNotFound();
            }
        }

        //
        // GET: /Plans/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.degreeProgramID = new SelectList(Request<DegreeProgram>.GetAll("A", "B").AsEnumerable(), "ID", "degreeProgramName");
            if (webSecurity.CurrentUser.IsInRole("Advisor"))
            {
                ViewBag.userID = new SelectList(Request<User>.GetAllUserRoles("A", "B").AsEnumerable(), "ID", "username");
                ViewBag.Advisor = true;
            }
            else
            {
                ViewBag.userID = webSecurity.CurrentUserId;
                ViewBag.Advisor = false;
            }
            ViewBag.semesterID = new SelectList(Request<Semester>.GetAll("A", "B").Where(i => i.standard == true).AsEnumerable(), "ID", "semesterName");
            return View();
        }

        //
        // POST: /Plans/Create

        [HttpPost]
        [Authorize]
        public ActionResult Create(Plan plan)
        {
            if (ModelState.IsValid)
            {
                int id = Request<Plan>.Add(plan, "A", "B");
                Plan newPlan = Request<Plan>.GetItemByID(id, "A", "B");
                newPlan.degreeProgram = Request<DegreeProgram>.GetItemByID(newPlan.degreeProgramID, "A", "B");
                ChangeDegreeProgram(newPlan);
                return RedirectToAction("Index");
            }
          
            ViewBag.degreeProgramID = new SelectList(Request<DegreeProgram>.GetAll("A", "B").AsEnumerable(), "ID", "degreeProgramName", plan.degreeProgramID);
            if (webSecurity.CurrentUser.IsInRole("Advisor"))
            {
                ViewBag.userID = new SelectList(Request<User>.GetAllUserRoles("A", "B").AsEnumerable(), "ID", "username");
                ViewBag.Advisor = true;
            }
            else
            {
                ViewBag.userID = webSecurity.CurrentUserId;
                ViewBag.Advisor = false;
            }
            ViewBag.semesterID = new SelectList(Request<Semester>.GetAll("A", "B").Where(i => i.standard == true).AsEnumerable(), "ID", "semesterName");
            return View(plan);
        }

        //
        // GET: /Plans/Edit/5
        [Authorize]
        public ActionResult Edit(int id = 0)
        {

            Plan plan = Request<Plan>.GetItemByID(id, "A", "B");
            //Plan plan = plans.Find(id);
            if (plan == null)
            {
                return HttpNotFound();
            }
            if (webSecurity.CurrentUser.IsInRole("Advisor") || plan.userID == webSecurity.CurrentUserId)
            {
                ViewBag.degreeProgramID = new SelectList(Request<DegreeProgram>.GetAll("A", "B").AsEnumerable(), "ID", "degreeProgramName", plan.degreeProgramID);
                if (webSecurity.CurrentUser.IsInRole("Advisor"))
                {
                    ViewBag.userID = new SelectList(Request<User>.GetAllUserRoles("A", "B").AsEnumerable(), "ID", "username", plan.userID);
                    ViewBag.Advisor = true;
                }
                else
                {
                    ViewBag.userID = webSecurity.CurrentUserId;
                    ViewBag.Advisor = false;
                }
                ViewBag.semesterID = new SelectList(Request<Semester>.GetAll("A", "B").Where(i => i.standard == true).AsEnumerable(), "ID", "semesterName", plan.semesterID);
                return View(plan);
            }
            else
            {
                return HttpNotFound();
            }
        }

        //
        // POST: /Plans/Edit/5

        [HttpPost]
        [Authorize]
        public ActionResult Edit(Plan plan)
        {
            if (ModelState.IsValid)
            {
                Plan planAttached = Request<Plan>.GetItemByID(plan.ID, "A", "B");
                plan.userID = planAttached.userID;
                if (webSecurity.CurrentUser.IsInRole("Advisor") || plan.userID == webSecurity.CurrentUserId)
                {
                    Request<Plan>.Update(planAttached, plan, "A", "B");
                    Plan newPlan = Request<Plan>.GetItemByID(plan.ID, "A", "B");
                    newPlan.degreeProgram = Request<DegreeProgram>.GetItemByID(newPlan.degreeProgramID, "A", "B");
                    ChangeDegreeProgram(newPlan);
                    return RedirectToAction("Index");
                }
            }
            ViewBag.degreeProgramID = new SelectList(Request<DegreeProgram>.GetAll("A", "B").AsEnumerable(), "ID", "degreeProgramName", plan.degreeProgramID);
            if (webSecurity.CurrentUser.IsInRole("Advisor"))
            {
                ViewBag.userID = new SelectList(Request<User>.GetAllUserRoles("A", "B").AsEnumerable(), "ID", "username", plan.userID);
                ViewBag.Advisor = true;
            }
            else
            {
                ViewBag.userID = webSecurity.CurrentUserId;
                ViewBag.Advisor = false;
            }
            ViewBag.semesterID = new SelectList(Request<Semester>.GetAll("A", "B").Where(i => i.standard == true).AsEnumerable(), "ID", "semesterName", plan.semesterID);
            return View(plan);
        }

        private void ChangeDegreeProgram(Plan plan)
        {
            List<PlanCourse> plans = Request<PlanCourse>.GetAll("A", "B").Where(i => i.planID == plan.ID).ToList();//planCourses.Where(i => i.planID == plan.ID).ToList();
            foreach (PlanCourse planCourse in plans)
            {
                Request<PlanCourse>.Delete(planCourse.ID, "A", "B");
            }
            Dictionary<int, int> semesterOrders = new Dictionary<int, int>();
            Dictionary<int, int> semesterMap = new Dictionary<int, int>();
            int nowSem = 1;
            List<Semester> semesterList = Request<Semester>.GetAll("A", "B").Where(i => i.ID >= plan.semesterID).ToList();
            foreach (Semester sem in semesterList)
            {
                if (sem.standard == true)
                {
                    semesterMap.Add(nowSem, sem.ID);
                    semesterOrders.Add(nowSem, 0);
                    nowSem++;
                }
            }

            List<RequiredCourse> requirements = plan.degreeProgram.requiredCourses.ToList();
            foreach (RequiredCourse req in requirements)
            {
                PlanCourse pcourse = new PlanCourse();
                pcourse.planID = plan.ID;
                int order = semesterOrders[req.semester];
                pcourse.order = order;
                semesterOrders[req.semester] = order + 1;
                pcourse.semesterID = semesterMap[req.semester];
                pcourse.courseID = req.courseID;
                pcourse.credits = req.course.courseHours;
                Request<PlanCourse>.Add(pcourse, "A", "B");
            }

            List<ElectiveCourse> elects = plan.degreeProgram.electiveCourses.ToList();
            foreach (ElectiveCourse elect in elects)
            {
                PlanCourse pcourse = new PlanCourse();
                pcourse.planID = plan.ID;
                int order = semesterOrders[elect.semester];
                pcourse.order = order;
                semesterOrders[elect.semester] = order + 1;
                pcourse.semesterID = semesterMap[elect.semester];
                pcourse.electiveListID = elect.electiveListID;
                pcourse.credits = elect.credits.ToString();
                Request<PlanCourse>.Add(pcourse, "A", "B");
            }
        }

        //
        // GET: /Plans/Delete/5
        [Authorize]
        public ActionResult Delete(int id = 0)
        {

            Plan plan = Request<Plan>.GetItemByID(id, "A", "B");
            PlanWithUser planwu = new PlanWithUser(plan, Request<User>.GetUserByID(plan.userID, "A", "B"));

            if (plan == null)
            {
                return HttpNotFound();
            }
            if (webSecurity.CurrentUser.IsInRole("Advisor") || plan.userID == webSecurity.CurrentUserId)
            {
                return View(planwu);
            }
            else
            {
                return HttpNotFound();
            }
        }

        //
        // POST: /Plans/Delete/5

        [HttpPost, ActionName("Delete")]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            Plan plan = Request<Plan>.GetItemByID(id, "A", "B");
            if (webSecurity.CurrentUser.IsInRole("Advisor") || plan.userID == webSecurity.CurrentUserId)
            {
                Request<Plan>.Delete(plan.ID, "A", "B");
            }
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult UpdateCourseInfo(int id = 0)
        {
            if (id > 0)
            {
                PlanCourse pcourse = Request<PlanCourse>.GetItemByID(id, "A", "B");
                if (pcourse != null)
                {
                    Plan planAttached = Request<Plan>.GetItemByID(pcourse.planID, "A", "B");
                    if (webSecurity.CurrentUser.IsInRole("Advisor") || planAttached.userID == webSecurity.CurrentUserId)
                    {
                        if (pcourse.electiveListID != null)
                        {
                            List<DropdownCourse> courses = new List<DropdownCourse>();
                            List<ElectiveListCourse> elistCourses = pcourse.electiveList.courses.ToList();
                            foreach (ElectiveListCourse elistc in elistCourses)
                            {
                                DropdownCourse now = new DropdownCourse();
                                now.ID = elistc.courseID;
                                now.courseHeader = elistc.course.courseHeader;
                                courses.Add(now);
                            }
                            if (pcourse.courseID != null)
                            {
                                ViewBag.courseID = new SelectList(courses.AsEnumerable(), "ID", "courseHeader", pcourse.courseID);
                            }
                            else
                            {
                                ViewBag.courseID = new SelectList(courses.AsEnumerable(), "ID", "courseHeader");
                            }
                        }
                        return PartialView("PlanCourseFormPartial", new PlanCourseEdit(pcourse));
                    }
                }
            }
            return HttpNotFound();
        }

        [HttpPost]
        [Authorize]
        public ActionResult MoveCourse(int ID, int semester, int order)
        {
            PlanCourse pcourseAttached = Request<PlanCourse>.GetItemByID(ID, "A", "B");
            Plan planAttached = Request<Plan>.GetItemByID(pcourseAttached.planID, "A", "B");
            if (webSecurity.CurrentUser.IsInRole("Advisor") || planAttached.userID == webSecurity.CurrentUserId)
            {
                if (Request<Semester>.GetItemByID(semester, "A", "B") != null)
                {
                    int oldSemester = pcourseAttached.semesterID;
                    int oldOrder = pcourseAttached.order;
                    if (semester != oldSemester)
                    {
                        List<PlanCourse> moveUp = planAttached.planCourses.Where(s => s.semesterID == oldSemester).ToList();
                        foreach (PlanCourse pc in moveUp)
                        {
                            if (pc.order > oldOrder)
                            {
                                pc.order = pc.order - 1;
                                Request<PlanCourse>.Update(pc, pc, "A", "B");
                            }
                        }
                        List<PlanCourse> moveDown = planAttached.planCourses.Where(s => s.semesterID == semester).ToList();
                        foreach (PlanCourse pc in moveDown)
                        {
                            if (pc.order >= order)
                            {
                                pc.order = pc.order + 1;
                                Request<PlanCourse>.Update(pc, pc, "A", "B");
                            }
                        }
                    }
                    else
                    {
                        if (oldOrder < order)
                        {
                            List<PlanCourse> moveUp = planAttached.planCourses.Where(s => s.semesterID == oldSemester).ToList();
                            foreach (PlanCourse pc in moveUp)
                            {
                                if (pc.order > oldOrder && pc.order <= order)
                                {
                                    pc.order = pc.order - 1;
                                    Request<PlanCourse>.Update(pc, pc, "A", "B");
                                }
                            }
                        }
                        else if (oldOrder > order)
                        {
                            List<PlanCourse> moveDown = planAttached.planCourses.Where(s => s.semesterID == oldSemester).ToList();
                            foreach (PlanCourse pc in moveDown)
                            {
                                if (pc.order >= order && pc.order < order)
                                {
                                    pc.order = pc.order + 1;
                                    Request<PlanCourse>.Update(pc, pc, "A", "B");
                                }
                            }
                        }
                    }
                    pcourseAttached.semesterID = semester;
                    if (order < 7)
                    {
                        pcourseAttached.order = order;
                    }
                    else
                    {
                        pcourseAttached.order = 7;
                    }
                    Request<PlanCourse>.Update(pcourseAttached, pcourseAttached, "A", "B");
                    return new HttpStatusCodeResult(HttpStatusCode.OK);
                }
            }
            return new HttpStatusCodeResult(HttpStatusCode.NotFound);
        }

        [HttpPost]
        [Authorize]
        public ActionResult SaveCourseInfo(int ID, int? courseID, string notes)
        {
            if (ModelState.IsValid)
            {
                PlanCourse pcourseAttached = Request<PlanCourse>.GetItemByID(ID, "A", "B");
                Plan planAttached = Request<Plan>.GetItemByID(pcourseAttached.planID, "A", "B");
                if (webSecurity.CurrentUser.IsInRole("Advisor") || planAttached.userID == webSecurity.CurrentUserId)
                {
                    pcourseAttached.notes = notes;
                    pcourseAttached.courseID = courseID;
                    Request<PlanCourse>.Update(pcourseAttached, pcourseAttached, "A", "B");
                    return new HttpStatusCodeResult(HttpStatusCode.OK);
                }
            }
            return new HttpStatusCodeResult(HttpStatusCode.NotFound);
        }

        public JsonResult GetSemesters(int id)
        {
            var sems = Request<Semester>.GetAll("A", "B").Where(i => i.ID >= id).ToList();
            var results = sems.Select(sem => new { sem.ID, sem.semesterName });
            return Json(results, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public JsonResult GetPlanCourses(int id)
        {
            Plan plan = Request<Plan>.GetItemByID(id, "A", "B");
            if (webSecurity.CurrentUser.IsInRole("Advisor") || plan.userID == webSecurity.CurrentUserId){
                List<FlowchartCourse> results = new List<FlowchartCourse>();
                foreach (PlanCourse pcourse in plan.planCourses)
                {
                    FlowchartCourse here = new FlowchartCourse();
                    here.order = pcourse.order;
                    here.semester = pcourse.semesterID;
                    here.hours = pcourse.credits;
                    here.pcourseID = pcourse.ID;
                    if (pcourse.courseID != null)
                    {
                        here.courseID = (int)pcourse.courseID;
                        here.courseTitle = pcourse.course.courseCatalogNumber;
                        here.courseName = pcourse.course.courseTitle;
                        here.prereq = new int[pcourse.course.prerequisites.Count];
                        int place = 0;
                        foreach(PrerequisiteCourse prereq in pcourse.course.prerequisites){
                            here.prereq[place++] = prereq.prerequisiteCourseID;
                        }
                    }
                    if (pcourse.electiveListID != null)
                    {
                        here.elistID = (int)pcourse.electiveListID;
                        here.elistName = pcourse.electiveList.shortName;
                    }
                    results.Add(here);
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