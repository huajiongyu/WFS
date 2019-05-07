using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WFS.Models;

namespace WFS.Helpers
{
    public class SettingHelper
    {
        public static decimal MaxCost()
        {
            using(WFSContext db = new WFSContext())
            {
                var setting = db.Settings.FirstOrDefault();
                return setting.MaxCost;
            }
        }

        public static decimal CountOfAll()
        {
            using (WFSContext db = new WFSContext())
            {
                var setting = db.Settings.FirstOrDefault();
                return setting.CountOfAll;
            }
        }

        public static void SetCountOfAll(decimal sum)
        {
            using (WFSContext db = new WFSContext())
            {
                var setting = db.Settings.FirstOrDefault();

                setting.CountOfAll = sum;

                db.SaveChanges();
            }
        }
    }
}