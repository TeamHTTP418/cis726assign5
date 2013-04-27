using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using MessageParser.Models;
using System.Runtime.Serialization;

namespace MessageParser
{
    public enum ModelType
    {
        Course, DegreeProgram, ElectiveCourse, ElectiveList, ElectiveListCourse, Plan, PlanCourse,
        PrerequisiteCourse, RequiredCourse, Semester, User, Role, UserRoles
    }
    public enum MethodType
    {
        GetItemByID, GetItemsByPlanID, GetAll, Add, Update, Delete, GetByName, GetAllByName, GetRolesForUser, GetUsersForRole
    }

    public class GenericRequest
    {
        public ModelType Type {get; set;}
        public MethodType Method { get; set; }
        public string Sender { get; set; }
        public string Receiver { get; set; }
        public int RequestedID { get; set; }
        public string RequestedName { get; set; }
    }

    public class Request<T> : GenericRequest
    {
        public List<T> RequestedMembers { get; set; }

        public Request()
        {
            this.Type = ModelType.Course;
            this.Method = MethodType.GetAll;
            this.Sender = "";
            this.Receiver = "";
            this.RequestedMembers = new List<T>();
        }
        public Request(ModelType m, MethodType method, string sender, string receiver)
        {
            this.Type = m;
            this.Method = method;
            this.Sender = sender;
            this.Receiver = receiver;
            this.RequestedMembers = new List<T>();
        }

        public static Response<T> DoRequest(Request<T> request, bool auth = false)
        {
            if (request != null)
            {
                ObjectMessageQueue queue = new ObjectMessageQueue();
                if (auth)
                {
                    string guid = queue.sendObject(request, ObjectMessageQueue.AUTH_REQUEST);
                    object response = queue.receiveByID(guid, ObjectMessageQueue.AUTH_RESPONSE);
                    Response<T> responseCast = response as Response<T>;
                    return responseCast;
                }
                else
                {
                    string guid = queue.sendObject(request, ObjectMessageQueue.DB_REQUEST);
                    object response = queue.receiveByID(guid, ObjectMessageQueue.DB_RESPONSE);
                    Response<T> responseCast = response as Response<T>;
                    return responseCast;
                }
            }

            return null;
        }

        public static T GetItemByID(int id, string sender, string receiver)
        {
            string typestr = typeof(T).ToString().Split('.').Last();
            Request<T> ret = null;
            foreach (ModelType m in Enum.GetValues(typeof(ModelType)))
            {
                if (m.ToString() == typestr)
                {
                    ret = new Request<T>(m, MethodType.GetItemByID, sender, receiver);
                    ret.RequestedID = id;
                }
            }


            Response<T> response = Request<T>.DoRequest(ret);
            if (response == null)
            {
                return default(T);
            }
            else
            {
                return response.ToValue();
            }
        }

        public static List<T> GetItemsByPlanID(int id, string sender, string receiver)
        {
            string typestr = typeof(T).ToString().Split('.').Last();
            Request<T> ret = null;
            foreach (ModelType m in Enum.GetValues(typeof(ModelType)))
            {
                if (m.ToString() == typestr)
                {
                    ret = new Request<T>(m, MethodType.GetItemsByPlanID, sender, receiver);
                    ret.RequestedID = id;
                }
            }

            Response<T> response = Request<T>.DoRequest(ret);
            if (response == null)
            {
                return new List<T>();
            }
            else
            {
                return response.ToList();
            }
        }
        
        public static List<T> GetAll(string sender, string receiver)
        {
            Request<T> ret = null;
            string typestr = typeof(T).ToString().Split('.').Last();
            foreach (ModelType m in Enum.GetValues(typeof(ModelType)))
            {
                if (m.ToString() == typestr)
                {
                    ret = new Request<T>(m, MethodType.GetAll, sender, receiver);
                }
            }

            Response<T> response = Request<T>.DoRequest(ret);
            if (response == null)
            {
                return new List<T>();
            }
            else
            {
                return response.ToList();
            }
        }

        public static bool Delete(int item_id, string sender, string receiver)
        {
            Request<T> ret = null;
            string typestr = typeof(T).ToString().Split('.').Last();
            foreach (ModelType m in Enum.GetValues(typeof(ModelType)))
            {
                if (m.ToString() == typestr)
                {
                    ret = new Request<T>(m, MethodType.Delete, sender, receiver);
                    ret.RequestedID = item_id;
                }
            }

            Response<T> response = Request<T>.DoRequest(ret);
            if (response == null)
            {
                return false;
            }
            else
            {
                return response.ToBool();
            }
        }

