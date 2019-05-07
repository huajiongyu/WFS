using System;
using System.Collections.Generic;
using System.Linq;
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
                var lastCode = LastCode(ToCode);
                if (form.Cost < MaxCost && ToCode == ProcessCode.L40)//财务转帐
                {
                    if (form.ProcessCode != ProcessCode.L20)
                    {
                        throw new Exception("不能越级处理。");
                    }
                }
                else if (form.ProcessCode != lastCode)
                {
                    throw new Exception("不能越级处理。");

                }

                //2.1检查权限
                if(IsInRoleRight(ToCode, Role) == false)
                {
                    throw new Exception("您没有此权限");
                }

                //3.修改表单
                form.Status = Status;
                form.ProcessCode = ToCode;
                form.ReturnRemark = Remark;

                //4.创建过程记录
                if(form.ProcessLog == null)
                {
                    form.ProcessLog = new List<ProcessLog>();
                }
                ProcessLog log = new ProcessLog()
                {
                    CreateBy = db.Users.FirstOrDefault(x => x.ID == Account),
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
    }
}