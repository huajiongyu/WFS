using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using WFS.Helpers;
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
                var form = db.Forms
                    .Include("ProcessLog")
                    .Where(x => x.ID.Trim() == id.Trim()).FirstOrDefault();
                if(form == null)
                {
                    return View();
                }
                model = (from f in db.Forms
                             join u in db.Users on f.CreateBy.Trim() equals u.ID.Trim()
                             select new FormPrintModel()
                             {
                                 ID = f.ID,
                                 Content = f.Content,
                                 Cost = f.Cost,
                                 CostChinese = "",
                                 Date = f.CreateTime,
                                 DeptName = u.Dept.Name.Trim(),
                                 //P1 = f.ProcessLog.Where(x=>x.ProcessCode == ProcessCode.L0)
                                 Title = f.Title,
                                 Type = f.Type == FormType.Budget ? "" : "",
                                 User = u.Name.Trim()
                             }).FirstOrDefault();
                var p1 = form.ProcessLog.FirstOrDefault(x => x.ProcessCode == ProcessCode.L10);
                if(p1 != null)
                {
                    var name = db.Users.Where(x => x.ID.Trim() == p1.CreateBy.Trim()).Select(x=>x.Name.Trim()).FirstOrDefault();
                    model.P1 = string.Format("{0}<br/>{1}<br/>{2}", name, p1.CreateDate.ToString("yyyy/MM/dd"), FormStrategy.StatusString(p1.status));
                }

                var p2 = form.ProcessLog.FirstOrDefault(x => x.ProcessCode == ProcessCode.L20);
                if (p2 != null)
                {
                    var name = db.Users.Where(x => x.ID.Trim() == p2.CreateBy.Trim()).Select(x => x.Name.Trim()).FirstOrDefault();
                    model.P2 = string.Format("{0}<br/>{1}<br/>{2}", name, p2.CreateDate.ToString("yyyy/MM/dd"), FormStrategy.StatusString(p2.status));
                }

                var p3 = form.ProcessLog.FirstOrDefault(x => x.ProcessCode == ProcessCode.L30);
                if (p3 != null)
                {
                    var name = db.Users.Where(x => x.ID.Trim() == p3.CreateBy.Trim()).Select(x => x.Name.Trim()).FirstOrDefault();
                    model.P3 = string.Format("{0}<br/>{1}<br/>{2}", name, p3.CreateDate.ToString("yyyy/MM/dd"), FormStrategy.StatusString(p3.status));
                }

               // var p4 = form.ProcessLog.FirstOrDefault(x => x.ProcessCode == ProcessCode.L40);
                if (form.FinDate != null)
                {
                    var name = db.Users.Where(x => x.ID.Trim() == form.FinBy.Trim()).Select(x => x.Name.Trim()).FirstOrDefault();
                    model.P4 = string.Format("{0}<br/>{1}<br/>{2}", name, ((DateTime)form.FinDate).ToString("yyyy/MM/dd"), "转帐");
                }

                model.CostChinese = ConvertToChinese(model.Cost);
            }
            return View(model);
        }

        public static String ConvertToChinese(Decimal number)
        {
            var s = number.ToString("#L#E#D#C#K#E#D#C#J#E#D#C#I#E#D#C#H#E#D#C#G#E#D#C#F#E#D#C#.0B0A");
            var d = Regex.Replace(s, @"((?<=-|^)[^1-9]*)|((?'z'0)[0A-E]*((?=[1-9])|(?'-z'(?=[F-L\.]|$))))|((?'b'[F-L])(?'z'0)[0A-L]*((?=[1-9])|(?'-z'(?=[\.]|$))))", "${b}${z}");
            var r = Regex.Replace(d, ".", m => "负元空零壹贰叁肆伍陆柒捌玖空空空空空空空分角拾佰仟万亿兆京垓秭穰"[m.Value[0] - '-'].ToString());
            return r;
        }
    }
}