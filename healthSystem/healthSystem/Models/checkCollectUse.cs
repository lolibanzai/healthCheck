using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using healthSystem.Models;

namespace healthSystem.Models
{
    public class checkCollectUse
    {
        HealthCheckEntities1 db = new HealthCheckEntities1();
        //查詢工種
        public string GETworkName(int? workId) //employee_workId ---> work_id,work_name
        {
            var query = from o in db.WorkInfo
                        where workId == o.work_id
                        select o.work_name;
            string name = query.FirstOrDefault();
            return name;
        }
        //查詢廠別
        public string GETfactoryName(string factoryId) //employee_factoryId ---> factory_id,factory_name
        {
            var query1 = from o in db.Factory
                         where factoryId == o.factory_id
                         select o.factory_name;
            string name = query1.FirstOrDefault();
            return name;
        }



        //查詢姓名
        public string GETemployeeName(string workNumber) //startHand_workNumber ---> employee_workNumber,employee_name
        {
            var query1 = from o in db.Employee
                         where workNumber == o.employee_workNumber
                         select o.employee_name;
            string name = query1.FirstOrDefault();
            return name;
        }
        //查詢性別
        public string GETemployeeGender(string workNumber) //startHand_workNumber ---> employee_workNumber,employee_gender
        {
            var query1 = from o in db.Employee
                         where workNumber == o.employee_workNumber
                         select o.employee_gender;
            string gender = query1.FirstOrDefault();
            return gender;
        }
        //查詢公司
        public string GETemployeeCorporation(string workNumber) //startHand_workNumber ---> employee_workNumber,employee_corporation
        {
            var query1 = from o in db.Employee
                         where workNumber == o.employee_workNumber
                         select o.employee_corporation;
            string corporation = query1.FirstOrDefault();
            return corporation;
        }
        //查詢廠別
        public string GETfactoryName1(string workNumber) //startHand_workNumber ---> employee_workNumber,employee_factoryId ---> factory_id,factory_name
        {
            var query1 = from o in db.Employee
                         where workNumber == o.employee_workNumber
                         select o.employee_factoryId;
            string factoryId = query1.FirstOrDefault();
            var query2 = from o in db.Factory
                         where factoryId == o.factory_id
                         select o.factory_name;
            string factoryName = query2.FirstOrDefault();
            return factoryName;
        }
        //查詢部門
        public string GETemployeeDepartment(string workNumber) //startHand_workNumber ---> employee_workNumber,employee_department
        {
            var query1 = from o in db.Employee
                         where workNumber == o.employee_workNumber
                         select o.employee_department;
            string employeeDepartment = query1.FirstOrDefault();
            return employeeDepartment;
        }
        //查詢入廠日期
        public DateTime GETemployeeComingDate(string workNumber) //startHand_workNumber ---> employee_workNumber,employee_comingDate
        {
            var query1 = from o in db.Employee
                         where workNumber == o.employee_workNumber
                         select o.employee_comingDate;
            DateTime employeeComingDate = query1.FirstOrDefault();
            return employeeComingDate;
        }
        //查詢工種
        public string GETworkName(string workNumber) //startHand_workNumber ---> (EmployeeWork)employee_workNumber,employee_Workid ---> (WorkInfo)work_id,work_name
        {
            var query1 = from o in db.EmployeeWork
                         where workNumber == o.employee_workNumber
                         select o.employee_Workid;
            int? employeeWorkId = query1.FirstOrDefault();
            var query2 = from o in db.WorkInfo
                         where employeeWorkId == o.work_id
                         select o.work_name;
            string workName = query2.FirstOrDefault();
            return workName;
        }
        //查詢工作地點
        public string GETmailingAddres(string workNumber) //startHand_workNumber ---> employee_workNumber,employee_mailingAddress
        {
            var query1 = from o in db.Employee
                         where workNumber == o.employee_workNumber
                         select o.employee_mailingAddress;
            string employeeComingDate = query1.FirstOrDefault();
            return employeeComingDate;
        }
        //判斷是否系統收集過
        public bool systemCollect(int StartID)
        {
            var query = (from o in db.StartHand
                         where o.startHand_checkId == StartID && o.startHand_type == "系統收集"
                         select o.startHand_id).ToList();
            if (query.Count() > 0)
            {
                return false;
            }
            return true;
        }
    }
}