        public static int Add(T new_item, string sender, string receiver)
        {
            Request<T> ret = null;
            string typestr = typeof(T).ToString().Split('.').Last();
            foreach (ModelType m in Enum.GetValues(typeof(ModelType)))
            {
                if (m.ToString() == typestr)
                {
                    ret = new Request<T>(m, MethodType.Add, sender, receiver);
                    ret.RequestedMembers.Add(new_item);
                }
            }

            Response<T> response = Request<T>.DoRequest(ret);
            if (response == null)
            {
                return -1;
            }
            else
            {
                return response.ToInt();
            }
        }

        public static bool Update(T old_item, T new_item, string sender, string receiver)
        {
            Request<T> ret = null;
            string typestr = typeof(T).ToString().Split('.').Last();
            foreach (ModelType m in Enum.GetValues(typeof(ModelType)))
            {
                if (m.ToString() == typestr)
                {
                    ret = new Request<T>(m, MethodType.Update, sender, receiver);
                    ret.RequestedMembers.Add(old_item);
                    ret.RequestedMembers.Add(new_item);
                }
            }

            Response<T> response = Request<T>.DoRequest(ret);
            if (response == null)
            {
                return false;
            }
            else
            {
                return response.ToBool();
            }
        }

        public static T GetUserByID(int id, string sender, string receiver)
        {
            string typestr = typeof(T).ToString().Split('.').Last();
            Request<T> ret = null;
            foreach (ModelType m in Enum.GetValues(typeof(ModelType)))
            {
                if (m.ToString() == typestr)
                {
                    ret = new Request<T>(m, MethodType.GetItemByID, sender, receiver);
                    ret.RequestedID = id;
                }
            }


            Response<T> response = Request<T>.DoRequest(ret, true);
            if (response == null)
            {
                return default(T);
            }
            else
            {
                return response.ToValue();
            }
        }

        public static T GetAuthItemById(int id, string sender, string receiver)
        {
            string typestr = typeof(T).ToString().Split('.').Last();
            Request<T> ret = null;
            foreach (ModelType m in Enum.GetValues(typeof(ModelType)))
            {
                if (m.ToString() == typestr)
                {
                    ret = new Request<T>(m, MethodType.GetItemByID, sender, receiver);
                    ret.RequestedID = id;
                }
            }


            Response<T> response = Request<T>.DoRequest(ret, true);
            if (response == null)
            {
                return default(T);
            }
            else
            {
                return response.ToValue();
            }
        }

        public static T GetUserRoleByName(string name, string sender, string receiver)
        {
            string typestr = typeof(T).ToString().Split('.').Last();
            Request<T> ret = null;
            foreach (ModelType m in Enum.GetValues(typeof(ModelType)))
            {
                if (m.ToString() == typestr)
                {
                    ret = new Request<T>(m, MethodType.GetByName, sender, receiver);
                    ret.RequestedName = name;
                }
            }


            Response<T> response = Request<T>.DoRequest(ret, true);
            if (response == null)
            {
                return default(T);
            }
            else
            {
                return response.ToValue();
            }
        }

        public static T GetAuthItemByName(string name, string sender, string receiver)
        {
            string typestr = typeof(T).ToString().Split('.').Last();
            Request<T> ret = null;
            foreach (ModelType m in Enum.GetValues(typeof(ModelType)))
            {
                if (m.ToString() == typestr)
                {
                    ret = new Request<T>(m, MethodType.GetByName, sender, receiver);
                    ret.RequestedName = name;
                }
            }


            Response<T> response = Request<T>.DoRequest(ret, true);
            if (response == null)
            {
                return default(T);
            }
            else
            {
                return response.ToValue();
            }
        }

        public static List<T> GetAllAuthItemByName(string name, string sender, string receiver)
        {
            string typestr = typeof(T).ToString().Split('.').Last();
            Request<T> ret = null;
            foreach (ModelType m in Enum.GetValues(typeof(ModelType)))
            {
                if (m.ToString() == typestr)
                {
                    ret = new Request<T>(m, MethodType.GetAllByName, sender, receiver);
                    ret.RequestedName = name;
                }
            }


            Response<T> response = Request<T>.DoRequest(ret, true);
            if (response == null)
            {
                return default(List<T>);
            }
            else
            {
                return response.ToList();
            }
        }

        public static bool AddUserRole(T new_item, string sender, string receiver)
        {
            Request<T> ret = null;
            string typestr = typeof(T).ToString().Split('.').Last();
            foreach (ModelType m in Enum.GetValues(typeof(ModelType)))
            {
                if (m.ToString() == typestr)
                {
                    ret = new Request<T>(m, MethodType.Add, sender, receiver);
                    ret.RequestedMembers.Add(new_item);
                }
            }

            Response<T> response = Request<T>.DoRequest(ret, true);
            if (response == null)
            {
                return false;
            }
            else
            {
                return response.ToBool();
            }
        }

