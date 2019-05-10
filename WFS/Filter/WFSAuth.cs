using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace WFS.Filter
{
    /// <summary>
    /// 角色权限筛选器
    /// 继承自ASP.NET MVC 四大过滤器之AuthorizeAttribute
    /// </summary>
    public class WFSAuth : AuthorizeAttribute
    {
        /// <summary>
        /// 实现判断权限的方法
        /// 如果返回false表示没有权限，如果返回true表示有权限
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var Request = httpContext.Request;
            var cookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            var ticket = FormsAuthentication.Decrypt(cookie.Value);
            string role = ticket.UserData;
            if (string.IsNullOrWhiteSpace(Roles))
            {
                return false;
            }

            var List_Roles = Roles.Split(',');
            foreach(string r in List_Roles)
            {
                if(r.Trim().Equals(role.Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }
    }
}