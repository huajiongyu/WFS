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
        /// 审批驳回
        /// </summary>
        [Display(Name = "驳回")]
        Return = 0,

        /// <summary>
        /// 审批中
        /// </summary>
        [Display(Name = "审批完成")]
        Appling = 1,

        /// <summary>
        /// 取消
        /// </summary>
        [Display(Name = "取消")]
        Calcel = 2,

        /// <summary>
        /// 完结
        /// </summary>
        [Display(Name = "完结")]
        Done = 3
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
        Finance = 2,

        /// <summary>
        /// 校长
        /// </summary>
        [Display(Name ="校长")]
        Hearmaster = 3,

        /// <summary>
        /// 部门主管
        /// </summary>
        [Display(Name ="部门主管")]
        Supervisor = 4
    }

    /// <summary>
    /// 表单流程
    /// </summary>
    public enum ProcessCode
    {
        /// <summary>
        /// 创建表单
        /// </summary>
        [Display(Name ="创建表单")]
        L0 = 0,

        /// <summary>
        /// 主任审核
        /// </summary>
        [Display(Name = "主任审核")]
        L10 = 10,

        /// <summary>
        /// 审核
        /// </summary>
        [Display(Name = "审核人员")]
        L20 = 20,

        /// <summary>
        /// 校长审核
        /// </summary>
        [Display(Name = "校长审核")]
        L30 = 30,

        /// <summary>
        /// 财务转帐
        /// </summary>
        [Display(Name = "财务转帐")]
        L40 = 40
    }
}