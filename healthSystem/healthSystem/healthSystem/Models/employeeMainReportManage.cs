using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using healthSystem.Models;

namespace healthSystem.Models
{
    public class employeeMainReportManage
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
        //查詢年度
        public int GETyear(int? startId) //ReportManage_startId ---> start_year
        {
            var query = from o in db.StartCheck
                        where startId == o.Start_id
                        select o.start_year;
            int year = query.FirstOrDefault();
            return year;
        }
        //查詢公司
        public string GETcorporation(string workNumber) //ReportManage_workNumber ---> employee_workNumber,employee_corporation
        {
            var query = from o in db.Employee
                        where workNumber == o.employee_workNumber
                        select o.employee_corporation;
            string corporation = query.FirstOrDefault();
            return corporation;
        }
        //查詢廠別
        public string GETfactoryName(string workNumber) //ReportManage_workNumber ---> employee_workNumber,employee_factoryId ---> factory_id,factory_name
        {
            var query = from o in db.Employee
                        where workNumber == o.employee_workNumber
                        select o.employee_factoryId;
            string factoryId = query.FirstOrDefault();
            var query1 = from o in db.Factory
                         where factoryId == o.factory_id
                         select o.factory_name;
            string name = query1.FirstOrDefault();
            return name;
        }
        //查詢部門
        public string GETdepartment(string workNumber) //ReportManage_workNumber ---> employee_workNumber,employee_department
        {
            var query = from o in db.Employee
                        where workNumber == o.employee_workNumber
                        select o.employee_department;
            string department = query.FirstOrDefault();
            return department;
        }
        //查詢醫院名稱
        public string GEThospitalName(int? hospitalId) //ReportManage_hospitalId ---> hospital_hospitalId,hospital_name
        {
            var query = from o in db.Hospital
                        where hospitalId == o.hospital_hospitalId
                        select o.hospital_name;
            string name = query.FirstOrDefault();
            return name;
        }
        //查詢方案名稱
        public string GETprogramName(int programId) //ReportManage_programId ---> program_programId,program_programName
        {
            var query = from o in db.Program
                        where programId == o.program_programId
                        select o.program_programName;
            string programName = query.FirstOrDefault();
            return programName;
        }
    }
}