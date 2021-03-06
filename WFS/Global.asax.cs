﻿using System;
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
