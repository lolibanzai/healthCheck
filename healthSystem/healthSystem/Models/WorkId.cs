using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using healthSystem.Models;

namespace healthSystem.Models
{
    public class WorkId
    {
        HealthCheckEntities1 db = new HealthCheckEntities1();
        public string GetWorkName(int workid)
        {
            var q = from o in db.WorkInfo
                    where o.work_id == workid
                    select o.work_name;
            string result = q.FirstOrDefault();
            return result;
        }
    }
}