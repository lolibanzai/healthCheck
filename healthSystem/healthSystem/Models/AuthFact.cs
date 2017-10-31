using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using healthSystem.Models;

namespace healthSystem.Models {
    public class AuthFact {
        HealthCheckEntities1 db = new HealthCheckEntities1();
        public bool GetAuthFac(string employee_workNumber,string factoryId) {
            bool result=false;
            var q = from o in db.Authority
                    where o.authority_workNumber == employee_workNumber && o.authority_factoryId == factoryId
                    select o;
            var r = q.ToList();
            if (r.Count() == 0) {
                return result;
            }
            result = true;
            return result;
        }
        public bool GetIsDisable(string employee_workNumber, string factoryId)
        {
            bool result = false;
            string authority_isDisable = "";
            var q = (from o in db.Authority
                    where o.authority_workNumber == employee_workNumber && o.authority_factoryId == factoryId
                    select o).ToList();
            //var rf = q.FirstOrDefault();
       
            if (q.Count() != 0) {
                foreach(var item in q) {
                    authority_isDisable = item.authority_IsDisable;

                    if (item.authority_IsDisable == "N") {
                        result = true;
                    }
                    if (item.authority_IsDisable == "Y") {
                        result = false;
                    }
                    return result;
                }
                //if (authority_isDisable == "N") {
                //    result = true;
                //}
                
                //if (authority_isDisable == "Y") {
                //    result = false;
                //}
                    
                //return result;
            } //if(q.Count()==0) { 
            //result = true;
            //if (authority_isDisable == "") {
            //    result = true;
            //}
            //return result;
            //}
            result = false;
            return result;
        }
    }
}