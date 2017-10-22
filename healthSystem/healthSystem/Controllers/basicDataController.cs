using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace healthSystem.Controllers
{
    public class basicDataController : Controller
    {
        // GET: basicData
        public ActionResult purviewMaster() //人員權限主頁
        {
            return View();
        }
        public ActionResult purviewMain() //人員權限主檔
        {
            return View();
        }
        public ActionResult employeeFactory() //人員權限明細檔-廠別權限
        {
            return View();
        }
        //------------------------------------------------------------------------------
        public ActionResult employeeMaster() //人事資料主頁
        {
            return View();
        }
        public ActionResult employeeMain() //人事資料主檔
        {
            return View();
        }
        public ActionResult employeeWork() //人事資料明細檔-工種
        {
            return View();
        }
        public ActionResult employeeReport() //人事資料明細檔-健檢記錄
        {
            return View();
        }
        //------------------------------------------------------------------------------
        public ActionResult hospitalMaster() //人事資料主頁
        {
            return View();
        }
        public ActionResult hospitalMain() //人事資料主檔
        {
            return View();
        }
        public ActionResult hospitalFile() //人事資料明細檔-附件管理
        {
            return View();
        }
        public ActionResult hospitalProgram() //人事資料明細檔-健檢方案
        {
            return View();
        }
    }
}