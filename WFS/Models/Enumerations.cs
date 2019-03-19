using System.ComponentModel.DataAnnotations;

namespace WFS.Models
{
    ///用到的枚举变量
    ///

    public enum FormType
    {
        /// <summary>
        /// 预算申请
        /// </summary>
        [Display(Name = "预算申请")]
        Budget = 0,

        /// <summary>
        /// 报销申请
        /// </summary>
        [Display(Name = "报销申请")]
        Expense = 1
    }

    public enum FormStatus
    {
        /// <summary>
        /// 审批退回
        /// </summary>
        [Display(Name = "审批退回")]
        Return = 0,

        /// <summary>
        /// 审批中
        /// </summary>
        [Display(Name = "审批中")]
        Appling = 1,

        /// <summary>
        /// 审批通过
        /// </summary>
        [Display(Name = "审批通过")]
        Passed = 2,

        /// <summary>
        /// 转帐完成
        /// </summary>
        [Display(Name = "转帐完成")]
        TransactionComplete = 3
    }

    public enum RoleType
    {
        /// <summary>
        /// 普通用户
        /// </summary>
        [Display(Name ="普通用户")]
        User = 0,

        /// <summary>
        /// 审核人员
        /// </summary>
        [Display(Name = "审核人员")]
        Assessor = 1,

        /// <summary>
        /// 财务人员
        /// </summary>
        [Display(Name = "财务人员")]
        Finance = 2
    }
}