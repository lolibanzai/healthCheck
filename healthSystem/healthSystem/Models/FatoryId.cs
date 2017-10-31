using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using healthSystem.Models;

namespace healthSystem.Models {
    //宣告一個查詢廠別類別
    public class FatoryId {
        HealthCheckEntities1 db = new HealthCheckEntities1();
        //查詢廠別名稱方法
        public string GetFactoryName(string factory_id) {
            string result = "";
            var q = from o in db.Factory
                    where o.factory_id == factory_id
                    select o.factory_name;
            return result;
        }
        //查詢廠別地區方法
        public string GetFactoryArea(string factory_id) {
            string resultArea = "";
            var q = from o in db.Factory
                    where o.factory_id == factory_id
                    select o.factory_area;
            return resultArea;
        }
        //查詢廠別地址方法
        public string GetFactoryAddress(string factory_id) {
            string result = "";
            var q = from o in db.Factory
                    where o.factory_id == factory_id
                    select o.factory_address;
            return result;
        }
    }
}