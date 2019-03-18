using System;
using System.ComponentModel.DataAnnotations;

namespace WFS.Models
{
    /// <summary>
    /// 申请表单
    /// </summary>
    public class FormCreateModel
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

        /// <summary>
        /// 表单状态
        /// </summary>
        [Required]
        public FormType Status { get; set; }

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
}