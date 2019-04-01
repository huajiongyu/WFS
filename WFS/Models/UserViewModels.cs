using System;
using System.ComponentModel.DataAnnotations;

namespace WFS.Models
{
    /// <summary>
    /// 关于帐号在前端的所有模型
    /// </summary>
    public class UserViewModel
    {
        [MaxLength(36)]
        [Required]
        public string ID { get; set; }

        [MaxLength(20)]
        [Required]
        public string Name { get; set; }

        [MaxLength(50)]
        [EmailAddress]
        public string EMail { get; set; }

        /// <summary>
        /// 银行名称
        /// </summary>
        [MaxLength(50)]
        public string BankName { get; set; }

        /// <summary>
        /// 开户省份
        /// </summary>
        [MaxLength(50)]
        public string BankProvice { get; set; }

        /// <summary>
        /// 开户城市
        /// </summary>
        [MaxLength(50)]
        public string BankCity { get; set; }

        /// <summary>
        /// 开户城市
        /// </summary>
        [MaxLength(50)]
        public string BankCity2 { get; set; }

        /// <summary>
        /// 开户支行
        /// </summary>
        [MaxLength(50)]
        public string BankSubName { get; set; }

        /// <summary>
        /// 银行帐号
        /// </summary>
        [MaxLength(60)]
        public string BankAccount { get; set; }

        [MaxLength(50)]
        [MinLength(6)]
        [Required]
        public string Password { get; set; }

        [Compare("Password")]
        public string PasswordConfirm { get; set; }

        [Required]
        public RoleType Role { get; set; }
        public bool? Disabled { get; set; }
        public DateTime CreateDate { get; set; }
    }

    /// <summary>
    /// 登錄
    /// </summary>
    public class UserLoginModel
    {
        public string ID { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }

    /// <summary>
    /// 修改密碼
    /// </summary>
    public class PasswordModel
    {
        [Required]
        [MaxLength(50)]
        public string Password { get; set; }

        [Compare("Password")]
        public string PasswordConfirm { get; set; }
    }
}