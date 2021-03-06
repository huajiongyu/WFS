﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WFS.Models;

namespace WFS.Helpers
{
    public class BankHelper
    {
        public static void InitialToDB()
        {
            using (var db = new WFSContext())
            {
                Bank[] BankRang =
                {
                    new Bank() {Id=2, Name="招商银行" },
                    new Bank() {Id=3, Name="建设银行" },
                    new Bank() {Id=4, Name="交通银行" },
                    new Bank() {Id=5, Name="邮储银行" },
                    new Bank() {Id=6, Name="工商银行" },
                    new Bank() {Id=7, Name="农业银行" },
                    new Bank() {Id=8, Name="中国银行" },
                    new Bank() {Id=9, Name="中信银行" },
                    new Bank() {Id=10, Name="光大银行" },
                    new Bank() {Id=11, Name="华夏银行" },
                    new Bank() {Id=12, Name="民生银行" },
                    new Bank() {Id=13, Name="广发银行" },
                    new Bank() {Id=14, Name="平安银行" },
                    new Bank() {Id=15, Name="星展银行" },
                    new Bank() {Id=16, Name="恒生银行" },
                    new Bank() {Id=17, Name="渣打银行" },
                    new Bank() {Id=18, Name="汇丰银行" },
                    new Bank() {Id=19, Name="东亚银行" },
                    new Bank() {Id=20, Name="花旗银行" },
                    new Bank() {Id=21, Name="浙商银行" },
                    new Bank() {Id=22, Name="恒丰银行" },
                    new Bank() {Id=23, Name="浦东发展银行" },
                    new Bank() {Id=24, Name="兴业银行" },
                    new Bank() {Id=26, Name="齐鲁银行" },
                    new Bank() {Id=27, Name="烟台银行" },
                    new Bank() {Id=28, Name="淮坊银行" },
                    new Bank() {Id=31, Name="渤海银行" },
                    new Bank() {Id=32, Name="上海银行" },
                    new Bank() {Id=33, Name="厦门银行" },
                    new Bank() {Id=34, Name="北京银行" },
                    new Bank() {Id=35, Name="福建海峡银行" },
                    new Bank() {Id=36, Name="吉林银行" },
                    new Bank() {Id=38, Name="宁波银行" },
                    new Bank() {Id=39, Name="焦作市商业银行" },
                    new Bank() {Id=40, Name="温州银行" },
                    new Bank() {Id=41, Name="广州银行" },
                    new Bank() {Id=42, Name="汉口银行" },
                    new Bank() {Id=43, Name="龙江银行" },
                    new Bank() {Id=44, Name="盛京银行" },
                    new Bank() {Id=45, Name="洛阳银行" },
                    new Bank() {Id=46, Name="辽阳银行" },
                    new Bank() {Id=47, Name="大连银行" },
                    new Bank() {Id=48, Name="苏州银行" },
                    new Bank() {Id=49, Name="河北银行" },
                    new Bank() {Id=50, Name="杭州银行" },
                    new Bank() {Id=51, Name="南京银行" },
                    new Bank() {Id=52, Name="东莞银行" },
                    new Bank() {Id=53, Name="金华银行" },
                    new Bank() {Id=54, Name="乌鲁木齐商业银行" },
                    new Bank() {Id=55, Name="绍兴银行" },
                    new Bank() {Id=56, Name="成都银行" },
                    new Bank() {Id=57, Name="抚顺银行" },
                    new Bank() {Id=58, Name="临商银行" },
                    new Bank() {Id=59, Name="宜昌市商业银行" },
                    new Bank() {Id=60, Name="葫芦岛银行" },
                    new Bank() {Id=61, Name="郑州银行" },
                    new Bank() {Id=62, Name="宁夏银行" },
                    new Bank() {Id=63, Name="珠海华润银行" },
                    new Bank() {Id=64, Name="齐商银行" },
                    new Bank() {Id=65, Name="锦州银行" },
                    new Bank() {Id=66, Name="徽商银行" },
                    new Bank() {Id=67, Name="重庆银行" },
                    new Bank() {Id=68, Name="哈尔滨银行" },
                    new Bank() {Id=69, Name="贵阳银行" },
                    new Bank() {Id=70, Name="西安银行" },
                    new Bank() {Id=71, Name="无锡市商业银行" },
                    new Bank() {Id=72, Name="丹东银行" },
                    new Bank() {Id=73, Name="兰州银行" },
                    new Bank() {Id=74, Name="南昌银行" },
                    new Bank() {Id=75, Name="晋商银行" },
                    new Bank() {Id=76, Name="青岛银行" },
                    new Bank() {Id=77, Name="南通商业银行" },
                    new Bank() {Id=78, Name="九江银行" },
                    new Bank() {Id=79, Name="日照银行" },
                    new Bank() {Id=80, Name="鞍山银行" },
                    new Bank() {Id=81, Name="秦皇岛银行" },
                    new Bank() {Id=82, Name="青海银行" },
                    new Bank() {Id=83, Name="台州银行" },
                    new Bank() {Id=84, Name="盐城银行" },
                    new Bank() {Id=85, Name="长沙银行" },
                    new Bank() {Id=86, Name="赣州银行" },
                    new Bank() {Id=87, Name="泉州银行" },
                    new Bank() {Id=88, Name="营口银行" },
                    new Bank() {Id=89, Name="富滇银行" },
                    new Bank() {Id=90, Name="阜新银行" },
                    new Bank() {Id=91, Name="嘉兴银行" },
                    new Bank() {Id=92, Name="廊坊银行" },
                    new Bank() {Id=93, Name="泰隆商业银行" },
                    new Bank() {Id=94, Name="内蒙古银行" },
                    new Bank() {Id=95, Name="湖州银行" },
                    new Bank() {Id=96, Name="沧州银行" },
                    new Bank() {Id=97, Name="广西北部湾银行" },
                    new Bank() {Id=98, Name="包商银行" },
                    new Bank() {Id=100, Name="威海商业银行" },
                    new Bank() {Id=101, Name="攀枝花市商业银行" },
                    new Bank() {Id=102, Name="绵阳市商业银行" },
                    new Bank() {Id=103, Name="泸州市商业银行" },
                    new Bank() {Id=104, Name="三门峡银行" },
                    new Bank() {Id=106, Name="邢台银行" },
                    new Bank() {Id=107, Name="商丘市商业银行" },
                    new Bank() {Id=108, Name="安徽省农村信用社" },
                    new Bank() {Id=109, Name="江西省农村信用社" },
                    new Bank() {Id=110, Name="湖南农村信用社" },
                    new Bank() {Id=111, Name="无锡农村商业银行" }
                };
                db.Banks.AddRange(BankRang);
                db.SaveChanges();
            }
        }
    }
}