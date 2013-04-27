using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MessageParser
{
    [DataContract]
    public class Response<T>
    {
        [DataMember]
        private MethodType method;
        [DataMember]
        private T resultVal;
        [DataMember]
        private List<T> resultList;
        [DataMember]
        private Boolean resultState;
        [DataMember]
        private int resultInt;


        public Response()
        {
            this.method = MethodType.GetAll;
            this.resultVal = default(T);
            this.resultList = new List<T>();
            this.resultState = false;
        }
        public Response(Request<T> originator, T result)
        {
            this.method = originator.Method;
            this.resultVal = result;
            this.resultList = new List<T>();
            this.resultList.Add(result);
            this.resultState = false;
        }
        public Response(Request<T> originator, List<T> result)
        {
            this.method = originator.Method;
            this.resultList = result;
            this.resultVal = result.FirstOrDefault();
            this.resultState = false;
        }
        public Response(Request<T> originator, bool result)
        {
            this.method = originator.Method;
            this.resultState = result;
            this.resultVal = default(T);
            this.resultList = null;
        }
        public Response(Request<T> originator, int result)
        {
            this.method = originator.Method;
            this.resultState = false;
            this.resultVal = default(T);
            this.resultList = null;
            this.resultInt = result;
        }

        public List<T> ToList()
        {
            return this.resultList;
        }
        public T ToValue()
        {
            return this.resultVal;
        }
        public Boolean ToBool()
        {
            return this.resultState;
        }

        public int ToInt()
        {
            return this.resultInt;
        }
    }
}
