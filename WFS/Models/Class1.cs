using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WFS.Models.Demo
{
    //用户
    public class User
    {
        public string Id { get; set; }
        public string Name { get; set; }

        /// <summary>
        /// 用户所属部门
        /// </summary>
        public virtual Deptment Dept { get; set; }
    }

    //部门
    public class Deptment
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        /// <summary>
        /// 部门主管
        /// </summary>
        public virtual User Supervisor { get; set; }

        /// <summary>
        /// 想表示部门成员，部门:用户 成  1:N的关系
        /// </summary>

        //public virtual ICollection<User> Users { get; set; }
    }
}