using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace healthSystem.Controllers
{
    public class memberController : Controller
    {
        // GET: member
        public ActionResult login()
        {
            return View();
        }
        public ActionResult welcomePage()
        {
            return View();
        }
    }
}