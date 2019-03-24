using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using WFS.Models;

namespace WFS.Helpers
{
    public class MailHelpers
    {
        public static Boolean SendMail(string MailTo, string Subject, string Content)
        {
            MailSettingModel Setting = MetaValueHelper.GetMailSetting();
            MailMessage message = new MailMessage();
            //设置发件人,发件人需要与设置的邮件发送服务器的邮箱一致
            MailAddress fromAddr = new MailAddress(Setting.SendMail, Setting.SendName);
            message.From = fromAddr;

            //设置收件人,可添加多个,添加方法与下面的一样
            message.To.Add(MailTo);

            //设置邮件标题
            message.Subject = Subject;

            //设置邮件内容
            message.Body = Content;

            //设置邮件发送服务器,服务器根据你使用的邮箱而不同,可以到相应的 邮箱管理后台查看,下面是QQ的
            SmtpClient client = new SmtpClient(Setting.SMTP, Setting.Port)
                ;
            //设置发送人的邮箱账号和密码，POP3/SMTP服务要开启, 密码要是POP3/SMTP等服务的授权码
            client.Credentials = new System.Net.NetworkCredential(Setting.SendMail, Setting.Password);//vtirsfsthwuadjfe  fhszmpegwoqnecja

            //启用ssl,也就是安全发送
            client.EnableSsl = Setting.SSL;

            //发送邮件
            client.Send(message);
            return true;
        }
    }

    //本文使用的是qq邮件，发送邮箱记得要把POP3/SMTP服务开启, 密码要是POP3/SMTP等服务的授权码

}