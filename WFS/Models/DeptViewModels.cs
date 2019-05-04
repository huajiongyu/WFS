using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// 部门页面数据模型
/// </summary>
namespace WFS.Models
{

    /// <summary>
    /// 创建部门
    /// </summary>
    public class DeptViewCreateModel
    {
        public Guid? Id { get; set; }

        public string Name { get; set; }
        public string Supervisor { get; set; }
    }
}