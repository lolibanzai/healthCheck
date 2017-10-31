using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using healthSystem.Models;

namespace healthSystem.Controllers
{
    public class memberController : Controller
    {
        HealthCheckEntities1 db = new HealthCheckEntities1();
        // GET: member
        public ActionResult login() {
            Session["message"] = "";
            return View();
        }
        [HttpPost]
        public ActionResult login(FormCollection frm) {
            try {
                //宣告2個變數裝員工帳號密碼
                string userid = Request["employee_username"];
                string password = Request["employee_password"];
                //查詢員工表中的員工帳號密碼
                var query = from o in db.Employee
                            where o.employee_username == userid && o.employee_password == password
                            select o;
                var data = query.ToList();
                //假如沒找到輸出錯誤訊息
                if (data.Count() != 1) {
                    Session["message"] = "帳號或密碼錯誤登入失敗";
                    return View("login", frm);
                }
                //把登入的員工編號角色存入Session
                foreach (var item in data) {
                    Session["employee_workNumber"] = item.employee_workNumber;
                    Session["employee_role"] = item.employee_role;
                }
            } catch {
                return View("login", frm);
            }
            return RedirectToAction("welcomePage");
        }
        //歡迎頁
        public ActionResult welcomePage() {
            return View();
        }
        //登出
        public ActionResult logout() {
            //清空Session
            Session.Clear();
            // Session["employee_workNumber"] = "";
            return RedirectToAction("login");
        }
    }
}