using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using WFS.Models;

namespace WFS.Controllers
{
    public class InitController : Controller
    {
        // GET: Init
        public ActionResult Index()
        {
            using (WFSContext db = new WFSContext())
            {
                //创建Uploads文件夹
                if (!Directory.Exists(Server.MapPath("~/Uploads")))
                {
                    Directory.CreateDirectory(Server.MapPath("~/Uploads"));
                }

                /*
                 * 如果是用户表为空
                 * 就创建一个管理员
                 */

                if (db.Users.Any())
                {
                    return RedirectToAction("Login", "account");
                }
                var user = new UserEntity()
                {
                    ID = "admin",
                    CreateDate = DateTime.Now,
                    Name = "财务",
                    Password = "123123",
                    Role = RoleType.Finance
                };

                var dept = new Deptment()
                {
                    Id = Guid.NewGuid(),
                    Name = "财务部"
                };

                db.Deptments.Add(dept);
                db.SaveChanges();

                dept.Supervisor = user;
                dept.Users = new List<UserEntity>()
                {
                    user
                };
                db.Users.Add(user);
                db.Entry<Deptment>(dept).State = System.Data.Entity.EntityState.Modified;

                db.Settings.RemoveRange(db.Settings.ToList());
                db.Settings.Add(new Settings()
                {
                    Id = "1",
                    CountOfAll = 20000000,
                    MaxCost = 5000
                });

                db.SaveChanges();


            }
            return RedirectToAction("Login", "account");
        }
    }
}