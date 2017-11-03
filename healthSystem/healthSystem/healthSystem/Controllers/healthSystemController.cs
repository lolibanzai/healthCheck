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
                start_state = data.start_state
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

        public ActionResult checkCollect(int StartID , int startYear , int saveSignal) //收集作業
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
            for (int i = 0;i<= allWorkNumber.Count()-1;i++)
            {
                //檢查年齡確定級距
                int level = 0;
                if (allWorkNumberAge[i] < 40)
                {
                    level = 5; //40歲以下的級距為每5年一次
                }
                if (40 <= allWorkNumberAge[i] && allWorkNumberAge[i] <65)
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

                int lastcheckYear = query5.First(); //存入

                //最近健檢的年度(newcheckYear)加上他的級距(level)即為他應該健檢的年度
                //如果跟這次啟動作業的年度一樣的話表示他就是今年要去健檢
                if (lastcheckYear+level == startYear)
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
        public ActionResult stateChange(string collectMark, int StartID, int startYear)
        {
            if (collectMark == "進入健檢收集")
            {
                StartCheck data = db.StartCheck.Find(StartID);
                data.start_state = "健檢收集";
                data.start_startDate = DateTime.Now;
                db.SaveChanges();
            }
            if (collectMark == "進入結案")
            {
                StartCheck data = db.StartCheck.Find(StartID);
                data.start_state = "結案";
                data.start_CheckEndDate = DateTime.Now;
                db.SaveChanges();
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
                }
                //action=2，刪除資料表
                if (action == 2)
                {
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
            string employeeWorkNumber = Session["employee_workNumber"].ToString();

            ViewModel_BookEmp data = new ViewModel_BookEmp();
            var q = from o in db.VW_BookEmp
                    where o.employee_workNumber == employeeWorkNumber
                    select o;
            var q1 = from o in db.VW_EmpAuth
                     where o.employee_workNumber == employeeWorkNumber
                     select o;
            var q2 = from o in db.Employee
                     where o.employee_workNumber == employeeWorkNumber
                     select o;
            data.BookView = q.ToList();
            data.EmpView = q1.ToList();
            data.Emp = q2.ToList();
            return View(data);
        }
        [HttpPost]
        public ActionResult bookMaster(ViewModel_BookEmp data) //預約健檢主頁
        {
            string bookYear = Request["year"];
            DateTime start_date = new DateTime();
            DateTime end_date = new DateTime();
            string bookState = Request["book_state"];
            string factoryId = Request["employee_factoryId"];
            string employee_name = Request["employee_name"];
            string employeeWorkNumber = Request["employee_WorkNumber"];
            string book_IsDisable = Request["book_IsDisable"];
            if (string.IsNullOrEmpty(bookYear))
            {
                bookYear = "";
            }
            if (string.IsNullOrEmpty(Request["startbook_date"]))
            {

            }
            else
                start_date = Convert.ToDateTime(Request["startbook_date"]);
            if (string.IsNullOrEmpty(Request["endbook_date"]))
            {
                end_date = DateTime.Now;
            }
            else
                end_date = Convert.ToDateTime(Request["endbook_date"]);
            if (string.IsNullOrEmpty(bookState))
            {
                bookState = "";
            }
            if (string.IsNullOrEmpty(factoryId))
            {
                factoryId = "";
            }
            if (string.IsNullOrEmpty(employee_name))
            {
                employee_name = "";
            }
            if (string.IsNullOrEmpty(employeeWorkNumber))
            {
                employeeWorkNumber = "";
            }
            if (string.IsNullOrEmpty(book_IsDisable))
            {
                book_IsDisable = "";
            }
            var q = (from o in db.VW_BookEmp
                     where o.book_Id.Contains(bookYear)
                     && (o.book_Date >= start_date && o.book_Date <= end_date)
                     && o.employee_name.Contains(employee_name)
                     && o.employee_workNumber.Contains(employeeWorkNumber)
                     && o.book_state.Contains(bookState)
                     && o.book_IsDisable.Contains(book_IsDisable)
                     select o).ToList();
            var q1 = from o in db.VW_EmpAuth
                     where o.employee_workNumber == employeeWorkNumber
                     select o;
            var q2 = (from o in db.Employee
                      where o.employee_workNumber == employeeWorkNumber
                      select o).ToList();
            data.BookView = q;
            data.EmpView = q1;
            data.Emp = q2;
            return View(data);
        }
        public ActionResult bookMain() //預約健檢主檔
        {
            return View();
        }

        //----------------------------------------------------------
        public ActionResult reportManageMaster() //健檢報告主頁
        {
            return View();
        }
        public ActionResult reportManageMain() //健檢報告主檔
        {
            return View();
        }
        public ActionResult excelImport() // 批次匯入
        {

            return View();
        }

    }
}