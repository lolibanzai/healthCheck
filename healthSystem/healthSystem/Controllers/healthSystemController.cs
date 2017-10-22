using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace healthSystem.Controllers
{
    public class healthSystemController : Controller
    {
        // GET: healthSystem
        public ActionResult StartCheckMaster() //啟動作業主頁
        {
            return View();
        }
        public ActionResult StartCheckMain() //啟動作業主主檔
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
    }
}