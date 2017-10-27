using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using healthSystem.Models;

namespace healthSystem.Models {
    public class FatoryId {
        HealthCheckEntities1 db = new HealthCheckEntities1();

        public string GetFactoryName(string factory_id) {
            string result = "";
            var q = from o in db.Factory
                    where o.factory_id == factory_id
                    select o.factory_name;
            return result;
        }

        public string GetFactoryArea(string factory_id) {
            string resultArea = "";
            var q = from o in db.Factory
                    where o.factory_id == factory_id
                    select o.factory_area;
            return resultArea;
        }

        public string GetFactoryAddress(string factory_id) {
            string result = "";
            var q = from o in db.Factory
                    where o.factory_id == factory_id
                    select o.factory_address;
            return result;
        }
    }
}