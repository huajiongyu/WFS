using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WFS.Models
{
    public class SettingViewModel
    {

        /// <summary>
        /// 总帐余额
        /// </summary>
        [Range(0, float.MaxValue)]
        //[Required]
        public Decimal GeneralLedger { get; set; }

        /// <summary>
        /// 发送邮件的地址
        /// </summary>
        [Required]
        [EmailAddress]
        public string SendMail { get; set; }

        /// <summary>
        /// 发送邮箱的显示名称
        /// </summary>
        [Required]
        public string SendName { get; set; }

        /// <summary>
        /// 发送邮件的密码
        /// </summary>
        [Required]
        public string Password { get; set; }

        /// <summary>
        /// SMTP地址
        /// </summary>
        [Required]
        public string SMTP { get; set; }

        /// <summary>
        /// 邮件服务器是否启用SSL
        /// </summary>
        public bool SSL { get; set; }

        /// <summary>
        /// SMTP端口
        /// </summary>
        [Required]
        public int Port { get; set; }
    }
}