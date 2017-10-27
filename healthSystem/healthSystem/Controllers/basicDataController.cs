using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using healthSystem.Models;
using healthSystem.ViewModel;

namespace healthSystem.Controllers
{
    public class basicDataController : Controller
    {
        HealthCheckEntities1 db = new HealthCheckEntities1();
        // GET: basicData
        public ActionResult purviewMaster() //人員權限主頁
        {
             string employee_workNumber = Session["employee_workNumber"].ToString();
            var q = from o in db.Employee
                    where o.employee_workNumber == employee_workNumber
                    select o;
            var emp = q.ToList();
            return View(emp);
        }
        [HttpPost]
        public ActionResult purviewMaster(Employee empData) //人員權限主頁
        {
            if (string.IsNullOrEmpty(empData.employee_name)) {
                empData.employee_name = "";
            }
            if (string.IsNullOrEmpty(empData.employee_username)) {
                empData.employee_username = "";
            }
            if (string.IsNullOrEmpty(empData.employee_email)) {
                empData.employee_email = "";
            }
            if (string.IsNullOrEmpty(empData.employee_factoryId)) {
                empData.employee_factoryId = "";
            }
            if (string.IsNullOrEmpty(empData.employee_isDisabled)) {
                empData.employee_isDisabled = "";
            }
            var q = from o in db.Employee
                    //where o.employee_name.Contains(empData.employee_name) && o.employee_isDisabled.Contains(empData.employee_isDisabled)
                    where (o.employee_name.Contains(empData.employee_name) && o.employee_username.Contains(empData.employee_username) &&
                    o.employee_email.Contains(empData.employee_email) && o.employee_factoryId.Contains(empData.employee_factoryId)) && o.employee_isDisabled.Contains(empData.employee_isDisabled)
                    select o;
            var result = q.ToList();
            
            return View(result);
        }
        public ActionResult purviewMain(FormCollection frm) //人員權限主檔,人員權限明細檔-廠別權限
        {
            string employee_workNumber = Request["employee_workNumber"];
            var q = from o in db.Authority
                    where o.authority_workNumber == employee_workNumber
                    select o;
            var q1 = from o in db.Employee
                     where o.employee_workNumber == employee_workNumber
                     select o;
            //var q2 = from o in db.Factory
            //         from o1 in db.Authority
            //         where o1.employee_workNumber==employee_workNumber
            //         select new { factory_id=o.factory_id,factory_area=o.factory_area, factory_name=o.factory_name,factory_contract=o.factory_contract};
            ViewModel_empAu data = new ViewModel_empAu();
            data.AuthView = q.ToList();
            data.EmpView = q1.ToList();
            //data.Factory = new List<Factory>();
            //    List<Factory>  q2.ToList();
            //List.
            return View(data);
            //I)
        }
        //------------------------------------------------------------------------------
        public ActionResult employeeMaster() //人事資料主頁
        {
            var query = from o in db.WorkInfo
                        select o;
            var query1 = from x in db.Factory
                         select x;
            var query2 = from o in db.Employee
                         where o.employee_workNumber == null
                         select o;
            basicDataEmployeeMaster data = new basicDataEmployeeMaster();
            data.workInfo = query.ToList();
            data.factory = query1.ToList();
            data.employee = query2.ToList();
            return View(data);
        }
        [HttpPost]
        public ActionResult employeeMaster(Factory factory,Employee employee,WorkInfo workInfo) //人事資料主頁
        {
            //若沒有輸入post回去會是null值,把它改成""去做模糊查詢
            if (string.IsNullOrEmpty(employee.employee_name)) {
                employee.employee_name = "";
            }
            if (string.IsNullOrEmpty(employee.employee_workNumber)) {
                employee.employee_workNumber = "";
            }
            if (string.IsNullOrEmpty(employee.employee_identityCard)) {
                employee.employee_identityCard = "";
            }
            if (string.IsNullOrEmpty(employee.employee_isDisabled)) {
                employee.employee_isDisabled = "";
            }
            if (string.IsNullOrEmpty(workInfo.work_name)) {
                workInfo.work_name = "";
            }
            //先將工廠名稱轉換成工廠ID,工種名稱轉換成工種ID
            var query = from f in db.Factory
                        where f.factory_name == factory.factory_name
                        select f.factory_id;
            var query1 = from w in db.WorkInfo
                         where w.work_name.Contains(workInfo.work_name)
                         select w.work_id;
            string factoryID = query.First();
            int workID = 0;
            if (query1.Count() == 1) {
                workID = query1.First();
            }
            var query2 = from e in db.Employee
                         where (e.employee_workId == workID &&
                               e.employee_name.Contains(employee.employee_name) &&
                               e.employee_workNumber.Contains(employee.employee_workNumber) &&
                               e.employee_factoryId == factoryID &&
                               e.employee_identityCard.Contains(employee.employee_identityCard) &&
                               e.employee_isDisabled.Contains(employee.employee_isDisabled))
                               ||
                               (workID == 0 &&
                               e.employee_name.Contains(employee.employee_name) &&
                               e.employee_workNumber.Contains(employee.employee_workNumber) &&
                               e.employee_factoryId == factoryID &&
                               e.employee_identityCard.Contains(employee.employee_identityCard) &&
                               e.employee_isDisabled.Contains(employee.employee_isDisabled))
                         select e;
            var query3 = from o in db.WorkInfo
                         select o;
            var query4 = from x in db.Factory
                         select x;
            basicDataEmployeeMaster data = new basicDataEmployeeMaster();
            data.employee = query2.ToList();
            data.workInfo = query3.ToList();
            data.factory = query4.ToList();
            return View(data);
        }
        public ActionResult employeeMain() //人事資料主檔,明細檔-工種,明細檔-健檢記錄
        {
            return View();
        }
        //------------------------------------------------------------------------------
        public ActionResult hospitalMaster() //醫院資料主頁
        {
            List<Hospital> hospitalData = new List<Hospital>();
            return View(hospitalData);
        }
        [HttpPost]
        public ActionResult hospitalMaster(Hospital hospitalData) //醫院資料主頁post
        {
            if (string.IsNullOrEmpty(hospitalData.hospital_name))
            {
                hospitalData.hospital_name = "";
            }
            if (string.IsNullOrEmpty(hospitalData.hospital_state))
            {
                hospitalData.hospital_state = "";
            }
            var query = from o in db.Hospital
                        where o.hospital_name.Contains(hospitalData.hospital_name) && o.hospital_state.Contains(hospitalData.hospital_state)
                        select o;
            //o.hospital_name.Contains(hospitalData.hospital_name)
            //上述為模糊查詢,利用Contains達成模糊查詢的目的
            //意思是:hospitalData.hospital_name的字串是否存在於o.hospital_name裡面
            //是的話就查出來
            List<Hospital> data = query.ToList();
            return View(data);
        }
        public ActionResult hospitalMain() //醫院資料主檔,明細檔-附件管理,明細檔-健檢方案(編輯完成後使用的ActionResult)
        {
            int hospital_hospitalId = Convert.ToInt32(Session["hospitalID"]);
            var query = from o in db.Hospital
                        where o.hospital_hospitalId == hospital_hospitalId
                        select o;
            var query1 = from o in db.HospitalProgramFile
                         where o.file_hospitalId == hospital_hospitalId
                         select o;
            var query2 = from o in db.Program
                         where o.program_hospitalId == hospital_hospitalId
                         select o;
            basicDataHospitalMain data = new basicDataHospitalMain();
            data.hospital = query.ToList();
            data.hospitalProgramFile = query1.ToList();
            data.program = query2.ToList();
            return View(data);
        }
        [HttpPost]
        public ActionResult hospitalMain(int hospital_hospitalId) //醫院資料主檔,明細檔-附件管理,明細檔-健檢方案(查詢完成後使用的ActionResult)
        {
            var query  = from o in db.Hospital
                         where o.hospital_hospitalId == hospital_hospitalId
                         select o;
            var query1 = from o in db.HospitalProgramFile
                         where o.file_hospitalId == hospital_hospitalId
                         select o;
            var query2 = from o in db.Program
                         where o.program_hospitalId == hospital_hospitalId
                         select o;
            basicDataHospitalMain data = new basicDataHospitalMain();
            data.hospital = query.ToList();
            data.hospitalProgramFile = query1.ToList();
            data.program = query2.ToList();
            Session["hospitalID"] = hospital_hospitalId;
            return View(data);
        }
        public ActionResult newhospital()
        {
            List<Hospital> data = new List<Hospital>();
            return View(data);
        }
        [HttpPost]
        public ActionResult newhospital(Hospital data)
        {
                Hospital newhospital = new Hospital()
                {
                    hospital_name = data.hospital_name,
                    hospital_uniform = data.hospital_uniform,
                    hospital_address = data.hospital_address,
                    hospital_state = data.hospital_state,
                    hospital_contact = data.hospital_contact,
                    hospital_phone = data.hospital_phone,
                    hospital_website = data.hospital_website,
                    hospital_email = data.hospital_email
                    //hospital_updateuser = data.hospital_updateuser,
                    //hospital_updatetime = data.hospital_updatetime
                };
                db.Hospital.Add(newhospital);
                db.SaveChanges();
            List<Hospital> hospitalList = new List<Hospital>();
            TempData["hospitalMessage"] = "新增成功";
            return View(hospitalList);
        }
        public ActionResult edithospital()
        {
            int hospitalID = Convert.ToInt32(Session["hospitalID"]);
            var query = from o in db.Hospital
                        where o.hospital_hospitalId == hospitalID
                        select o;
            List<Hospital> data = query.ToList();
            return View(data);
        }
        [HttpPost]
        public ActionResult edithospital(Hospital editData)
        {
            Hospital data = db.Hospital.Find(Session["hospitalID"]);
            data.hospital_name = editData.hospital_name;
            data.hospital_uniform = editData.hospital_uniform;
            data.hospital_address = editData.hospital_address;
            data.hospital_state = editData.hospital_state;
            data.hospital_contact = editData.hospital_contact;
            data.hospital_phone = editData.hospital_phone;
            data.hospital_website = editData.hospital_website;
            data.hospital_email = editData.hospital_email;
            data.hospital_updateuser = editData.hospital_updateuser;
            data.hospital_updatetime = editData.hospital_updatetime;
            db.SaveChanges();
            return RedirectToAction("hospitalMain", "basicData");
        }
    }
}