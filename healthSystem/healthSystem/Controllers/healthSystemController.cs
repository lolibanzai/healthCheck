using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using healthSystem.Models;
using healthSystem.ViewModel;
using System.IO;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Data.OleDb;
using System.Net.Mail;
using System.Net;

namespace healthSystem.Controllers
{
    public class healthSystemController : Controller
    {
        HealthCheckEntities1 db = new HealthCheckEntities1();
        // GET: healthSystem
        public ActionResult StartCheckMaster() //啟動作業主頁
        {
            healthSystemStartCheckMaster data = new healthSystemStartCheckMaster();

            //查詢存在的年度,Distinct()可以去除掉查詢出來後重複的值
            var query = from o in db.StartCheck
                        select o.start_year;
            data.year = query.Distinct().ToList();
            //查詢空的給StartCheck
            var query1 = from o in db.StartCheck
                         where o.Start_id == null
                         select o;
            data.startCheck = query1.ToList();
            return View(data);
        }
        [HttpPost]
        public ActionResult StartCheckMaster(StartCheck data)
        {
            if (string.IsNullOrEmpty(data.start_state))
            {
                data.start_state = "";
            }
            healthSystemStartCheckMaster viewData = new healthSystemStartCheckMaster();
            //查詢存在的年度,Distinct()可以去除掉查詢出來後重複的值
            var query = from o in db.StartCheck
                        select o.start_year;
            viewData.year = query.Distinct().ToList();
            //查詢後給StartCheck
            var query1 = from o in db.StartCheck
                         where o.start_year == data.start_year && o.start_state.Contains(data.start_state)
                         select o;
            viewData.startCheck = query1.ToList();

            return View(viewData);
        }
        public ActionResult StartCheckMain() //啟動作業主檔
        {
            return View();
        }
        [HttpPost]
        public ActionResult StartCheckMain(int Start_id) //啟動作業主檔
        {
            healthSystemStartCheckMain data = new healthSystemStartCheckMain();
            //主檔
            var query = from o in db.StartCheck
                        where o.Start_id == Start_id
                        select o;
            data.startCheck = query.ToList();
            //受檢單位
            var query1 = from o in db.StartPlace
                         where o.startplace_startId == Start_id
                         select o;
            data.startPlace = query1.ToList();
            //附件管理
            var query2 = from o in db.StartFile
                         where o.start_id == Start_id
                         select o;
            data.startFile = query2.ToList();
            return View(data);
        }
        public ActionResult newStartCheck() //啟動健檢
        {
            return View();
        }
        [HttpPost]
        public ActionResult newStartCheck(StartCheck data, string noteText) //啟動健檢
        {
            StartCheck newStartCheck = new StartCheck()
            {
                start_year = data.start_year,
                start_principal = data.start_principal,
                start_state = data.start_state,
                start_healthCheckDate = data.start_healthCheckDate
            };
            db.StartCheck.Add(newStartCheck);
            db.SaveChanges();

            //找出剛剛新增的啟動健檢的ID
            var query = from o in db.StartCheck
                        orderby o.Start_id descending
                        select o.Start_id;
            int newid = query.First();
            string NewID = newid.ToString();


            //補上start_note的值
            StartCheck edit = db.StartCheck.Find(newid);
            edit.start_note = "note" + NewID + ".txt";
            db.SaveChanges();

            string path = "~/txt/note" + NewID + ".txt";
            FileStream fileStream = new FileStream(Server.MapPath(path), FileMode.Create);
            fileStream.Close();   //切記開了要關,不然會被佔用而無法修改喔!!!
            using (StreamWriter sw = new StreamWriter(Server.MapPath(path)))
            {
                // 欲寫入的文字資料 ~
                sw.Write(noteText);
            }

            return RedirectToAction("StartCheckMaster", "healthSystem");
        }
        public ActionResult newStartPlace(int Start_id)
        {
            
            //宣告一個ViewModel_StartFac類別
            ViewModel_StartFac data = new ViewModel_StartFac();
            //查詢Factory資料
            var q1 = from o in db.Factory
                     select o;
            //把StartId丟入ViewModel_StartFac下StartId中
            data.StartId = Start_id;
            //創一個新的啟動作業跟廠別對應資料表
            List<StartPlace> startPlace = new List<StartPlace>();
            //把Factory與StartPlace資料丟入ViewModel_startPlace
            data.Factory = q1.ToList();
            data.Start = startPlace;
            return View(data);
        }
        [HttpPost]
        //Post回新增廠別資料
        public ActionResult newStartPlace(ViewModel_StartFac data)
        {
            //宣告一個action變數，接收是新增還是刪除啟動作業下的廠別
            int action = Convert.ToInt32(Request["AddorRemove"].ToString());
            //宣告一個變數接啟動作業ID
            int Start_id = Convert.ToInt32(Request["Start_id"].ToString());
            //宣告一個變數接廠別編號
            string factoryid = Request["factoryId"];
            try
            {
                //action為時，新增啟動作業廠別
                if (action == 0)
                {
                    //創新資料行
                    StartPlace star = new StartPlace()
                    {
                        startplace_startId = Start_id,
                        startPlace_factoryId = factoryid
                    };
                    db.StartPlace.Add(star);
                    //存回資料庫
                    db.SaveChanges();
                }
                //action為1，表示刪除資料行
                if (action == 1)
                {
                    //先查詢該筆資料
                    var query = (from o in db.StartPlace
                                 where o.startplace_startId == Start_id && o.startPlace_factoryId == factoryid
                                 select o).FirstOrDefault();
                    //把該筆資料標記為刪除
                    db.StartPlace.Remove(query);
                    //存回資料庫，並刪除該筆
                    db.SaveChanges();
                }
            }
            catch (Exception ex) { return Content(ex.ToString()); } //如果資料庫寫入錯誤，拋出錯誤
            //重新查詢廠別資料
            var q1 = from o in db.Factory
                     select o;
            //把啟動作業ID給data的StartId
            data.StartId = Start_id;
            //把廠別資料給data的Factory
            data.Factory = q1.ToList();
            //把啟動作業的表丟給data的Start
            data.Start = (from o in db.StartPlace
                          select o).ToList();
            return View(data);
        }
        //----------------------------------------------------------
        public ActionResult checkCollectMaster() //收集作業主頁
        {
            healthSystemCheckCollectMaster data = new healthSystemCheckCollectMaster();
            var query = from o in db.StartCheck
                        where o.start_state == "處理中"
                        select o;
            data.beginSstartCheck = query.ToList();
            var query1 = from o in db.StartCheck
                         where o.start_state == "健檢收集"
                         select o;
            data.startCheck = query1.ToList();
            return View(data);
        }
        [HttpPost]
        public ActionResult checkCollectMaster(int x)
        {
            return View();
        }

