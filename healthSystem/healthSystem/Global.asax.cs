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
            Session["message"] = "";
            Session["employee_workNumber"] = "";
            Session["employee_role"] = "";
        }
    }
}
