using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using healthSystem.Models;

namespace healthSystem.Models
{
    public class FatoryId
    {
        HealthCheckEntities1 db = new HealthCheckEntities1();
        //廠別代號查詢廠別名稱
        public string GetFactoryName(string factory_id)
        {
            string result = "";
            var q = (from o in db.Factory
                     where o.factory_id == factory_id
                     select o.factory_name).FirstOrDefault();
            result = q;
            return result;
        }
        //廠別代號查出廠別地區
        public string GetFactoryArea(string factory_id)
        {
            string resultArea = "";
            var q = (from o in db.Factory
                     where o.factory_id == factory_id
                     select o.factory_area).FirstOrDefault();
            resultArea = q;
            return resultArea;
        }
        //廠別代號查出廠別地址
        public string GetFactoryAddress(string factory_id)
        {
            string result = "";
            var q = (from o in db.Factory
                     where o.factory_id == factory_id
                     select o.factory_address).FirstOrDefault();
            result = q;
            return result;
        }
    }
}