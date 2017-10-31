using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using healthSystem.Models;

namespace healthSystem.Models
{
    public class startCheckMain
    {
        HealthCheckEntities1 db = new HealthCheckEntities1();
        //查詢廠別地區
        public string GETfactoryArea(string factoryId) //startPlace_factoryId ---> factory_id,factory_area
        {
            var query = from o in db.Factory
                        where o.factory_id == factoryId
                        select o.factory_area;
            string area = query.FirstOrDefault();
            return area;
        }
        //查詢廠別名稱
        public string GETfactoryName(string factoryId) //startPlace_factoryId ---> factory_id,factory_name
        {
            var query = from o in db.Factory
                        where o.factory_id == factoryId
                        select o.factory_name;
            string name = query.FirstOrDefault();
            return name;
        }
        //查詢廠別窗口人員
        public string GETfactoryContract(string factoryId) //startPlace_factoryId ---> factory_id,factory_contract
        {
            var query = from o in db.Factory
                        where o.factory_id == factoryId
                        select o.factory_contract;
            string contract = query.FirstOrDefault();
            return contract;
        }
    }
}