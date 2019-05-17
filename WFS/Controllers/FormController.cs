using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WFS.Models;

namespace WFS.Controllers
{
    public class FormController : Controller
    {
        // GET: Form
        public ActionResult Print(string id)
        {
            FormPrintModel model;
            using (WFSContext db = new WFSContext())
            {

            }
            return View();
        }
    }

    
}