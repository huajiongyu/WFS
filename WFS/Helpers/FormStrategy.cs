using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using WFS.Models;

namespace WFS.Helpers
{
    /// <summary>
    /// 表单策略管理器
    /// 
    /// </summary>
    public class FormStrategy
    {
        private static List<ProcessCode> Process = new List<ProcessCode>()
        {
            ProcessCode.L0,
            ProcessCode.L10,
            ProcessCode.L20,
            ProcessCode.L30,
            ProcessCode.L40
        };

        /// <summary>
        /// 通过表单
        /// </summary>
        /// <param name="FormId">表单ID</param>
        /// <param name="ToCode">目标状态码</param>
        /// <param name="Account">操作人员</param>
        /// <param name="Role">操作人员身份</param>
        public static bool CheckPassForm(string FormId, ProcessCode ToCode, string Account, RoleType Role)
        {
            using (var db = new WFSContext())
            {
                var MaxCost = SettingHelper.MaxCost();

                //1.查找表单
                var form = db.Forms.Include("ProcessLog").FirstOrDefault(x => x.ID.Trim() == FormId.Trim());
                if (form == null)
                {
                    throw new ArgumentNullException("没有找到表单:" + FormId);
                }
                else if (form.Status == FormStatus.Calcel || form.Status == FormStatus.Return)
                {
                    return false;
                }

                //2.检查流程是否跳级
                if(Role == RoleType.Supervisor)
                {
                    var per = db.Users.FirstOrDefault(x => x.ID == form.CreateBy);
                    if(per.Dept.Supervisor.ID.Trim().Equals(Account.Trim(), StringComparison.OrdinalIgnoreCase) && form.ProcessCode == ProcessCode.L0 )
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                var lastCode = LastCode(ToCode);
                if (form.Cost < MaxCost && ToCode == ProcessCode.L40)//财务转帐
                {
                    if (form.ProcessCode != ProcessCode.L20)
                    {
                        return false;
                    }
                }
                else if (form.ProcessCode != lastCode)
                {
                    return false;

                }

                //2.1检查权限
                if (IsInRoleRight(ToCode, Role) == false)
                {
                    return false;
                }
                return true;
            }
        }

        /// <summary>
        /// 通过表单
        /// </summary>
        /// <param name="FormId">表单ID</param>
        /// <param name="ToCode">目标状态码</param>
        /// <param name="Account">操作人员</param>
        /// <param name="Role">操作人员身份</param>
        public static bool PassForm(string FormId, ProcessCode ToCode, FormStatus Status, string Account, RoleType Role, string Remark = "")
        {
            using (var db = new WFSContext())
            {
                var MaxCost = SettingHelper.MaxCost();

                //1.查找表单
                var form = db.Forms.Include("ProcessLog").FirstOrDefault(x => x.ID.Trim() == FormId.Trim());
                if (form == null)
                {
                    throw new ArgumentNullException("没有找到表单:" + FormId);
                }
                else if (form.Status == FormStatus.Calcel || form.Status == FormStatus.Return)
                {
                    throw new Exception("表单已取消或驳回，不能再操作");
                }

                //2.检查流程是否跳级
                if(CheckPassForm(FormId, ToCode, Account, Role) == false)
                {
                    throw new Exception("你没有权限处理此流程.");
                }

                //3.修改表单
                form.Status = Status;
                form.ProcessCode = ToCode;
                form.ProcessTime = DateTime.Now;
                form.ReturnRemark = Remark;

                //4.创建过程记录
                if(form.ProcessLog == null)
                {
                    form.ProcessLog = new List<ProcessLog>();
                }
                ProcessLog log = new ProcessLog()
                {
                    CreateBy = Account,
                    CreateDate = DateTime.Now,
                    FormId = form.ID,
                    ProcessCode = ToCode,
                    Remark = Remark,
                    Id = Guid.NewGuid()
                };
                form.ProcessLog.Add(log);

                //5.保存
                db.SaveChanges();
                return true;
            }

        }

        /// <summary>
        /// 上一个代码
        /// </summary>
        /// <param name="ToCode"></param>
        /// <returns></returns>
        private static ProcessCode LastCode(ProcessCode ToCode)
        {
            var index = Process.FindIndex(x => x == ToCode);
            if (index <= 0)
            {
                return ProcessCode.L0;
            }
            else
            {
                return Process[index - 1];
            }
        }

        /// <summary>
        /// 检查角色是否拥有ToCode的权限
        /// </summary>
        /// <param name="ToCode"></param>
        /// <param name="Role"></param>
        /// <returns></returns>
        private static bool IsInRoleRight(ProcessCode ToCode, RoleType Role)
        {
            switch (Role)
            {
                case RoleType.Assessor:
                    if(ToCode == ProcessCode.L20)
                    {
                        return true;
                    }
                    break;
                case RoleType.Finance:
                    if(ToCode == ProcessCode.L40)
                    {
                        return true;
                    }
                    break;
                case RoleType.Hearmaster:
                    if(ToCode == ProcessCode.L30)
                    {
                        return true;
                    }
                    break;
                case RoleType.Supervisor:
                    if(ToCode == ProcessCode.L10)
                    {
                        return true;
                    }
                    break;
                default:
                    return false;
            }
            return false;
        }

        /// <summary>
        /// 获取枚举的Display属性值
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string ProcessString(ProcessCode code)
        {
            var type = code.GetType();//先获取这个枚举的类型
            var field = type.GetField(code.ToString());//通过这个类型获取到值
            var obj = (DisplayAttribute)field.GetCustomAttribute(typeof(DisplayAttribute));//得到特性
            return obj.Name ?? "";
        }

        /// <summary>
        /// 获取枚举的Display属性值
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string StatusString(FormStatus status)
        {
            var type = status.GetType();//先获取这个枚举的类型
            var field = type.GetField(status.ToString());//通过这个类型获取到值
            var obj = (DisplayAttribute)field.GetCustomAttribute(typeof(DisplayAttribute));//得到特性
            return obj.Name ?? "";
        }

        public static string FormStatusDesc(string formid)
        {
            using(WFSContext db = new WFSContext())
            {
                var form = db.Forms.FirstOrDefault(x => x.ID == formid);
                switch (form.Status)
                {
                    case FormStatus.Calcel:
                        return "已取消";
                    case FormStatus.Done:
                        return "已结案";
                    default:
                        if(form.ProcessCode == ProcessCode.L0)
                        {
                            return ProcessString(form.ProcessCode);
                        }
                        return ProcessString(form.ProcessCode) + ":" + StatusString(form.Status);
                    
                }
            }

        }
    }
}