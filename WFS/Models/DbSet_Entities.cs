using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using Newtonsoft.Json;

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

        [JsonIgnore]
        public virtual Deptment Dept { get; set; }

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
        /// 当前流程码
        /// </summary>
        public ProcessCode ProcessCode { get; set; }

        /// <summary>
        /// 最后一次处理时间
        /// </summary>
        public DateTime? ProcessTime { get; set; }

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
        /// 创建人
        /// </summary>
        [Required]
        [MaxLength(36)]
        public string CreateBy { get; set; }

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

        public virtual ICollection<ProcessLog> ProcessLog { get; set; }
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

        [JsonIgnore]
        public ICollection<UserEntity> Users { get; set; }
    }

    /// <summary>
    /// 表单流程记录
    /// </summary>
    public class ProcessLog
    {
        [Key]
        [Required]
        public Guid Id { get; set; }

        /// <summary>
        /// 表单编号
        /// </summary>
        [MaxLength(15)]
        [Required]
        public string FormId { get; set; }

        /// <summary>
        /// 处理流程
        /// </summary>

        public ProcessCode ProcessCode { get; set; }

        /// <summary>
        /// 表单状态
        /// </summary>

        public FormStatus status { get; set; }

        /// <summary>
        /// 驳回原因
        /// </summary>

        public string Remark { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>

        public virtual string CreateBy { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime CreateDate { get; set; }
    }

    /// <summary>
    /// 系统设置
    /// </summary>
    public class Settings
    {
        /// <summary>
        /// 系统设置
        /// </summary>
        [MaxLength(50)]
        [Required]
        [Key]
        public string Id { get; set; }

        /// <summary>
        /// 审批人员最大审批金额
        /// 超过的需要再由校长审批
        /// </summary>
        public decimal MaxCost { get; set; }

        /// <summary>
        /// 现金池
        /// </summary>
        [Required]
        [Range(1, 20000000)]
        public decimal CountOfAll { get; set; }
    }
}