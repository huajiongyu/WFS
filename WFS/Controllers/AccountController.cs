using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WFS.Models;

namespace WFS.Controllers
{
    public class AccountController : Controller
    {

        /// <summary>
        /// 显示登录页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// 登录(处理提交)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pwd"></param>
        /// <param name="RememberMe"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Login(string id, string pwd, bool RememberMe = false)
        {
            using (var db = new WFSContext())
            {
                //从数据库查找用户
                var user = db.Users.FirstOrDefault(x=>x.ID.Trim().Equals(id.Trim(), StringComparison.OrdinalIgnoreCase)
                && x.Password.Trim().Equals(pwd.Trim(), StringComparison.OrdinalIgnoreCase));

                //如果没找到到用户
                if(user == null)
                {
                    return View();
                }

                string userRoles = user.Role.ToString(); //调用UserToRole方法来获取role字符串 
                FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(1, user.ID.Trim(), DateTime.Now, DateTime.Now.AddMinutes(30), RememberMe, userRoles, "/"); //建立身份验证票对象 
                string HashTicket = FormsAuthentication.Encrypt(Ticket); //加密序列化验证票为字符串 
                HttpCookie UserCookie = new HttpCookie(FormsAuthentication.FormsCookieName, HashTicket);
                //生成Cookie 
                Response.Cookies.Add(UserCookie); //输出Cookie 
                //Response.Redirect(Request["ReturnUrl"]); // 重定向到用户申请的初始页面 
                
                Session["UserName"] = user.Name.Trim();

                //根据不同角色，跳转到不同的页面
                switch (user.Role)
                {
                    case RoleType.Assessor:
                        return RedirectToAction("Index", "Assessor");
                        break;
                    case RoleType.Finance:
                        return RedirectToAction("Index", "Treasury");
                        break;
                    default:
                        return RedirectToAction("Index", "Proposer");
                        break;
                }
                
            }
        }

        /// <summary>
        /// 注销登录
        /// </summary>
        /// <returns></returns>
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session["UserName"] = null;
            return RedirectToAction("Login");
        }
    }
}