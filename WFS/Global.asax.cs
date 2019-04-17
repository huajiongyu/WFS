using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using WFS.Models;
using System.IO;

namespace WFS
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //创建Uploads文件夹
            if (!Directory.Exists(Server.MapPath("~/Uploads")))
            {
                Directory.CreateDirectory(Server.MapPath("~/Uploads"));
            }

            /*
             * 如果是用户表为空
             * 就创建一个管理员
             */
            using(var db = new WFSContext())
            {
                if (!db.Users.Any())
                {
                    db.Users.Add(new UserEntity()
                    {
                        ID = "admin",
                        CreateDate = DateTime.Now,
                        Name = "财务",
                        Password = "123123",
                        Role = RoleType.Finance
                    });
                    db.SaveChanges();
                }
            }

            AutoMapper.Mapper.Initialize(x => {
                x.CreateMap<UserViewModel, UserEntity>();
                x.CreateMap<UserEntity, UserViewModel>();
                x.CreateMap<FormCreateModel, FormEntity>();
                x.CreateMap<FormEntity, FormCreateModel>();
                x.CreateMap<SettingViewModel, MailSettingModel>();
                x.CreateMap<MailSettingModel, SettingViewModel>();
            });


        }
    }
}
