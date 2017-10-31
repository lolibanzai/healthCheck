using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using healthSystem.Models;

namespace healthSystem.Models {
    public class Emp {
        HealthCheckEntities1 db = new HealthCheckEntities1();
        public string Name(string employee_workNumber) {
            string employee_id = employee_workNumber;
            var q = from o in db.Employee
                    where o.employee_workNumber == employee_id
                    select o.employee_name;
            string result = q.FirstOrDefault();
            return result;
        }
        public string GetFactoryName(string authority_factoryId) {
            string authority_factoryid = authority_factoryId;
            var q = from o in db.Factory
                    where o.factory_id == authority_factoryId
                    select o.factory_name;
            string result = q.FirstOrDefault();
            return result;
        }
        public string GetFactoryArea(string authority_factoryId) {
            string authority_factoryid = authority_factoryId;
            var q = from o in db.Factory
                    where o.factory_id == authority_factoryId
                    select o.factory_area;
            string result = q.FirstOrDefault();
            return result;
        }
        public string GetFactoryId(string employee_workNumber) {
            var q = from o in db.Employee
                    where o.employee_workNumber == employee_workNumber
                    select o.employee_factoryId;
            string result = q.FirstOrDefault();
            return result;
        }
        public string GetEmail(string employee_workNumber) {
            var q = from o in db.Employee
                    where o.employee_workNumber == employee_workNumber
                    select o.employee_email;
            string result = q.FirstOrDefault();
            return result;
        }
        public string GetUserName(string employee_workNumber) {
            var q = from o in db.Employee
                    where o.employee_workNumber == employee_workNumber
                    select o.employee_username;
            string result = q.FirstOrDefault();
            return result;
        }
        public string GetIsDisabled(string employee_workNumber) {
            var q = from o in db.Employee
                    where o.employee_workNumber == employee_workNumber
                    select o.employee_isDisabled;
            string result = q.FirstOrDefault();
            return result;
        }
        public string GetRole(string employee_workNumber)
        {
            var q = from o in db.Employee
                    where o.employee_workNumber == employee_workNumber
                    select o.employee_role;
            string result = q.FirstOrDefault();
            return result;
        }
    }
}

