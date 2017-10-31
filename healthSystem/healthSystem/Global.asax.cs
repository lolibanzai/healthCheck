using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace healthSystem
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
protected void Session_Start(object sender, EventArgs e) {
            //裝錯誤訊息(登入)
            Session["message"] = "";
            //裝登入員工編號
            Session["employee_workNumber"] = "";
            //裝登入員工角色
            Session["employee_role"] = "";
            //裝權限修改者員工編號
            Session["employee_acc"] = "";
        }
    }
}
