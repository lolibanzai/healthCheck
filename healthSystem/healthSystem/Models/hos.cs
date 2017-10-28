using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace healthSystem.Models
{
    public class hos
    {
        HealthCheckEntities1 db = new HealthCheckEntities1();
        public string GetHosName(string hospitalID)
        {
            int hosID = Convert.ToInt32(hospitalID);
            var q = from o in db.Hospital
                    where o.hospital_hospitalId == hosID
                    select o.hospital_name;
            string result = q.FirstOrDefault();
            return result;
        }
    }
}