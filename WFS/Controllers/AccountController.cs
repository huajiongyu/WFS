using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WFS.Helpers;
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
        public ActionResult Login(LoginModel model)
        {
            using (var db = new WFSContext())
            {
                //从数据库查找用户
                var user = db.Users.FirstOrDefault(x=>x.ID.Trim().Equals(model.id.Trim(), StringComparison.OrdinalIgnoreCase)
                && x.Password.Trim().Equals(model.pwd.Trim(), StringComparison.OrdinalIgnoreCase));

                //如果没找到到用户
                if(user == null)
                {
                    ViewBag.id = model.id;
                    ViewBag.Msg = "密码不正确。";
                    return View();
                }else if (user.Disabled == true)
                {
                    ViewBag.id = model.id;
                    ViewBag.Msg = "此帐号已被停用。请与财务或管理员联系。";
                    return View();
                }

                string userRoles = user.Role.ToString(); 
                FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(1, user.ID.Trim(), DateTime.Now, DateTime.Now.AddMinutes(30), model.RememberMe, userRoles, "/"); //建立身份验证票对象 
                string HashTicket = FormsAuthentication.Encrypt(Ticket); //加密序列化验证票为字符串 
                HttpCookie UserCookie = new HttpCookie(FormsAuthentication.FormsCookieName, HashTicket);
                //生成Cookie 
                Response.Cookies.Add(UserCookie); //输出Cookie 
                //Response.Redirect(Request["ReturnUrl"]); // 重定向到用户申请的初始页面 
                
                Session["UserName"] = user.Name.Trim();

                //根据不同角色，跳转到不同的页面
                switch (user.Role)
                {
                    case RoleType.Assessor://审核人员
                    case RoleType.Hearmaster://校长
                    case RoleType.Supervisor://部门主任
                        return RedirectToAction("Index", "Assessor");
                    case RoleType.Finance://财务
                        return RedirectToAction("Appling", "Treasury");
                    default://其他:普通用户
                        return RedirectToAction("Index", "Proposer");
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

    public class LoginModel
    {
        [Required]
        public string id { get; set; }
        public string pwd{ get; set; }
        public bool RememberMe { get; set; } = false;
    }
}