        public ActionResult checkCollect(int StartID, int startYear, int saveSignal) //收集作業
        {
            Session["checkCollectStartID"] = StartID;
            Session["checkCollectstartYear"] = startYear;
            healthSystemCheckCollect data = new healthSystemCheckCollect();

            data.StartID = StartID;//把StartID存到ViewModel給手動收集的新增人員使用

            //列出目前的廠別
            var query = from o in db.StartPlace
                        where o.startplace_startId == StartID
                        select o;
            data.startPlace = query.ToList();

            //列出待驗的人員
            var query1 = from o in db.StartPlace  //先查出這個健檢計畫的受檢廠別有哪些
                         where o.startplace_startId == StartID
                         select o.startPlace_factoryId;
            List<string> factoryID = query1.ToList(); //把廠別存成List

            List<string> allWorkNumber = new List<string>(); //存放此健檢計畫受檢單位的一般員工
            List<int> allWorkNumberAge = new List<int>(); //存放此健檢計畫受檢單位的一般員工的年齡
            List<string> startHandWorkNumber = new List<string>(); //存放挑選過後該去健檢的員工(存入startHand時使用)
            List<string> startHandFactoryID = new List<string>(); //存放挑選過後該去健檢的員工的廠別代號(存入startHand時使用)

            List<Employee> employeeList = new List<Employee>(); //創造一個List類別為Employee,物件名稱為employeeList
            foreach (var ID in factoryID) //把查到的廠別依序取出來查詢所屬此廠別的員工
            {
                var query2 = from o in db.Employee
                             where o.employee_factoryId == ID && o.employee_workId == 3 //o.employee_workId == 3:找出身分為一般員工的人員
                             select o.employee_workNumber;
                allWorkNumber.AddRange(query2.ToList()); //存入allWorkNumber
            }

            foreach (var number in allWorkNumber) //查詢allWorkNumber裡面員工的年齡
            {
                var query4 = from o in db.Employee
                             where o.employee_workNumber == number
                             select o.employee_age;
                allWorkNumberAge.AddRange(query4.ToList());
            }
            //allWorkNumber和allWorkNumberAge兩個List存放的資料
            // List<string> allWorkNumber  |  List<int> allWorkNumberAge
            // 員工工號A                   |  員工編號A的年齡            編號第0筆
            // 員工工號B                   |  員工編號B的年齡            編號第1筆
            // 員工工號C                   |  員工編號C的年齡            編號第2筆
            // 員工工號D                   |  員工編號D的年齡            編號第3筆
            // 員工工號E                   |  員工編號E的年齡            編號第4筆

            //用for迴圈去繞出編號第0筆的員工工號與年齡,即可對應
            for (int i = 0; i <= allWorkNumber.Count() - 1; i++)
            {
                //檢查年齡確定級距
                int level = 0;
                if (allWorkNumberAge[i] < 40)
                {
                    level = 5; //40歲以下的級距為每5年一次
                }
                if (40 <= allWorkNumberAge[i] && allWorkNumberAge[i] < 65)
                {
                    level = 3; //40歲以上未滿65歲的級距為每3年一次
                }
                if (65 <= allWorkNumberAge[i])
                {
                    level = 1; //65歲以上的級距為每5年一次
                }
                //查詢ReportCheckItem查出 編號第i筆 最近一次健檢的年度,存到lastcheckYear
                string stringAllWorkNumber = allWorkNumber[i];
                var query5 = from o in db.ReportCheckItem
                             where o.ReportCheckItem_employee_workNumber == stringAllWorkNumber
                             orderby o.ReportCheckItem_checkYear descending
                             select o.ReportCheckItem_checkYear;

                int? lastcheckYear = query5.FirstOrDefault(); //存入
                if (!lastcheckYear.HasValue)
                {
                    lastcheckYear = startYear - 1;
                }

                //最近健檢的年度(newcheckYear)加上他的級距(level)即為他應該健檢的年度
                //如果跟這次啟動作業的年度一樣的話表示他就是今年要去健檢
                if (lastcheckYear + level == startYear)
                {
                    //查出這人在Employee的資料存入employeeList
                    var query6 = from o in db.Employee
                                 where o.employee_workNumber == stringAllWorkNumber
                                 select o;
                    employeeList.AddRange(query6.ToList());
                    //查出這人在Employee的員工編號
                    var query7 = from o in db.Employee
                                 where o.employee_workNumber == stringAllWorkNumber
                                 select o.employee_workNumber;
                    startHandWorkNumber.AddRange(query7.ToList());
                    //查出這人在Employee的廠別代號
                    var query8 = from o in db.Employee
                                 where o.employee_workNumber == stringAllWorkNumber
                                 select o.employee_factoryId;
                    startHandFactoryID.AddRange(query8.ToList());
                }
            }//for繞完後就已經篩選出應該要去健檢的人


            data.employee = employeeList; //存入ViewModel的employee做顯示


            //儲存被按下後將系統收集的人員存入
            if (saveSignal == 0)
            {
                foreach (var workNumber in startHandWorkNumber)
                {
                    //產生流水號
                    var query8 = from o in db.StartHand
                                 where o.startHand_checkId == StartID
                                 orderby o.startHand_serialNumber descending
                                 select o.startHand_serialNumber;
                    int serialNumber = query8.FirstOrDefault() + 1;
                    string stringSerialNumber = serialNumber.ToString();
                    //找出廠別代碼
                    var query9 = from o in db.Employee
                                 where o.employee_workNumber == workNumber
                                 select o.employee_factoryId;
                    string factoryId = query9.FirstOrDefault();
                    //將系統收集的資料存入startHand裡面
                    StartHand newStartHand = new StartHand()
                    {
                        startHand_checkId = StartID,
                        startHand_workNumber = workNumber,
                        startHand_type = "系統收集",
                        startHand_No = "RG" + startYear.ToString() + stringSerialNumber.PadLeft(5, '0') + factoryId.PadLeft(4, '0'),
                        startHand_serialNumber = serialNumber,
                        startHand_updateTime = DateTime.Now
                    };
                    db.StartHand.Add(newStartHand);
                    db.SaveChanges();
                }
            }
            //列出startHand人員(以儲存人員)
            var query3 = from o in db.StartHand
                         where o.startHand_checkId == StartID
                         select o;
            data.startHand = query3.ToList();

            return View(data);
        }
        public ActionResult stateChange(int StartID, int startYear)
        {
            //狀態更改為健檢收集
            StartCheck data = db.StartCheck.Find(StartID);
            data.start_state = "健檢收集";
            data.start_startDate = DateTime.Now;
            db.SaveChanges();

            return RedirectToAction("checkCollectMaster", "healthSystem");
        }
        public ActionResult stateChangeAndMail(int StartID, int startYear)
        {
            //狀態更改為結案
            StartCheck data = db.StartCheck.Find(StartID);
            data.start_state = "結案";
            data.start_CheckEndDate = DateTime.Now;
            db.SaveChanges();

            //找出這次健檢的人的workNumber
            var query = from o in db.StartHand
                        where StartID == o.startHand_checkId
                        select o.startHand_workNumber;
            List<string> workNumber = query.ToList();

            //開始執行寄出Email的程式

            //先找出每個人的Email並且用逗號串接起來存入email
            string email = "";
            foreach (var Number in workNumber)
            {
                var query1 = from o in db.Employee
                             where Number == o.employee_workNumber
                             select o.employee_email;
                var item = query1.FirstOrDefault();
                email += item + ",";
            }
            email = email.Substring(0, email.Length - 1); //存入email
            //return Content(email); //測試用


            //找出該健檢方案的體檢時間
            var query2 = from o in db.StartCheck
                         where StartID == o.Start_id
                         select o.start_healthCheckDate;
            string healthCheckDate = query2.FirstOrDefault().ToShortDateString();

            //return Content(healthCheckDate); //測試用



            //設定Email資料
            //設定smtp主機
            string smtpAddress = "smtp.gmail.com";
            //設定Port
            int portNumber = 587;
            bool enableSSL = true;
            //填入寄送方email和密碼
            string emailFrom = "";
            string password = "";
            //收信方email
            string emailTo = email;
            //主旨
            string subject = "健檢通知";
            //內容
            string body = "健檢日期為" + healthCheckDate;

            //return Content(body); //測試用

            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(emailFrom);
                mail.To.Add(emailTo);
                mail.Subject = subject;
                mail.Body = body;
                // 若你的內容是HTML格式，則為True
                mail.IsBodyHtml = false;

                ////夾帶檔案
                //mail.Attachments.Add(new Attachment("C:\\SomeFile.txt"));
                //mail.Attachments.Add(new Attachment("C:\\SomeZip.zip"));

                using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber))
                {
                    smtp.Credentials = new NetworkCredential(emailFrom, password);
                    smtp.EnableSsl = enableSSL;
                    smtp.Send(mail);
                }
            }
            //寄出Email程式結束


            //紀錄發送日期
            foreach (var Number in workNumber)
            {
                var query3 = from o in db.StartHand
                             where o.startHand_workNumber == Number
                             select o.startHand_id;
                int id = query3.First();
                StartHand sendMailTime = db.StartHand.Find(id);
                sendMailTime.startHand_sendMailTime = DateTime.Now;
            }

            return RedirectToAction("checkCollectMaster", "healthSystem");
        }
        public ActionResult startHand()
        {
            //宣告變數
            string factoryId = Request["factoryId"];
            int startId = Convert.ToInt32(Request["StartID"]);
            //需告一個新的ViewModel_StHanEmp
            ViewModel_StHanEmp data = new ViewModel_StHanEmp();
            //查詢空資料行
            var query = (from o in db.Employee
                         where o.employee_factoryId == ""
                         select o).ToList();
            //把空員工資料行與啟動作業編號丟入
            data.Employee = query;
            data.StartId = startId;
            return View(data);
        }
        //POST啟動作業
        [HttpPost]
        public ActionResult startHand(ViewModel_StHanEmp data)
        {
            //宣告變數接收傳回值
            string employee_workNumber = Request["employee_WorkNumber"];
            string employee_name = Request["employee_Name"];
            string factory_Id = Request["factoryId"];
            int startId = data.StartId;
            Session["startId"]= startId;
            //用action決定動作 0表示查詢顯示資料
            int action = Convert.ToInt32(Request["action"]);

            try
            {
                //action =1時，新增新資料行
                if (action == 1)
                {
                    //找出年度
                    var query1 = from o in db.StartCheck
                                 where o.Start_id == startId
                                 select o.start_year;
                    int year = query1.FirstOrDefault();
                    //產生流水號
                    var query8 = from o in db.StartHand
                                 where o.startHand_checkId == startId
                                 orderby o.startHand_serialNumber descending
                                 select o.startHand_serialNumber;
                    int serialNumber = query8.FirstOrDefault() + 1;
                    string stringSerialNumber = serialNumber.ToString();
                    //找出廠別代碼
                    var query9 = from o in db.Employee
                                 where o.employee_workNumber == employee_workNumber
                                 select o.employee_factoryId;
                    string factoryId = query9.FirstOrDefault();
                    //找出更新者
                    string updateUserWorkNumber = Session["employee_workNumber"].ToString();
                    var query2 = from o in db.Employee
                                 where o.employee_workNumber == updateUserWorkNumber
                                 select o.employee_name;
                    string updateUserName = query2.FirstOrDefault();

                    StartHand startHand = new StartHand()
                    {

                        startHand_checkId = startId,
                        startHand_workNumber = employee_workNumber,
                        startHand_type = "手動收集",
                        startHand_No = "RG" + year.ToString() + stringSerialNumber.PadLeft(5, '0') + factoryId.PadLeft(4, '0'),
                        startHand_serialNumber = serialNumber,
                        standHand_updateUser = updateUserName,
                        startHand_updateTime = DateTime.Now

                    };
                    //StartHand加入新資料行
                    db.StartHand.Add(startHand);
                    //寫回資料庫
                    db.SaveChanges();
                    employee_workNumber = "";
                }
                //action=2，刪除資料表
                if (action == 2)
                {
                    employee_workNumber = "";
                    //查詢要刪除的資料行
                    var q2 = (from o in db.StartHand
                              where o.startHand_checkId == startId && o.startHand_workNumber == employee_workNumber
                              select o).FirstOrDefault();
                    //把該筆資料標記為刪除
                    db.StartHand.Remove(q2);
                    //存回資料庫，並刪除該筆
                    db.SaveChanges();
                }
                //try有錯時，拋出錯誤
            }
            catch (Exception ex) { return Content(ex.ToString()); }
            //假如employee_workNumber為空
            if (string.IsNullOrEmpty(employee_workNumber))
                //把employee_workNumber值設為空字串
                employee_workNumber = "";
            //假如employee_name為空
            if (string.IsNullOrEmpty(employee_name))
                //把employee_name值設為空字串
                employee_name = "";
            //假如factory_Id為空
            if (string.IsNullOrEmpty(factory_Id))
                //把factory_Id值設為空字串
                factory_Id = "";
            //模糊查詢員工編號、員工姓名、廠別代號
            var query = from o in db.Employee
                        where o.employee_workId == 3 &&
                              o.employee_workNumber.Contains(employee_workNumber) &&
                              o.employee_name.Contains(employee_name) &&
                              o.employee_factoryId.Contains(factory_Id)
                        select o;
            //把查詢到的員工資料丟入ViewModel_StHanEmp裡
            data.Employee = query.ToList();
            data.StartId = data.StartId;
            return View(data);
        }
        //----------------------------------------------------------
        public ActionResult bookMaster() //預約健檢主頁
        {
            //string bookYear = Request["year"];
            //string start_date = Request["startbook_date"];
            //string end_date = Request["endbook_date"];
            //string bookState = Request["book_state"];
            //string factoryId = Request["employee_factoryId"];
            //string employee_name = Request["employee_name"];
            //string employeeWorkNumber = Request["employee_WorkNumber"];
            //string book_IsDisable = Request["book_IsDisable"];

            //宣告一個員工編號變數
            string employeeWorkNumber = Session["employee_workNumber"].ToString();
            //宣告一個新的ViewModel_BookEmp
            ViewModel_BookEmp data = new ViewModel_BookEmp();
            //查詢員工與預約健檢的檢視表
            var q = from o in db.VW_BookEmp
                    where o.employee_workNumber == employeeWorkNumber
                    select o;
            //查詢員工與權限的檢視表
            var q1 = from o in db.VW_EmpAuth
                     where o.employee_workNumber == employeeWorkNumber
                     select o;
            //查詢員工資料表
            var q2 = from o in db.Employee
                     where o.employee_workNumber == employeeWorkNumber
                     select o;
            //把查詢到的資料放入ViewModel_BookEmp中
            data.BookView = q.ToList();
            data.EmpView = q1.ToList();
            data.Emp = q2.ToList();
            return View(data);
        }
        [HttpPost]
        public ActionResult bookMaster(ViewModel_BookEmp data) //預約健檢主頁
        {
            //宣告一個健檢年度變數
            string bookYear = Request["book_year"].ToString();
            //宣告一個年度整數
            int year = Convert.ToInt32(Request["book_year"]);
            //宣告查詢時間(開始與結束)
            DateTime start_date = new DateTime();
            DateTime end_date = new DateTime();
            //宣告一個健檢狀態變數
            string bookState = Request["book_state"];
            //宣告一格廠別編號變數
            string factoryId = Request["employee_factoryId"];
            //宣告一格員工姓名變數
            string employee_name = Request["employeeName"];
            //宣告一個員工編號變數
            string employeeWorkNumber = Request["employee_WorkNumber"];
            //宣告一個健檢是否停用變數
            string book_IsDisable = Request["book_IsDisable"];
            //假如健檢年度為空或無效值
            if (string.IsNullOrEmpty(bookYear))
            {
                //健檢年度為空字串
                bookYear = "";
            }
            //假如查詢開始日期為空或無效值
            if (string.IsNullOrEmpty(Request["startbook_date"]))
            {

            }
            else
                //否則把輸入開始查詢日期給開始日期
                start_date = Convert.ToDateTime(Request["startbook_date"]);
            //假如查詢結束日期為空或無效值
            if (string.IsNullOrEmpty(Request["endbook_date"]))
            {
                end_date = DateTime.Now;
            }
            else
                //否則把輸入結束查詢日期給開始日期
                end_date = Convert.ToDateTime(Request["endbook_date"]);
            //判斷bookState是否為空
            if (string.IsNullOrEmpty(bookState))
            {
                //是給空字串
                bookState = "";
            }
            //判斷廠別是否為空
            if (string.IsNullOrEmpty(factoryId))
            {
                //是給空字串
                factoryId = "";
            }
            //判斷員工姓名是否為空
            if (string.IsNullOrEmpty(employee_name))
            {
                //是給空字串
                employee_name = "";
            }
            //判斷員工編號是否為空
            if (string.IsNullOrEmpty(employeeWorkNumber))
            {
                //是給空字串
                employeeWorkNumber = "";
            }
            //判斷預約健檢停用是否為空
            if (string.IsNullOrEmpty(book_IsDisable))
            {
                //是給空字串
                book_IsDisable = "";
            }
            //查詢員工資料表
            var q = (from o in db.VW_BookEmp
                     where o.book_Id.Contains(bookYear) &&
                            o.book_year == year &&
                            (o.book_Date >= start_date && o.book_Date <= end_date) &&
                            o.employee_name.Contains(employee_name) &&
                            o.employee_workNumber.Contains(employeeWorkNumber) &&
                            o.book_state.Contains(bookState) &&
                            o.employee_factoryId.Contains(factoryId) &&
                            o.book_IsDisable.Contains(book_IsDisable)
                     select o).ToList();
            //查詢員工權限檢視表
            var q1 = from o in db.VW_EmpAuth
                     where o.employee_workNumber == employeeWorkNumber
                     select o;
            //查詢員工資料表
            var q2 = (from o in db.Employee
                      where o.employee_workNumber == employeeWorkNumber
                      select o).ToList();
            //把資料放入ViewModel_BookEmp中
            data.BookView = q;
            data.EmpView = q1;
            data.Emp = q2;
            return View(data);
        }
        public ActionResult bookMain() //預約健檢主檔
        {
            //宣告一個預約健檢ID變數
            string bookId = Request["book_Id"];
            //宣告一個ViewModel_BookAction方法
            ViewModel_BookAction data = new ViewModel_BookAction();
            //查詢預約健檢與員工資料表
            var q = from o in db.VW_BookEmp
                    where o.book_Id == bookId
                    select o;
            //把查詢到的給ViewModel_BookAction
            data.GetW_BookEmp = q.ToList();
            //宣告一個Action出值為0
            data.GetAction = 0;
            return View(data);
        }


        [HttpPost]
        public ActionResult bookMain(ViewModel_BookAction data)
        {
            //宣告一個預約健檢單號變數
            string bookid = Request["book_Id"];
            //初始化更新者變數
            string updateuser = "";
            //宣告一個Emp方法
            Emp emp = new Emp();
            //把更新者用登入者員工編號去查出姓名
            updateuser = emp.Name(Session["employee_workNumber"].ToString());
            //宣告一個action變數
            int action = Convert.ToInt32(Request["action"]);
            try
            {
                //假如變數為1(無用函數)
                if (action == 1)
                {
                    //查詢預約健檢表
                    var q1 = from o in db.Book
                             where o.book_id == bookid
                             select o;
                    foreach (var item1 in q1)
                    {
                        //item1.
                    }
                }
                //假如action為2，取消預約健檢
                if (action == 2)
                {
                    //查詢預約健檢表
                    var q2 = from o in db.Book
                             where o.book_id == bookid
                             select o;
                    //把預約健檢表的內容更改
                    foreach (var item2 in q2)
                    {
                        item2.book_IsDisable = "Y";
                        item2.book_updateUser = updateuser;
                        item2.book_updateDate = DateTime.Now;
                    }
                    db.SaveChanges();
                }
                //判斷action是否為3
                if (action == 3)
                {
                    //查詢預約健檢資料表
                    var q3 = from o in db.Book
                             where o.book_id == bookid
                             select o;
                    //把預約狀態改成預約中
                    foreach (var item3 in q3)
                    {
                        item3.book_state = "預約中";
                    }
                    //寫回資料庫
                    db.SaveChanges();
                }
            }
            catch (Exception ex) { return Content(ex.ToString()); } //拋出錯誤碼
            //查詢員工與預約健檢表
            var query = from o in db.VW_BookEmp
                        where o.book_Id == bookid
                        select o;
            //action給0初始化
            action = 0;
            //把資料放入ViewModel_BookAction
            data.GetW_BookEmp = query.ToList();
            data.GetAction = action;
            return View(data);
        }
        public ActionResult EditBook()
        {
            //宣告一個變數bookId
            string bookId = Request["book_Id"];
            //查詢員工與預約編號表
            var query = from o in db.VW_BookEmp
                        where o.book_Id == bookId
                        select o;
            //查詢醫院與方案檢視表
            var q1 = from o in db.VW_HospitalProgram
                     where o.hospital_state == "N" && o.program_state == "N" && o.work_name == "高階主管"
                     select o;
            //創一個新的ViewModel_BookHos表
            ViewModel_BookHos data = new ViewModel_BookHos();
            //把員工與預約健檢檢視表 跟 醫院方案表放入
            data.GetW_BookEmp = query.ToList();
            data.GetW_HosProgram = q1.ToList();
            return View(data);
        }
        [HttpPost]
        public ActionResult EditBook(Book data)
        {
            //宣告一個醫院與方案的ID
            int hospitalProgramId = Convert.ToInt32(Request["hospitalProgram"]);
            //宣告一個員工手機
            string employee_cellPhone = Request["employee_cellphone"];
            //宣告一個郵寄地址
            string employee_mailingAddress = Request["employee_mailingAddress"];
            //宣告一個員工編號
            string employee_workNumber = Session["employee_workNumber"].ToString();
            string book_EatAspririn = Request["book_EatAspririn"];
            //宣告一個醫院id
            int hospitalId = 0;
            //宣告一個方案id
            int programId = 0;
            //宣告一個差異年分
            int diffYear = 0;
            //宣告一個補助金額
            int money = 0;
            //初始化
            //DateTime hostTime;
            //if (Session["employee_role"].ToString() == "Y") {
            //    hostTime = DateTime.Now;
            //}
            //引用emp方法
            Emp emp = new Emp();
            //查詢醫院方案檢視表
            var q1 = from o in db.VW_HospitalProgram
                     where o.program_programId == hospitalProgramId
                     select o;
            //取出醫院編號跟方案編號
            foreach (var hos in q1)
            {
                hospitalId = hos.hospital_hospitalId;
                programId = hos.program_programId;
            }
            //判斷差異年分(今年與上次健檢年度)
            if (data.book_healthDate.HasValue)
            {
                diffYear = new DateTime((DateTime.Now - Convert.ToDateTime(data.book_healthDate)).Ticks).Year;
            }
            else
            {
                diffYear = 4;
            }
            //判斷補助金額
            switch (diffYear)
            {
                //差異年度為0補助金額：0
                case 0:
                    money = 0;
                    break;
                //差異年度為1補助金額：500
                case 1:
                    money = 500;
                    break;
                //差異年度為2補助金額：10000
                case 2:
                    money = 1000;
                    break;
                case 3:
                    //差異年度為3補助金額：1500
                    money = 1500;
                    break;
                //差異年度為其他補助金額：1500
                default:
                    money = 1500;
                    break;
            }
            //動作方法
            int action = Convert.ToInt32(Request["action"]);
            //if (action == 1) {
            //查詢book表
            var q = from o in db.Book
                    where o.book_id == data.book_id
                    select o;
            //判斷是否為系統管理員
            if (Session["employee_role"].ToString() == "Y")
            {
                //取出值並修改值
                foreach (var item in q)
                {
                    item.book_year = data.book_year;
                    item.book_state = "預約完成";
                    item.book_hospitalId = hospitalId;
                    item.book_ProgramId = programId;
                    item.book_cellphone = employee_cellPhone;
                    item.book_Notice = data.book_Notice;
                    item.book_special = data.book_special;
                    item.book_note = data.book_note;
                    item.book_exceptDate1 = data.book_exceptDate1;
                    item.book_exceptDate2 = data.book_exceptDate2;
                    item.book_exceptDate3 = data.book_exceptDate3;
                    item.book_Date = DateTime.Now;
                    item.book_updateUser = emp.Name(employee_workNumber);
                    item.book_updateDate = DateTime.Now;
                    item.book_healthDate = data.book_healthDate;
                    item.book_welfare = money;
                    item.book_hostTime = DateTime.Now;
                    item.book_host = emp.Name(employee_workNumber);
                    item.book_costDate = Convert.ToDateTime(data.book_costDate);
                    item.book_cost = data.book_cost;
                    item.book_EatAspririn = book_EatAspririn;
                    item.book_medicalHistory = data.book_medicalHistory;
                    item.book_IsAllergy = data.book_IsAllergy;
                    item.book_IsDisable = "N";
                }
                db.SaveChanges();
                return RedirectToAction("welcomePage", "member");
            }
            else
            {
                //其餘人取出值並改動
                foreach (var item in q)
                {
                    item.book_year = data.book_year;
                    item.book_state = "處理中";
                    item.book_hospitalId = hospitalId;
                    item.book_ProgramId = programId;
                    item.book_cellphone = employee_cellPhone;
                    item.book_Notice = data.book_Notice;
                    item.book_special = data.book_special;
                    item.book_note = null;
                    item.book_exceptDate1 = data.book_exceptDate1;
                    item.book_exceptDate2 = data.book_exceptDate2;
                    item.book_exceptDate3 = data.book_exceptDate3;
                    item.book_Date = DateTime.Now;
                    item.book_updateUser = emp.Name(employee_workNumber);
                    item.book_updateDate = DateTime.Now;
                    item.book_healthDate = data.book_healthDate;
                    item.book_welfare = money;
                    item.book_costDate = null;
                    item.book_cost = null;
                    item.book_EatAspririn = book_EatAspririn;
                    item.book_medicalHistory = data.book_medicalHistory;
                    item.book_IsAllergy = data.book_IsAllergy;
                    item.book_IsDisable = "N";
                }
            }
            //寫入資料庫
            db.SaveChanges();
            //新增一個result (ViewModel_BookHos)
            ViewModel_BookHos result = new ViewModel_BookHos();
            //查詢Book與Eep檢視表
            var query = from o in db.VW_BookEmp
                        where o.book_Id == data.book_id
                        select o;
            //查詢醫院與方案檢視表
            var query1 = from o in db.VW_HospitalProgram
                         where o.hospital_state == "N" && o.program_state == "N" && o.work_name == "高階主管"
                         select o;
            //把Book與Eep檢視表放入ViewModel_BookHos
            result.GetW_BookEmp = query.ToList();
            //把HosProgram表放入ViewModel_BookHos
            result.GetW_HosProgram = query1.ToList();
            //輸出結果
            return View(result);
            //}
            //return View(data);
        }

        public ActionResult newBook(FormCollection frm)
        {
            //宣告健檢年度變數
            int bookyear = DateTime.Now.Year;
            //宣告員工編號變數
            string employee_workNumber = Session["employee_workNumber"].ToString();
            //宣告廠別ID變數
            string factoryId = "";
            //引用VWModel_BookEmp
            VWModel_BookEmp data = new VWModel_BookEmp();
            //查詢員工資料
            var q1 = from o in db.Employee
                     where o.employee_workNumber == employee_workNumber
                     select o;
            //取出員工所屬廠別

            foreach (var fa in q1)
            {
                factoryId = fa.employee_factoryId;
            }
            //查詢適用高階主管的健檢方案與醫院名稱
            var q2 = from o in db.VW_HospitalProgram
                     where o.work_name == "高階主管"
                     select o;
            //查詢流水號
            int? bookSerial = (from o in db.Book
                               where o.book_year == DateTime.Now.Year
                               orderby o.book_id descending
                               select o.book_serialNumber).FirstOrDefault();
            //假如查無資料給出流水號1
            if (bookSerial == 0)
            {
                bookSerial = 1;
                //其餘其況序號+1
            }
            else
            {
                bookSerial += 1;
            }
            //把bookSerial改成字串
            string bookserialNumber = bookSerial.ToString();
            //把健檢編號 加工成RP+年度+流水號從左邊補到共5碼+廠別編號補成4碼
            string bookid = "RP" + (DateTime.Now.Year).ToString() + bookserialNumber.PadLeft(5, '0') + factoryId.PadLeft(4, '0');
            //宣告一個新的Book資料行
            Book x = new Book()
            {
                book_id = bookid,
                book_serialNumber = bookSerial,
                book_year = bookyear,
                book_state = "處理中"
            };
            //宣告一個List<Book>
            List<Book> book = new List<Book>();
            //把 book資料行加入List
            book.Add(x);
            //把List (預約健檢新行&員工資料&醫院方案)
            data.BookRow = book;
            data.EmpRow = q1.ToList();
            data.HosProg = q2.ToList();
            return View(data);
        }
        [HttpPost]
        public ActionResult newBook(Book data)
        {
            //宣告一個序號變數
            int serialNumber = Convert.ToInt32(Request["book_serialNumber"]);
            string book_EatAspririn = Request["book_EatAspririn"];
            //宣告一個醫院與方案的ID
            int hospitalProgramId = Convert.ToInt32(Request["hospitalProgram"]);
            //宣告一個員工手機
            string employee_cellPhone = Request["employee_cellphone"];
            //宣告一個郵寄地址
            string employee_mailingAddress = Request["employee_mailingAddress"];
            //宣告一個員工編號
            string employee_workNumber = Session["employee_workNumber"].ToString();
            //宣告一個醫院id
            int hospitalId = 0;
            //宣告一個方案id
            int programId = 0;
            //宣告一個差異年分
            int diffYear = 0;
            //宣告一個補助金額
            int money = 0;
            //初始化
            //DateTime hostTime;
            //if (Session["employee_role"].ToString() == "Y") {
            //    hostTime = DateTime.Now;
            //}
            //引用emp方法
            Emp emp = new Emp();
            //查詢醫院方案檢視表
            var q1 = from o in db.VW_HospitalProgram
                     where o.program_programId == hospitalProgramId
                     select o;
            //取出醫院編號跟方案編號
            foreach (var hos in q1)
            {
                hospitalId = hos.hospital_hospitalId;
                programId = hos.program_programId;
            }
            //判斷差異年分(今年與上次健檢年度)
            if (data.book_healthDate.HasValue)
            {
                diffYear = new DateTime((DateTime.Now - Convert.ToDateTime(data.book_healthDate)).Ticks).Year;
            }
            else
            {
                diffYear = 4;
            }
            //判斷補助金額
            switch (diffYear)
            {
                //差異年度為0補助金額：0
                case 0:
                    money = 0;
                    break;
                //差異年度為1補助金額：500
                case 1:
                    money = 500;
                    break;
                //差異年度為2補助金額：1000
                case 2:
                    money = 1000;
                    break;
                //差異年度為3補助金額：1500
                case 3:
                    money = 1500;
                    break;
                //其他差異年度補助金額：1500
                default:
                    money = 1500;
                    break;
            }
            //查詢book表

            //其餘人取出值並改動
            data.book_state = "預約中";
            data.book_hospitalId = hospitalId;
            data.book_ProgramId = programId;
            data.book_workNumber = employee_workNumber;
            data.book_mobile = employee_cellPhone;
            data.book_cellphone = employee_cellPhone;
            data.book_welfare = money;
            data.book_updateUser = emp.Name(employee_workNumber);
            data.book_updateDate = DateTime.Now;
            data.book_Date = DateTime.Now;
            data.book_serialNumber = serialNumber;
            if (!string.IsNullOrEmpty(Request["book_special"].ToString()))
            {
                data.book_special = Request["book_special"].ToString();
            }
            data.book_EatAspririn = Request["book_EatAspririn"];
            data.book_IsDisable = "N";
            data.book_noticeaddress = Request["book_noticeaddress"];
            //寫入資料庫
            try
            {
                db.Book.Add(data);
                db.SaveChanges();
            }
            catch (Exception ex) { return Content(ex.ToString()); }
            return RedirectToAction("welcomePage", "member");
        }
        //----------------------------------------------------------
        public ActionResult reportManageMaster() //健檢報告主頁
        {

            List<ReportManage> data = new List<ReportManage>();

            return View(data);
        }
        [HttpPost]
        public ActionResult reportManageMaster(ReportManage reportData) //健檢報告主頁
        {
            if (string.IsNullOrEmpty(reportData.ReportManage_IsDisable)) {

                reportData.ReportManage_IsDisable = "";
            }
            if (string.IsNullOrEmpty(reportData.ReportManage_workNumber))
            {
                reportData.ReportManage_workNumber = "";
            }
           
            var query = (from o in db.ReportManage
                         where o.ReportManage_workNumber.Contains(reportData.ReportManage_workNumber) && o.ReportManage_IsDisable.Contains(o.ReportManage_IsDisable)
                         select o);



            List<ReportManage> data = query.ToList();

            return View(data);
        }
        public ActionResult reportManageMain()
        {

            string reportManageId = Session["reportID"].ToString();
            var query = from o in db.vReportCheck
                        where o.ReportManage_id == reportManageId
                        select o;
            healthSystemReportMange data = new healthSystemReportMange();
            data.Report = query.ToList();
            return View(data);

        }
        [HttpPost]
        public ActionResult reportManageMain(vReportCheck reportData) //健檢報告主檔
        {
            Session["reportID"] = reportData.ReportManage_id;
            var query = from o in db.vReportCheck
                        where o.ReportManage_id == reportData.ReportManage_id
                        select o;

            healthSystemReportMange data = new healthSystemReportMange();
            data.Report = query.ToList();
            return View(data);
        }



        public ActionResult editReport() //編輯健檢報告主檔
        {
            string manageId = Session["reportID"].ToString();
            var query = from o in db.ReportManage
                        where o.ReportManage_id == manageId
                        select o;

            List<ReportManage> data = query.ToList();

            return View(data);

        }
        [HttpPost]
        public ActionResult editReport(string ReportManage_medicalRecord) //編輯健檢報告主檔
        {
            string notes = Request["ReportManage_notes"].ToString();
            string isDisable = Request["ReportManage_IsDisable"].ToString();
            ReportManage data = db.ReportManage.Find(Session["reportID"]);
            data.ReportManage_medicalRecord = ReportManage_medicalRecord;
            data.ReportManage_IsDisable = isDisable;
            data.ReportManage_notes = notes;


            db.SaveChanges();
            return RedirectToAction("reportManageMain", "healthSystem");


        }
        public ActionResult excelImport() // 批次匯入
        {

            //var query1 = from o in db.ReportCheckItem
            //             orderby o.ReportCheckItem_reportId descending
            //             select o.ReportCheckItem_reportId;
            //int reportId = query1.First();


            //var query2 = from o in db.ReportCheckItem
            //             where o.ReportCheckItem_reportId == reportId
            //             select o;

            //List<ReportCheckItem> newData = query2.ToList();

            return View();
        }
        [HttpPost]
        public ActionResult excelImport(FormCollection frm, ReportCheckItem report) // 批次匯入
        {
            HttpPostedFileBase postedFile = Request.Files["postedFile"];
            string workNumber = Request["ReportCheckItem_employee_workNumber"];


            Dictionary<string, object> jo = new Dictionary<string, object>();
            string filePath = string.Empty;
            if (postedFile != null)
            {
                string path = Server.MapPath("~/Uploads/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                filePath = path + Path.GetFileName(postedFile.FileName);
                string extension = Path.GetExtension(postedFile.FileName);
                postedFile.SaveAs(filePath);

                string conString = string.Empty;
                switch (extension)
                {
                    case ".xls": //Excel 97-03.
                        conString = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                        break;
                    case ".xlsx": //Excel 07 and above.
                        conString = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                        break;
                }

                DataTable dt = new DataTable();
                conString = string.Format(conString, filePath);

                using (OleDbConnection connExcel = new OleDbConnection(conString))
                {
                    using (OleDbCommand cmdExcel = new OleDbCommand())
                    {
                        using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                        {
                            cmdExcel.Connection = connExcel;

                            //Get the name of First Sheet.
                            connExcel.Open();
                            DataTable dtExcelSchema;
                            dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                            string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                            connExcel.Close();

                            //Read Data from First Sheet.
                            connExcel.Open();
                            cmdExcel.CommandText = "SELECT * From [" + sheetName + "]";
                            odaExcel.SelectCommand = cmdExcel;
                            odaExcel.Fill(dt);
                            connExcel.Close();
                        }
                    }
                }

                conString = ConfigurationManager.ConnectionStrings["Constring"].ConnectionString;
                using (SqlConnection con = new SqlConnection(conString))
                {
                    using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                    {
                        //Set the database table name.
                        sqlBulkCopy.DestinationTableName = "dbo.ReportCheckItem";

                        //[OPTIONAL]: Map the Excel columns with that of the database table

                        sqlBulkCopy.ColumnMappings.Add("ReportCheckItem_employee_workNumber", "ReportCheckItem_employee_workNumber");
                        sqlBulkCopy.ColumnMappings.Add("ReportCheckItem_checkDate", "ReportCheckItem_checkDate");
                        sqlBulkCopy.ColumnMappings.Add("ReportCheckItem_checkYear", "ReportCheckItem_checkYear");
                        sqlBulkCopy.ColumnMappings.Add("ReportCheckItem_generalComment_1", "ReportCheckItem_generalComment_1");
                        sqlBulkCopy.ColumnMappings.Add("ReportCheckItem_generalComment_2", "ReportCheckItem_generalComment_2");
                        sqlBulkCopy.ColumnMappings.Add("ReportCheckItem_generalComment_3", "ReportCheckItem_generalComment_3");
                        sqlBulkCopy.ColumnMappings.Add("ReportCheckItem_generalComment_4", "ReportCheckItem_generalComment_4");
                        sqlBulkCopy.ColumnMappings.Add("ReportCheckItem_generalComment_5", "ReportCheckItem_generalComment_5");
                        sqlBulkCopy.ColumnMappings.Add("ReportCheckItem_generalComment_6", "ReportCheckItem_generalComment_6");
                        sqlBulkCopy.ColumnMappings.Add("ReportCheckItem_generalComment_7", "ReportCheckItem_generalComment_7");
                        sqlBulkCopy.ColumnMappings.Add("ReportCheckItem_generalComment_8", "ReportCheckItem_generalComment_8");
                        sqlBulkCopy.ColumnMappings.Add("ReportCheckItem_generalComment_9", "ReportCheckItem_generalComment_9");
                        sqlBulkCopy.ColumnMappings.Add("ReportCheckItem_generalComment_10", "ReportCheckItem_generalComment_10");
                        sqlBulkCopy.ColumnMappings.Add("ReportCheckItem_generalComment_11", "ReportCheckItem_generalComment_11");
                        sqlBulkCopy.ColumnMappings.Add("ReportCheckItem_generalComment_12", "ReportCheckItem_generalComment_12");
                        sqlBulkCopy.ColumnMappings.Add("ReportCheckItem_generalComment_13", "ReportCheckItem_generalComment_13");
                        sqlBulkCopy.ColumnMappings.Add("ReportCheckItem_generalComment_14", "ReportCheckItem_generalComment_14");
                        sqlBulkCopy.ColumnMappings.Add("ReportCheckItem_generalComment_15", "ReportCheckItem_generalComment_15");
                        sqlBulkCopy.ColumnMappings.Add("ReportCheckItem_generalComment_16", "ReportCheckItem_generalComment_16");
                        sqlBulkCopy.ColumnMappings.Add("ReportCheckItem_generalComment_17", "ReportCheckItem_generalComment_17");
                        sqlBulkCopy.ColumnMappings.Add("ReportCheckItem_hw", "ReportCheckItem_hw");
                        sqlBulkCopy.ColumnMappings.Add("ReportCheckItem_pastHistory", "ReportCheckItem_pastHistory");
                        sqlBulkCopy.ColumnMappings.Add("ReportCheckItem_symptoms", "ReportCheckItem_symptoms");
                        sqlBulkCopy.ColumnMappings.Add("ReportCheckItem_HeightValue", "ReportCheckItem_HeightValue");
                        sqlBulkCopy.ColumnMappings.Add("ReportCheckItem_HVIsPass", "ReportCheckItem_HVIsPass");
                        sqlBulkCopy.ColumnMappings.Add("ReportCheckItem_WeightValue", "ReportCheckItem_WeightValue");
                        sqlBulkCopy.ColumnMappings.Add("ReportCheckItem_WVIsPass", "ReportCheckItem_WVIsPass");
                        sqlBulkCopy.ColumnMappings.Add("ReportCheckItem_WaistValue", "ReportCheckItem_WaistValue");
                        sqlBulkCopy.ColumnMappings.Add("ReportCheckItem_WaistIsPass", "ReportCheckItem_WaistIsPass");







                        con.Open();
                        sqlBulkCopy.WriteToServer(dt);

                        con.Close();

                        TempData["uploadExcelMessage"] = "上傳成功";

                    }
                }
            }
            else
            {

                jo.Add("success", false);
                jo.Add("message", "file upload error.");
            }

            var query1 = from o in db.ReportCheckItem
                         orderby o.ReportCheckItem_reportId descending
                         select o.ReportCheckItem_reportId;
            int reportId = query1.First();
            Session["reportId"] = reportId.ToString();
            var query2 = from o in db.ReportCheckItem
                         where o.ReportCheckItem_reportId == reportId
                         select o;
            var query3 = from o in db.ReportCheckItem
                         where o.ReportCheckItem_reportId == reportId
                         select o.ReportCheckItem_employee_workNumber;
            Session["excel_workNumber"] = query3.First();

            healthSystemReportMange data = new healthSystemReportMange();

            List<ReportCheckItem> newData = query2.ToList();

            data.reportCheckItem = newData;

            return View(data);






        }

        [HttpPost]
        public ActionResult ReportImport(ReportManage report)
        {

            string updateUserWorkNumber = Session["employee_workNumber"].ToString();
            int? hosId = report.ReportManage_hospitalId;
            int programId = Convert.ToInt32(Request["ReportManage_programId"].ToString());
            int startId = Convert.ToInt32(Request["ReportManage_startId"].ToString());

            string isDisable = Request["ReportManage_IsDisable"].ToString();
            string medicalNumber = Request["ReportManage_medicalRecord"].ToString();
            int reportId = Convert.ToInt32(Session["reportId"].ToString());
            //產生流水號
            var query1 = from o in db.ReportManage
                         where o.ReportManage_startId == startId
                         orderby o.ReportManage_serialNumber descending
                         select o.ReportManage_serialNumber;
            int serialNumber = query1.FirstOrDefault() + 1;
            string stringSerialNumber = serialNumber.ToString();

            //更新者
            var query2 = from o in db.Employee
                         where o.employee_workNumber == updateUserWorkNumber
                         select o.employee_name;
            string updateUserName = query2.FirstOrDefault();
            //廠別
            var query3 = from o in db.Employee
                         where o.employee_workNumber == updateUserWorkNumber
                         select o.employee_factoryId;
            string factoryId = query3.FirstOrDefault();
            //年度
            var query4 = from o in db.StartCheck
                         where o.Start_id == startId
                         select o.start_year;

            int? yearCheck = query4.FirstOrDefault();
            try
            {
                ReportManage newReport = new ReportManage()
                {
                    report_id = reportId,
                    ReportManage_startId = startId,
                    ReportManage_programId = programId,
                    ReportManage_hospitalId = hosId,
                    ReportManage_workNumber = Session["excel_workNumber"].ToString(),
                    ReportManage_id = "H" + yearCheck.ToString() + stringSerialNumber.PadLeft(5, '0') + factoryId.PadLeft(4, '0'),
                    ReportManage_serialNumber = serialNumber,
                    ReportManage_medicalRecord= medicalNumber,
                    ReportManage_IsDisable = isDisable,
                    ReportManage_updateUser = updateUserName,
                    ReportManage_updateTime = DateTime.Now
                };
                //reportcheck加入新資料行
                db.ReportManage.Add(newReport);
                //寫回資料庫
                db.SaveChanges();
            }
            catch (Exception e) { return Content(e.ToString()); }

            //report = newReport;

            return RedirectToAction("excelImport");
        }
        //刪除健檢報告==>Y
        public ActionResult delreport()
        {
            string ReportManage_id = Session["reportID"].ToString();
            ReportManage data = new ReportManage();
            data = db.ReportManage.Find(ReportManage_id);
            data.ReportManage_IsDisable = "Y";
            db.SaveChanges();
            return RedirectToAction("reportManageMain", "healthSystem");
        }
    }


}