using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace healthSystem.Models
{
    public class reportManageUse
    {
        HealthCheckEntities1 db = new HealthCheckEntities1();
        //工號查詢姓名
        public string GETemployeeName(string workNumber) //startHand_workNumber ---> employee_workNumber,employee_name
        {
            var query1 = from o in db.Employee
                         where workNumber == o.employee_workNumber
                         select o.employee_name;
            string name = query1.FirstOrDefault();
            return name;
        }

        //工號查詢公司
        public string GETemployeeCampany(string workNumber) {

            var query2 = from o in db.Employee
                         where workNumber == o.employee_workNumber
                         select o.employee_corporation;

            string corporation = query2.FirstOrDefault();
            return corporation;


        }
        //工號查詢廠別

        public string GETemployeeFactory(string workNumber)
        {

            var query2 = from o in db.Employee
                         where workNumber == o.employee_workNumber
                         select o.employee_corporation;

            string corporation = query2.FirstOrDefault();
            return corporation;


        }
        //用醫院ID查詢醫院名稱
        public string GETHosName(int? HosId)
        {
           
            var query2 = from o in db.Hospital
                         where HosId == o.hospital_hospitalId
                         select o.hospital_name;

            string hospitalNname = query2.FirstOrDefault();
            return hospitalNname;


        }
        //工號查詢預約單號

        public string GETBookNumber(string workNumber) //startHand_workNumber ---> employee_workNumber,employee_name
        {
            var query1 = from o in db.Book
                         where workNumber == o.book_workNumber
                         select o.book_id;
            string BookId = query1.FirstOrDefault();
            return BookId;
        }

        //工號查詢部門
        public string GETemployee_department(string workNumber)
        {

            var query2 = from o in db.Employee
                         where workNumber == o.employee_workNumber
                         select o.employee_department;

            string department = query2.FirstOrDefault();
            return department;


        }
        //工號查詢集團ID
        public string Getemployee_groupId(string workNumber)
        {

            var query2 = from o in db.Employee
                         where workNumber == o.employee_workNumber
                         select o.employee_groupId;

            string groupId = query2.FirstOrDefault();
            return groupId;


        }
        //工號查詢身分證
        public string Getemployee_identityCard(string workNumber)
        {

            var query2 = from o in db.Employee
                         where workNumber == o.employee_workNumber
                         select o.employee_identityCard;

            string identityCard = query2.FirstOrDefault();
            return identityCard;


        }
        //用方案ID查詢方案名稱
        public string GETProgramName(int ProgramId)
        {

            var query2 = from o in db.Program
                         where ProgramId == o.program_programId
                         select o.program_programName;

            string programName = query2.FirstOrDefault();
            return programName;


        }
        //用方案ID查詢方案名稱
        public string GETWorkName(int ProgramId)
        {

            var query2 = from o in db.Program
                         where ProgramId == o.program_programId
                         select o.program_workid;

            int workid = query2.FirstOrDefault();

            var query3 = from o in db.WorkInfo
                         where workid == o.work_id
                         select o.work_name;
            string workname = query3.FirstOrDefault();
            return workname;

     


        }
        public string GEThosHw(int? reportId)
        {

            var query2 = from o in db.ReportCheckItem
                         where reportId == o.ReportCheckItem_reportId
                         select o.ReportCheckItem_hw;

            string ReportCheckItem_hw = query2.FirstOrDefault();

            return ReportCheckItem_hw;



        }
        public string GEThospass(int? reportId)
        {

            var query2 = from o in db.ReportCheckItem
                         where reportId == o.ReportCheckItem_reportId
                         select o.ReportCheckItem_pastHistory;

            string ReportCheckItem_pastHistory = query2.FirstOrDefault();

            return ReportCheckItem_pastHistory;



        }
        public string GEThosSym(int? reportId)
        {

            var query2 = from o in db.ReportCheckItem
                         where reportId == o.ReportCheckItem_reportId
                         select o.ReportCheckItem_symptoms;

            string ReportCheckItem_symptoms = query2.FirstOrDefault();

            return ReportCheckItem_symptoms;



        }
        public string GEThosDate(int? reportId)
        {

            var query2 = from o in db.ReportCheckItem
                         where reportId == o.ReportCheckItem_reportId
                         select o.ReportCheckItem_checkDate;

            string ReportCheckItem_checkDate = query2.FirstOrDefault();

            return ReportCheckItem_checkDate;



        }

        public string GETreportWN(int? reportId)
        {

            var query2 = from o in db.ReportCheckItem
                         where reportId == o.ReportCheckItem_reportId
                         select o.ReportCheckItem_employee_workNumber;

            string employee_workNumber = query2.FirstOrDefault();

            return employee_workNumber;



        }
        public string GETreportNM(int? reportId)
        {

            var query2 = from o in db.ReportCheckItem
                         where reportId == o.ReportCheckItem_reportId
                         select o.ReportCheckItem_employee_workNumber;

            string workNumber = query2.FirstOrDefault();

            var query3 = from o in db.Employee
                         where  workNumber == o.employee_workNumber
                         select o.employee_name;
            string name = query3.FirstOrDefault();
            return name;



        }

    }
}