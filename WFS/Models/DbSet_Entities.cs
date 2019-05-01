using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

/// <summary>
/// 所有DB数据模型
/// </summary>
namespace WFS.Models
{
    /// <summary>
    /// 用户表
    /// </summary>
    public class UserEntity
    {
        [MaxLength(36)]
        [Required]
        [Column(TypeName ="Char")]
        public string ID { get; set; }

        [MaxLength(20)]
        [Required]
        public string Name { get; set; }

        [MaxLength(50)]
        [EmailAddress]
        public string EMail { get; set; }

        [MaxLength(50)]
        [Required]
        public string Password { get; set; }

        [Required]
        public RoleType Role { get; set; }

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

        public bool? Disabled { get; set; }
        public DateTime CreateDate { get; set; }

    }

    /// <summary>
    /// 申请表单
    /// </summary>
    public class FormEntity
    {
        /// <summary>
        /// 表单编号
        /// </summary>
        [MaxLength(15)]
        [Required]
        [Key]
        public string ID { get; set; }

        /// <summary>
        /// 表单类型
        /// </summary>
        [Required]
        public FormType Type { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        /// <summary>
        /// 申请内容
        /// </summary>
        [Required]
        [MaxLength(4000)]
        public string Content { get; set; }

        [Range(0, float.MaxValue, ErrorMessage = "请输入数字金额")]
        [Required]
        public decimal Cost { get; set; }

        /// <summary>
        /// 退回原因
        /// </summary>
        public string ReturnRemark { get; set; }

        /// <summary>
        /// 表单状态
        /// </summary>
        [Required]
        public FormStatus Status { get; set; }

        /// <summary>
        /// 表单创建日期
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 审批通过日期
        /// </summary>
        public DateTime? PassDate { get; set; }

        /// <summary>
        /// 转帐日期
        /// </summary>
        public DateTime? FinDate { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        [Required]
        [MaxLength(36)]
        public string CreateBy { get; set; }

        /// <summary>
        /// 审批人
        /// </summary>
        [MaxLength(36)]
        public string PasswordBy { get; set; }

        /// <summary>
        /// 财务处理人员
        /// </summary>
        [MaxLength(36)]
        public string FinBy { get; set; }

        /// <summary>
        /// 附件原文件名
        /// </summary>
        [MaxLength(100)]
        public string FileName { get; set; }

        /// <summary>
        /// 附件ID
        /// </summary>
        [MaxLength(100)]
        public string FileId { get; set; }

        /// <summary>
        /// 财务文件原文件名
        /// </summary>
        [MaxLength(100)]
        public string FinFileName { get; set; }

        /// <summary>
        /// 财务文件ID
        /// </summary>
        [MaxLength(100)]
        public string FinFileId { get; set; }
    }

    /// <summary>
    /// 系统参数表
    /// 存储系统使用的参数，如：总帐余额， 网站名称等
    /// </summary>
    public class MetaValues
    {
        /// <summary>
        /// 参数名称
        /// </summary>
        [MaxLength(30)]
        [Key]
        public string MetaID { get; set; }

        /// <summary>
        /// 参数值
        /// </summary>
        [MaxLength(2000)]
        public string Value { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [MaxLength(500)]
        public string Desription { get; set; }
    }

    public class Bank
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }
    }

    /// <summary>
    /// 部门
    /// </summary>
    public class Deptment
    {

        /// <summary>
        /// 部门编码
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        [MaxLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// 部门主任
        /// </summary>        
        public virtual UserEntity Supervisor { get; set; }

        public ICollection<UserEntity> Users { get; set; }
    }
}