using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;

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

        public string MyProperty { get; set; }
    }
}