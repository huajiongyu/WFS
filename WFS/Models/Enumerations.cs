namespace WFS.Models
{
    ///用到的枚举变量
    ///

    public enum FormType
    {
        /// <summary>
        /// 预算申请
        /// </summary>
        Budget = 0,

        /// <summary>
        /// 报销申请
        /// </summary>
        Expense = 1
    }

    public enum FormStatus
    {
        /// <summary>
        /// 审批通回
        /// </summary>
        Return = 0,

        /// <summary>
        /// 审批中
        /// </summary>
        Appling = 1,

        /// <summary>
        /// 审批通过
        /// </summary>
        Passed = 2,

        /// <summary>
        /// 转帐完成
        /// </summary>
        TransactionComplete = 3
    }

    public enum RoleType
    {
        /// <summary>
        /// 普通用户
        /// </summary>
        User = 0,

        /// <summary>
        /// 审核人员
        /// </summary>
        Assessor = 1,

        /// <summary>
        /// 财务人员
        /// </summary>
        Finance = 2
    }
}