using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WFS.Models
{
    public class MailSettingModel
    {
        public string SendMail { get; set; }
        public string SendName { get; set; }
        public string Password { get; set; }
        public string SMTP { get; set; }
        public bool SSL { get; set; }
        public int Port { get; set; }
    }
}