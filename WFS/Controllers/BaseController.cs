using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using System.Web.Security;
using WFS.Models;

namespace WFS.Controllers
{
    public class BaseController : Controller
    {
        //
        // 摘要:
        //     在进行授权时调用。
        //
        // 参数:
        //   filterContext:
        //     有关当前请求和操作的信息。
        protected override void OnAuthentication(AuthenticationContext filterContext)
        {
            //HttpContext.User
            base.OnAuthentication(filterContext);
        }

        private UserEntity _User;

        public string Role {
            get {
                var cookie = Request.Cookies[FormsAuthentication.FormsCookieName];
                var ticket = FormsAuthentication.Decrypt(cookie.Value);
                string role = ticket.UserData;
                return role;
            } }

        public UserEntity LoginUser
        {
            get
            {
                using(WFSContext db = new WFSContext())
                {
                    if(_User != null)
                    {
                        return _User;
                    }
                    var cookie = Request.Cookies[FormsAuthentication.FormsCookieName];
                    var ticket = FormsAuthentication.Decrypt(cookie.Value);
                    _User = db.Users.Include("Dept").FirstOrDefault(x => x.ID == ticket.Name);//
                    return _User;
                }
            }
        }
    }
}