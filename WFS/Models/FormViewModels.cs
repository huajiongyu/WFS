using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WFS.Models;

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

        [Range(0, float.MaxValue)]
        [Required]
        public decimal Cost { get; set; }

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

        public bool Enable { get; set; }
        
        public List<ProcessLog> log { get; set; }
    }
}