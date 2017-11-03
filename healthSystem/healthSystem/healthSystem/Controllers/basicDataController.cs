using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using healthSystem.Models;
using healthSystem.ViewModel;
using System.IO;

namespace healthSystem.Controllers
{
    public class basicDataController : Controller
    {
        HealthCheckEntities1 db = new HealthCheckEntities1();
        // GET: basicData
        public ActionResult purviewMaster() //人員權限主頁
        {
            string employee_workNumber = Session["employee_workNumber"].ToString();
            //var q = from o in db.Employee
            //        where o.employee_workNumber == employee_workNumber
            //        select o;
            //var emp = q.ToList();
            List<Employee> emp = new List<Employee>();
            return View(emp);
        }
        [HttpPost]
        public ActionResult purviewMaster(Employee empData) //人員權限主頁
        {
            //確認employee_姓名是否為空，是給空字串
            if (string.IsNullOrEmpty(empData.employee_name))
            {
                empData.employee_name = "";
            }
            //確認employee帳號是否為空，是給空字串
            if (string.IsNullOrEmpty(empData.employee_username))
            {
                empData.employee_username = "";
            }
            //確認employee_email是否為空，是給空字串
            if (string.IsNullOrEmpty(empData.employee_email))
            {
                empData.employee_email = "";
            }
            //確認employee歸屬廠別是否為空，是給空字串
            if (string.IsNullOrEmpty(empData.employee_factoryId))
            {
                empData.employee_factoryId = "";
            }
            //確認employee停用是否為空，是給空字串
            if (string.IsNullOrEmpty(empData.employee_isDisabled))
            {
                empData.employee_isDisabled = "";
            }
            //Contains模糊搜尋，查詢任意值就給空字串
            var q = from o in db.Employee
                    where (o.employee_name.Contains(empData.employee_name) &&
                           o.employee_username.Contains(empData.employee_username) &&
                           o.employee_email.Contains(empData.employee_email) &&
                           o.employee_factoryId.Contains(empData.employee_factoryId)) &&
                           o.employee_isDisabled.Contains(empData.employee_isDisabled)
                    select o;
            var result = q.ToList();

            return View(result);
        }
        public ActionResult purviewMain(FormCollection frm) //人員權限主檔,人員權限明細檔-廠別權限
        {
            //接收回應的employee_workNumber值
            string employee_workNumber = Request["employee_workNumber"];
            //用員工編號ID查權限
            var q = from o in db.Authority
                    where o.authority_workNumber == employee_workNumber
                    select o;
            //用員工編號查員工資料
            var q1 = from o in db.Employee
                     where o.employee_workNumber == employee_workNumber
                     select o;
            //創一個新的ViewModel_empAu
            ViewModel_empAu data = new ViewModel_empAu();
            //把查出來的員工資料跟權限給ViewModel_empAu
            data.AuthView = q.ToList();
            data.EmpView = q1.ToList();
            //取出員工編號給Session["employee_acc"]
            foreach (var item in data.EmpView)
            {
                Session["employee_acc"] = item.employee_workNumber;
            }
            return View(data);
        }
        //[HttpPost]
        //public ActionResult purviewMain(FormCollection frm) //人員權限主檔,人員權限明細檔-廠別權限
        //{
        //    string employee_workNumber = Request["employee_workNumber"];
        //    var q = from o in db.Authority
        //            where o.authority_workNumber == employee_workNumber
        //            select o;
        //    var q1 = from o in db.Employee
        //             where o.employee_workNumber == employee_workNumber
        //             select o;
        //    //var q2 = from o in db.Factory
        //    //         from o1 in db.Authority
        //    //         where o1.employee_workNumber==employee_workNumber
        //    //         select new { factory_id=o.factory_id,factory_area=o.factory_area, factory_name=o.factory_name,factory_contract=o.factory_contract};
        //    ViewModel_empAu data = new ViewModel_empAu();
        //    data.AuthView = q.ToList();
        //    data.EmpView = q1.ToList();
        //    //data.Factory = new List<Factory>();
        //    //    List<Factory>  q2.ToList();
        //    //List.
        //    return View(data);
        //    //I)
        //}
        //編輯員工權限
        public ActionResult EditfactoryAuthority(FormCollection frm)
        {
            //宣告employeeworkNumber接傳回來的employeeworkNumber值
            string employeeworkNumber = Request["employeeworkNumber"];
            // 查詢員工編號與權限沒停用的資料
            var q1 = from o in db.Authority
                     where o.authority_workNumber == employeeworkNumber && o.authority_IsDisable == "N"
                     select o;
            //查詢廠別資料
            var q2 = from o in db.Factory
                     select o;
            //宣告一個新的ViewModel_AuFa
            ViewModel_AuFa data = new ViewModel_AuFa();
            //把查詢到的權限資料跟廠別資料放入
            data.Factory = q2.ToList();
            data.AuthView = q1.ToList();
            return View(data);
        }
        [HttpPost]
        public ActionResult EditFactoryAuthority(ViewModel_AuFa auth)
        {
            //宣告變數
            string workNumber = Session["employee_acc"].ToString();
            string employee_workNumber = Request["employee_workNumber"];
            string auth_workNumber = Request["authority_workNumber"];
            string factoryid = Request["factoryId"];
            //action為0表示新增，為1表示停用
            int action = Convert.ToInt32(Request["AddorRemove"].ToString());
            //引用Emp類別
            Emp emp = new Emp();
            try
            {
                //action==0時，表示新增權限
                if (action == 0)
                {
                    //用員工編號跟廠別ID查詢
                    var query = (from o in db.Authority
                                 where o.authority_workNumber == workNumber && o.authority_factoryId == factoryid
                                 select o).ToList();
                    //當查無資料時query陣列沒有元素
                    if (query.Count() == 0)
                    {
                        //創新的Authority資料行
                        Authority authfac = new Authority()
                        {
                            authority_workNumber = auth_workNumber,
                            authority_factoryId = factoryid,
                            authority_IsDisable = "N",
                            authority_role = emp.GetRole(employee_workNumber),
                            authority_updateTime = DateTime.Now,
                            authority_updateuser = emp.Name(employee_workNumber)
                        };
                        //把資料行加入Authority資料表，並儲存
                        db.Authority.Add(authfac);
                        db.SaveChanges();
                    }
                    //當查詢員工編號跟廠別編號有值時
                    else if (query.Count() != 0)
                    {
                        //把資料行內的每個元素做變更
                        foreach (var item in query)
                        {
                            item.authority_Id = item.authority_Id;
                            item.authority_factoryId = item.authority_factoryId;
                            item.authority_workNumber = item.authority_workNumber;
                            item.authority_IsDisable = "N";
                            item.authority_updateuser = emp.Name(employee_workNumber);
                            item.authority_updateTime = DateTime.Now;
                        }
                        //寫回資料表
                        db.SaveChanges();
                    }
                }
                //當action為1時，表示停用該權限
                if (action == 1)
                {
                    //查詢員工編號與廠別代碼的資料
                    var query = (from o in db.Authority
                                 where o.authority_workNumber == workNumber && o.authority_factoryId == factoryid
                                 select o);
                    var qtoList = query.ToList();
                    //取出每個資料數值並更新
                    foreach (var item in query)
                    {
                        item.authority_Id = item.authority_Id;
                        item.authority_factoryId = item.authority_factoryId;
                        item.authority_workNumber = item.authority_workNumber;
                        item.authority_IsDisable = "Y";
                        item.authority_updateuser = emp.Name(employee_workNumber);
                        item.authority_updateTime = DateTime.Now;
                    }
                    //寫回資料表
                    db.SaveChanges();
                }
            }
            catch (Exception ex) //寫回錯誤輸出SQL錯誤碼
            {
                return Content(ex.ToString());
            }
            //查詢員工編號與權限沒停用的資料
            var q1 = from o in db.Authority
                     where o.authority_workNumber == workNumber && o.authority_IsDisable == "N"
                     select o;
            //查詢廠別資料
            var q2 = from o in db.Factory
                     select o;
            //引用ViewModel_AuFa類別
            ViewModel_AuFa data = new ViewModel_AuFa();
            //把查詢到的廠別資料給ViewModel_AuFa 下 Factoroy
            data.Factory = q2.ToList();
            //把查詢到的權限資料給ViewModel_AuFa 下 AuthView
            data.AuthView = q1.ToList();
            return View(data);
        }
        //人事---------------------------------------------------------------------------------------
        public ActionResult employeeMaster() //人事資料主頁
        {
            var query = from o in db.WorkInfo
                        select o;
            var query1 = from x in db.Factory
                         select x;
            var query2 = from o in db.VW_EmpWorkInfo
                         where o.employee_workNumber == null
                         select o;
            basicDataEmployeeMaster data = new basicDataEmployeeMaster();
            data.workInfo = query.ToList();
            data.factory = query1.ToList();
            data.vW_EmpWorkInfo = query2.ToList();
            return View(data);
        }
        [HttpPost]
        public ActionResult employeeMaster(Factory factory, Employee employee, WorkInfo workInfo) //人事資料主頁
        {
            //若沒有輸入post回去會是null值,把它改成""去做模糊查詢
            if (string.IsNullOrEmpty(employee.employee_name))
            {
                employee.employee_name = "";
            }
            if (string.IsNullOrEmpty(employee.employee_workNumber))
            {
                employee.employee_workNumber = "";
            }
            if (string.IsNullOrEmpty(employee.employee_identityCard))
            {
                employee.employee_identityCard = "";
            }
            if (string.IsNullOrEmpty(employee.employee_isDisabled))
            {
                employee.employee_isDisabled = "";
            }
            if (string.IsNullOrEmpty(workInfo.work_name))
            {
                workInfo.work_name = "";
            }
            //先將工廠名稱轉換成工廠ID
            var query = from f in db.Factory
                        where f.factory_name == factory.factory_name
                        select f.factory_id;
            string factoryID = query.First();
            //工種名稱轉換成工種ID
            var query1 = from w in db.WorkInfo
                         where w.work_name.Contains(workInfo.work_name)
                         select w.work_id;
            int workID = 0; //當workID等於0就是查詢全部
            string workname = "";
            if (query1.Count() == 1)
            {
                workID = query1.First();
            }
            if (workID == 3)
            {
                workname = "一般人員";
            }
            if (workID == 4)
            {
                workname = "高階主管";
            }
            //轉換完畢
            var query2 = from e in db.VW_EmpWorkInfo
                         where (e.employee_Workid == workID &&
                                e.employee_name.Contains(employee.employee_name) &&
                                e.employee_workNumber.Contains(employee.employee_workNumber) &&
                                e.employee_factoryId == factoryID &&
                                e.employee_identityCard.Contains(employee.employee_identityCard) &&
                                e.employee_isDisabled.Contains(employee.employee_isDisabled))
                                ||
                               (e.work_name == workname &&
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

            //var query2 = from e in db.Employee
            //             where (e.employee_workId == workID &&
            //                   e.employee_name.Contains(employee.employee_name) &&
            //                   e.employee_workNumber.Contains(employee.employee_workNumber) &&
            //                   e.employee_factoryId == factoryID &&
            //                   e.employee_identityCard.Contains(employee.employee_identityCard) &&
            //                   e.employee_isDisabled.Contains(employee.employee_isDisabled))
            //                   ||
            //                   (workID == 0 &&
            //                   e.employee_name.Contains(employee.employee_name) &&
            //                   e.employee_workNumber.Contains(employee.employee_workNumber) &&
            //                   e.employee_factoryId == factoryID &&
            //                   e.employee_identityCard.Contains(employee.employee_identityCard) &&
            //                   e.employee_isDisabled.Contains(employee.employee_isDisabled))
            //             select e;
            var query3 = from o in db.WorkInfo
                         select o;
            var query4 = from x in db.Factory
                         select x;
            basicDataEmployeeMaster data = new basicDataEmployeeMaster();
            data.vW_EmpWorkInfo = query2.ToList();
            data.workInfo = query3.ToList();
            data.factory = query4.ToList();
            return View(data);
        }
        public ActionResult employeeMain() //人事資料主檔,明細檔-工種,明細檔-健檢記錄(編輯完成後使用的ActionResult)
        {
            string workNumber = Session["workNumber"].ToString();
            int workId = Convert.ToInt32(Session["workId"]);
            var query = from o in db.Employee
                        where o.employee_workNumber == workNumber
                        select o;
            var query1 = from x in db.WorkInfo
                         where x.work_id == workId
                         select x;
            var query2 = from z in db.ReportManage
                         where z.ReportManage_workNumber == workNumber
                         select z;
            var query3 = from a in db.VW_EmpWorkInfo
                         where a.employee_workNumber == workNumber
                         select a;

            basicDataEmpolyeeMain data = new basicDataEmpolyeeMain();
            data.employee = query.ToList();
            data.workInfo = query1.ToList();
            data.reportManage = query2.ToList();
            data.vW_EmpWorkInfo = query3.ToList();

            return View(data);
        }
        [HttpPost]
        public ActionResult employeeMain(VW_EmpWorkInfo employee) //人事資料主檔,明細檔-工種,明細檔-健檢記錄(查詢完成按下放大鏡後使用的ActionResult)
        {
            var query = from o in db.Employee
                        where o.employee_workNumber == employee.employee_workNumber
                        select o;
            var query1 = from x in db.WorkInfo
                         where x.work_id == employee.employee_Workid
                         select x;
            var query2 = from z in db.ReportManage
                         where z.ReportManage_workNumber == employee.employee_workNumber
                         select z;
            var query3 = from a in db.VW_EmpWorkInfo
                         where a.employee_workNumber == employee.employee_workNumber
                         select a;

            basicDataEmpolyeeMain data = new basicDataEmpolyeeMain();
            data.employee = query.ToList();
            data.workInfo = query1.ToList();
            data.reportManage = query2.ToList();
            data.vW_EmpWorkInfo = query3.ToList();

            Session["workNumber"] = employee.employee_workNumber;
            Session["workId"] = employee.employee_Workid;
            return View(data);
        }
        public ActionResult editEmployee() //更改人事主檔
        {
            string workNumber = Session["workNumber"].ToString();
            var query = from o in db.Employee
                        where o.employee_workNumber == workNumber
                        select o;
            List<Employee> data = query.ToList();

            return View(data);
        }
        [HttpPost]
        public ActionResult editEmployee(Employee editData)
        {
            Employee data = db.Employee.Find(Session["workNumber"]);
            data.employee_reportAddress = editData.employee_reportAddress;
            data.employee_reportMobile = editData.employee_reportMobile;
            data.employee_note = editData.employee_note;
            db.SaveChanges();
            return RedirectToAction("employeeMain", "basicData");
        }
        public ActionResult editEmployeeWork() //更改所屬工種
        {
            var query = from o in db.WorkInfo
                        where o.work_id != 3 && o.work_id != 4
                        select o;
            List<WorkInfo> data = query.ToList();
            return View(data);
        }
        [HttpPost]
        public ActionResult editEmployeeWork(string work_name)
        {
            //將工種名稱轉換成id
            var query = from o in db.WorkInfo
                        where o.work_name == work_name
                        select o.work_id;
            int workID = query.First();

            //將工號轉換成EmployeeWork表對應的ID
            string workNumber = Session["workNumber"].ToString();
            var query1 = from o in db.EmployeeWork
                         where o.employee_workNumber == workNumber
                         select o.Id;
            int EmployeeWorkID = query1.First();

            //修改資料並儲存
            EmployeeWork data = db.EmployeeWork.Find(EmployeeWorkID);
            data.employee_Workid = workID;
            db.SaveChanges();

            return RedirectToAction("employeeMain", "basicData");
        }
        public ActionResult delEmployeeWork()
        {
            string workNumber = Session["workNumber"].ToString();
            var query = from o in db.EmployeeWork
                        where o.employee_workNumber == workNumber
                        select o.Id;
            int id = query.First();
            EmployeeWork data = db.EmployeeWork.Find(id);
            data.workId_isDisable = "Y";
            db.SaveChanges();

            return RedirectToAction("employeeMain", "basicData");
        }
        //醫院------------------------------------------------------------------------------
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
        public ActionResult delProgram(int program_programId)
        {
            Program data = db.Program.Find(program_programId);
            data.program_state = "Y";
            db.SaveChanges();
            return RedirectToAction("hospitalMain", "basicData");
        }
        public ActionResult editProgram(int program_programId)
        {
            var query = from o in db.Program
                        where o.program_programId == program_programId
                        select o;
            basicDataEditProgram data = new basicDataEditProgram();
            data.program = query.ToList();
            var query1 = from o in db.WorkInfo
                         select o;
            data.workInfo = query1.ToList();
            return View(data);
        }
        [HttpPost]
        public ActionResult editProgram(Program program, WorkInfo workInfo)
        {
            var query = from o in db.WorkInfo
                        where o.work_name == workInfo.work_name
                        select o.work_id;
            int workId = query.First();
            Program data = db.Program.Find(program.program_programId);
            data.program_programName = program.program_programName;
            data.program_content = program.program_content;
            data.program_workid = workId;
            data.program_updateuser = program.program_updateuser;
            data.program_updatetime = program.program_updatetime;
            db.SaveChanges();
            return RedirectToAction("hospitalMain", "basicData");
        }

        public ActionResult DownloadFile(int id) //檔案下載
        {

            HospitalProgramFile Download = db.HospitalProgramFile.Find(id);
            if (Download != null)
            {
                Stream iStream = new FileStream(Download.file_url, FileMode.Open, FileAccess.Read, FileShare.Read);
                return File(iStream, Download.file_url, Download.hos_FileName);



            }
            else
            {

                return JavaScript("alert(\"無此檔案\")");

            }
        }

        //上傳頁面
        public ActionResult FileUpload()
        {
            HospitalProgramFile hosFileInfo = new HospitalProgramFile();
            return View(hosFileInfo);

        }//附件管理上傳
        [HttpPost]
        public ActionResult FileUpload(FormCollection frm)//附件管理上傳POST
        {

            HttpPostedFileBase hosFile = Request.Files["hos_FileName"];
            int hosId = Convert.ToInt32(Session["hospitalID"]);
            string FileContent = Request["FileContent"];
            string UploadedhosFile = Server.MapPath("~\\Upload") + "\\" + hosFile.FileName;

            hosFile.SaveAs(UploadedhosFile);


            HospitalProgramFile hosFileInfo = new HospitalProgramFile()
            {
                file_content = FileContent,
                hos_FileName = hosFile.FileName,
                file_hospitalId = hosId,
                file_url = UploadedhosFile

            };
            db.HospitalProgramFile.Add(hosFileInfo);
            db.SaveChanges();
            TempData["uploadFileMessage"] = "上傳成功";
            return View();
        }
        [HttpPost]
        public ActionResult FileUploadDelete(int file_fileId)//檔案刪除
        {
            var query = (from d in db.HospitalProgramFile
                         where d.file_fileId == file_fileId
                         select d).FirstOrDefault();
            ////判斷此id是否有資料
            if (query != default(HospitalProgramFile))
            {
                db.HospitalProgramFile.Remove(query);
                //儲存所有變更
                db.SaveChanges();
                TempData["ResultMessage"] = String.Format("", query.hos_FileName);
                return RedirectToAction("hospitalMain", "basicData");
            }
            else
            {
                //如果沒有資料則顯示錯誤訊息並導回StoreMember頁面
                TempData["ResultMessage"] = "指定資料不存在，無法刪除，請重新操作";
                return RedirectToAction("hospitalMain", "basicData");
            }

        }
        public ActionResult FileUploadEdit(int file_fileId)//得到編輯資料
        {
            HospitalProgramFile filedata = db.HospitalProgramFile.Find(file_fileId);
            //int fileId = Convert.ToInt32(Request["file_fileId"]);
            //var query = from o in db.HospitalProgramFile
            //            where o.file_fileId == file_fileId
            //            select o;
            //HospitalProgramFile data = query.FirstOrDefault();
            return View(filedata);

        }
        [HttpPost]
        public ActionResult FileUploadEdit(HospitalProgramFile editFileData, int file_fileId)//送出編輯資料
        {
            //int fileId = Convert.ToInt32(Request["file_fileId"]);
            HospitalProgramFile data = db.HospitalProgramFile.Find(file_fileId);
            data.file_content = editFileData.file_content;

            db.SaveChanges();
            return RedirectToAction("hospitalMain", "basicData");


        }

        public ActionResult Register(FormCollection frm)
        {
            string employee_workNumber = Session["employee_acc"].ToString();
            var q = from o in db.Employee
                    where o.employee_workNumber == employee_workNumber
                    select o;
            var data = q.ToList();
            return View(data);
        }
        [HttpPost]
        public ActionResult Register(Employee emp)
        {
            string employee_workNumber = "";
            string employee_password = "";
            string employee_username = "";
            /*foreach(var item in emp.EmpView) {*/
            employee_workNumber = emp.employee_workNumber;
            //employee_username = emp.employee_username;
            //employee_password = emp.employee_password;
            //}
            //employee_workNumber = Request["employee_workNumber"];
            if (string.IsNullOrEmpty(Request["employee_username"]))
            {
            }
            else
                employee_username = Request["employee_username"];
            if (string.IsNullOrEmpty(Request["employee_password"]))
            {
            }
            else
                employee_password = Request["employee_password"];

            if (employee_username == "")
            {
                var q = from o in db.Employee
                        where o.employee_workNumber == employee_workNumber
                        select o;
                var result = q.ToList();
                return View("Register", result);
            }
            else
            {
                var data = db.Employee.Find(employee_workNumber);
                data.employee_username = employee_username;
                data.employee_password = employee_password;
                db.SaveChanges();
                //Session.Remove("employee_acc");

                return RedirectToAction("purviewMaster", "basicData");
            }
        }
    }
    }