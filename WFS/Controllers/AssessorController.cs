using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WFS.Models;
using WFS.Helpers;
using WFS.Filter;

namespace WFS.Controllers
{
    [Authorize]
    public class AssessorController : BaseController
    {
        #region 显示（查询）
        [WFSAuth(Roles= "Assessor,Hearmaster,Supervisor")]
        public ActionResult Index()
        {
            ViewBag.Role = Role;
            ViewBag.UserName = LoginUser.ID;
            return View();
        }

        /// <summary>
        /// 表格异步获取数据
        /// 由于在前端分页及搜索，所以不需要接参数
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [WFSAuth(Roles = "Assessor,Hearmaster,Supervisor")]
        public ActionResult TableData()
        {
            using (WFSContext db = new WFSContext())
            {

                ProcessCode toCode = ProcessCode.L0;
                switch (LoginUser.Role)
                {
                    case RoleType.Assessor:
                        toCode = ProcessCode.L20;
                        break;
                    case RoleType.Finance:
                        toCode = ProcessCode.L40;
                        break;
                    case RoleType.Hearmaster:
                        toCode = ProcessCode.L30;
                        break;
                    case RoleType.Supervisor:
                        toCode = ProcessCode.L10;
                        break;
                }

                var MaxCost = SettingHelper.MaxCost();

                List<FormCreateModel> rows = new List<FormCreateModel>();
                var forms = //db.Forms.OrderByDescending(x => x.CreateTime).ToList();
                    (from f in db.Forms
                     join u in db.Users on f.CreateBy equals u.ID
                     join d in db.Deptments on u.Dept equals d
                     //1、不列出用户自己取消的表单
                     //2、不列出还没到自己审批的表单
                     //2.1、部门主任不列也不属于自己部门的表单
                     where !(f.ProcessCode == ProcessCode.L0 && f.Status == FormStatus.Calcel)//用户自己取消的表单
                     &&( (LoginUser.Role == RoleType.Supervisor && d.Supervisor.ID.Trim() == LoginUser.ID.Trim())//部门主任可以查看除了用户取消以外的所有表单
                         || (LoginUser.Role == RoleType.Assessor && f.ProcessCode >= ProcessCode.L10)//审核人员可以查看部门主任审批和自已审批过的表单
                         || (LoginUser.Role == RoleType.Hearmaster && f.ProcessCode >= ProcessCode.L20)//校长可以查看审批人员审批过和自己审批过的表单
                         //财务可以查看校长审批过和审批人员已通过而且额度小到需要校长审批的表单
                         || (LoginUser.Role == RoleType.Finance && (
                                    (f.ProcessCode >= ProcessCode.L20 && f.Cost < MaxCost)
                                    || (f.ProcessCode >= ProcessCode.L30 && f.Cost >= MaxCost))
                     ))
                     select f
                     ).ToList();

                rows = AutoMapper.Mapper.Map<List<FormCreateModel>>(forms);



                rows.ForEach(x =>
                {
                    x.Enable = FormStrategy.CheckPassForm(x.ID, toCode, LoginUser.ID, LoginUser.Role);
                    x.CurentStatusDesc = FormStrategy.FormStatusDesc(x.ID);
                });
                return Json(rows);
            }
        }

        public ActionResult Detail(string id)
        {
            using (WFSContext db = new WFSContext())
            {
                var form = db.Forms.FirstOrDefault(x => x.ID == id.Trim());
                return View(form);
            }
        }
        #endregion

        #region 表单操作
        /// <summary>
        /// 通过表单
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpPost]
        [WFSAuth(Roles = "Assessor,Hearmaster,Supervisor")]
        public ActionResult Pass(string ID)
        {
            try
            {
                ProcessCode ToCode = ProcessCode.L0;
                switch (LoginUser.Role)
                {
                    case RoleType.Assessor:
                        ToCode = ProcessCode.L20;
                        break;
                    case RoleType.Finance:
                        ToCode = ProcessCode.L40;
                        break;
                    case RoleType.Hearmaster:
                        ToCode = ProcessCode.L30;
                        break;
                    case RoleType.Supervisor:
                        ToCode = ProcessCode.L10;
                        break;
                    default:
                        throw new Exception("你没有权限执行此操作");
                }
                FormStrategy.PassForm(ID, ToCode, FormStatus.Appling, User.Identity.Name, LoginUser.Role);
                return Json(new JsonResultModel()
                {
                    success = true,
                    message = "操作成功.已通过申请。"
                });
            }catch(Exception ex)
            {
                return Json(new JsonResultModel()
                {
                    success = false,
                    message = ex.Message
                });
            }
        }
      
        /// <summary>
        /// 退回表单
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Remark"></param>
        /// <returns></returns>
        [HttpPost]
        [WFSAuth(Roles = "Assessor,Hearmaster,Supervisor")]
        public ActionResult CallBack(string ID, string Remark)
        {
            try
            {
                ProcessCode ToCode = ProcessCode.L0;
                switch (LoginUser.Role)
                {
                    case RoleType.Assessor:
                        ToCode = ProcessCode.L20;
                        break;
                    case RoleType.Finance:
                        ToCode = ProcessCode.L40;
                        break;
                    case RoleType.Hearmaster:
                        ToCode = ProcessCode.L30;
                        break;
                    case RoleType.Supervisor:
                        ToCode = ProcessCode.L10;
                        break;
                    default:
                        throw new Exception("你没有权限执行此操作");
                }
                FormStrategy.PassForm(ID, ToCode, FormStatus.Return, User.Identity.Name, LoginUser.Role, Remark);
                return Json(new JsonResultModel()
                {
                    success = true,
                    message = "操作成功。"
                });
            }catch(Exception ex)
            {
                return Json(new JsonResultModel()
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        #endregion


    }
}