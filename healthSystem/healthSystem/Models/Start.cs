using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using healthSystem.Models;

namespace healthSystem.Models
{
    public class Start
    {
        HealthCheckEntities1 db = new HealthCheckEntities1();
        public bool Start_exist(int StartId, string factoryId)
        {
            bool result = true;
            var q = (from o in db.StartPlace
                     where o.startplace_startId == StartId && o.startPlace_factoryId == factoryId
                     select o).ToList();
            if (q.Count != 0)
            {
                result = false;
                return result;
            }
            return result;
        }
        public bool StartHand_Emp(int startHand_checkId, string employee_workNumber)
        {
            bool result = true;
            var q = (from o in db.StartHand
                     where o.startHand_checkId == startHand_checkId && o.startHand_workNumber == employee_workNumber
                     select o).ToList();
            if (q.Count != 0)
            {
                result = false;
                return result;
            }
            return result;
        }
        public string GetFactoryName(string factory_id)
        {
            string result = "";
            var q = (from o in db.Factory
                     where o.factory_id == factory_id
                     select o.factory_name).FirstOrDefault();
            result = q;
            return result;
        }
        public bool StartHand_FaExist(string startPlace_factoryId, int startId)
        {
            bool result = true;
            var q = (from o in db.StartPlace
                     where o.startplace_startId == startId && o.startPlace_factoryId == startPlace_factoryId
                     select o).ToList();
            if (q.Count != 0)
            {
                result = false;
                return result;
            }
            return result;
        }
    }
}