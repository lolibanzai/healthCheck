using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using healthSystem.Models;

namespace healthSystem.Models {
    public class AuthFact {
        HealthCheckEntities1 db = new HealthCheckEntities1();
        //查詢員工編號與廠別代碼，存在丟True，反之丟False
        public bool GetAuthFac(string employee_workNumber,string factoryId) {
            bool result=false;
            var q = from o in db.Authority
                    where o.authority_workNumber == employee_workNumber && o.authority_factoryId == factoryId
                    select o;
            var r = q.ToList();
            //查無資料，List元素數量為0
            if (r.Count() == 0) {
                return result;
            }
            result = true;
            return result;
        }
        //查詢員工編號與廠別權限是否停用
        public bool GetIsDisable(string employee_workNumber, string factoryId)
        {
            bool result = false;
            string authority_isDisable = "";
            var q = (from o in db.Authority
                    where o.authority_workNumber == employee_workNumber && o.authority_factoryId == factoryId
                    select o).ToList();
            //有查到資料
            if (q.Count() != 0) {
                foreach(var item in q) {
                    authority_isDisable = item.authority_IsDisable;
                    //判斷停用註記是否為Y，是丟True
                    if (item.authority_IsDisable == "N") {
                        result = true;
                    }
                    //判斷停用註記是否為N，是丟false
                    if (item.authority_IsDisable == "Y") {
                        result = false;
                    }
                    return result;
                }
            }
            //其餘情況都丟False
            result = false;
            return result;
        }
    }
}