        public static bool AddAuthItem(T new_item, string sender, string receiver)
        {
            Request<T> ret = null;
            string typestr = typeof(T).ToString().Split('.').Last();
            foreach (ModelType m in Enum.GetValues(typeof(ModelType)))
            {
                if (m.ToString() == typestr)
                {
                    ret = new Request<T>(m, MethodType.Add, sender, receiver);
                    ret.RequestedMembers.Add(new_item);
                }
            }

            Response<T> response = Request<T>.DoRequest(ret, true);
            if (response == null)
            {
                return false;
            }
            else
            {
                return response.ToBool();
            }
        }

        public static bool DeleteUserRole(int id, string sender, string receiver)
        {
            Request<T> ret = null;
            string typestr = typeof(T).ToString().Split('.').Last();
            foreach (ModelType m in Enum.GetValues(typeof(ModelType)))
            {
                if (m.ToString() == typestr)
                {
                    ret = new Request<T>(m, MethodType.Delete, sender, receiver);
                    ret.RequestedID = id;
                }
            }

            Response<T> response = Request<T>.DoRequest(ret, true);
            if (response == null)
            {
                return false;
            }
            else
            {
                return response.ToBool();
            }
        }

        public static bool DeleteAuthItem(int id, string sender, string receiver)
        {
            Request<T> ret = null;
            string typestr = typeof(T).ToString().Split('.').Last();
            foreach (ModelType m in Enum.GetValues(typeof(ModelType)))
            {
                if (m.ToString() == typestr)
                {
                    ret = new Request<T>(m, MethodType.Delete, sender, receiver);
                    ret.RequestedID = id;
                }
            }

            Response<T> response = Request<T>.DoRequest(ret, true);
            if (response == null)
            {
                return false;
            }
            else
            {
                return response.ToBool();
            }
        }

        public static bool UpdateUserRole(T old_item, T new_item, string sender, string receiver)
        {
            Request<T> ret = null;
            string typestr = typeof(T).ToString().Split('.').Last();
            foreach (ModelType m in Enum.GetValues(typeof(ModelType)))
            {
                if (m.ToString() == typestr)
                {
                    ret = new Request<T>(m, MethodType.Update, sender, receiver);
                    ret.RequestedMembers.Add(old_item);
                    ret.RequestedMembers.Add(new_item);
                }
            }

            Response<T> response = Request<T>.DoRequest(ret, true);
            if (response == null)
            {
                return false;
            }
            else
            {
                return response.ToBool();
            }
        }

        public static bool UpdateAuthItem(T old_item, T new_item, string sender, string receiver)
        {
            Request<T> ret = null;
            string typestr = typeof(T).ToString().Split('.').Last();
            foreach (ModelType m in Enum.GetValues(typeof(ModelType)))
            {
                if (m.ToString() == typestr)
                {
                    ret = new Request<T>(m, MethodType.Update, sender, receiver);
                    ret.RequestedMembers.Add(old_item);
                    ret.RequestedMembers.Add(new_item);
                }
            }

            Response<T> response = Request<T>.DoRequest(ret, true);
            if (response == null)
            {
                return false;
            }
            else
            {
                return response.ToBool();
            }
        }

        public static List<T> GetAllUserRoles(string sender, string receiver)
        {
            Request<T> ret = null;
            string typestr = typeof(T).ToString().Split('.').Last();
            foreach (ModelType m in Enum.GetValues(typeof(ModelType)))
            {
                if (m.ToString() == typestr)
                {
                    ret = new Request<T>(m, MethodType.GetAll, sender, receiver);
                }
            }

            Response<T> response = Request<T>.DoRequest(ret, true);
            if (response == null)
            {
                return new List<T>();
            }
            else
            {
                return response.ToList();
            }
        }

        public static List<T> GetAllAuthItems(string sender, string receiver)
        {
            Request<T> ret = null;
            string typestr = typeof(T).ToString().Split('.').Last();
            foreach (ModelType m in Enum.GetValues(typeof(ModelType)))
            {
                if (m.ToString() == typestr)
                {
                    ret = new Request<T>(m, MethodType.GetAll, sender, receiver);
                }
            }

            Response<T> response = Request<T>.DoRequest(ret, true);
            if (response == null)
            {
                return new List<T>();
            }
            else
            {
                return response.ToList();
            }
        }

        public static List<T> GetRolesForUser(int id, string sender, string receiver)
        {
            Request<T> ret = null;
            string typestr = typeof(T).ToString().Split('.').Last();
            foreach (ModelType m in Enum.GetValues(typeof(ModelType)))
            {
                if (m.ToString() == typestr)
                {
                    ret = new Request<T>(m, MethodType.GetRolesForUser, sender, receiver);
                    ret.RequestedID = id;
                }
            }

            Response<T> response = Request<T>.DoRequest(ret, true);
            if (response == null)
            {
                return new List<T>();
            }
            else
            {
                return response.ToList();
            }
        }
    }
}
