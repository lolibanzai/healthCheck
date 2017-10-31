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
            var query = from o in db.StartCheck
                        select o;
            List<StartCheck> data = query.ToList();
            return View(data);
        }
        [HttpPost]
        public ActionResult StartCheckMaster(StartCheck data)
        {
            if (string.IsNullOrEmpty(data.start_state))
            {
                data.start_state = "";
            }
            var query = from o in db.StartCheck
                        where o.start_year == data.start_year && o.start_state.Contains(data.start_state)
                        select o;
            List<StartCheck> selectData = query.ToList();
            return View(selectData);
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
        [HttpPost]
        public ActionResult newStartCheck(StartCheck data, string noteText)//啟動健檢
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
        public ActionResult newStartCheck()
        {
            return View();
        }
        public ActionResult StartPlace() //啟動作業明細檔-受檢單位
        {
            return View();
        }
        public ActionResult StartFile() //啟動作業明細檔-附件管理
        {
            return View();
        }
        //----------------------------------------------------------
        public ActionResult newStart() //啟動作業-新增的畫面
        {
            return View();
        }
        //----------------------------------------------------------
        public ActionResult bookMaster() //預約健檢主頁
        {
            return View();
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
        public ActionResult checkItem() //健檢報告明細檔-健檢項目
        {
            return View();
        }
        public ActionResult doubleCheck() //健檢報告明細檔-複檢項目
        {
            return View();
        }
        public ActionResult reportFile() //健檢報告明細檔-附件管理
        {
            return View();
        }
        public ActionResult excelImport() {

            return View();
        }

    }
}