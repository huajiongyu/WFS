using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WFS.Models;
using WFS.Helpers;

namespace WFS.Controllers
{
    [Authorize]
    public class AssessorController : BaseController
    {
        #region 显示（查询）
        // GET: Treasury
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
        public ActionResult TableData()
        {
            using (WFSContext db = new WFSContext())
            {
                List<FormCreateModel> rows = new List<FormCreateModel>();
                var forms = //db.Forms.OrderByDescending(x => x.CreateTime).ToList();
                    (from f in db.Forms
                     join u in db.Users on f.CreateBy equals u.ID
                     join d in db.Deptments on u.Dept equals d
                     where !(LoginUser.Role == RoleType.Supervisor && d.Supervisor.ID != LoginUser.ID)
                     select f
                     ).ToList();
                rows = AutoMapper.Mapper.Map<List<FormCreateModel>>(forms);

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


            //using (WFSContext db = new WFSContext())
            //{
            //    //防止空指针
            //    ID = ID ?? "";

            //    //根据ID查找表单
            //    var form = db.Forms.FirstOrDefault(x => x.ID.Trim() == ID.Trim());
            //    if (form == null)//如果表单为空，提示没有数据
            //    {
            //        return Json(new JsonResultModel()
            //        {
            //            success = false,
            //            message = "找不到数据"
            //        });
            //    }

            //    //检查表单状态，如果状态已通过申请，返回提示
            //    if (form.Status > FormStatus.Passed)
            //    {
            //        return Json(new JsonResultModel()
            //        {
            //            success = false,
            //            message = "此表单不能再做此操作。"
            //        });
            //    }
            //    if (form.Status == FormStatus.Passed2)
            //    {
            //        form.Status = FormStatus.Passed;
            //    }
            //    else if (form.Cost >= 5000 && form.Status == FormStatus.Appling)
            //    {
            //        form.Status = FormStatus.Passed2;
            //    }
            //    else if (form.Status == FormStatus.Appling)
            //    {
            //        form.Status = FormStatus.Passed;
            //    }
            //    form.PassDate = DateTime.Now; //通过时间
            //    form.PasswordBy = "jason";//通过人
            //    db.Entry<FormEntity>(form).State = System.Data.Entity.EntityState.Modified;
            //    db.SaveChanges();

            //    return Json(new JsonResultModel()
            //    {
            //        success = true,
            //        message = "操作成功.已通过申请。"
            //    });
            //}
        }

        /*
        
        /// <summary>
        /// 退回表单
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Remark"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CallBack(string ID, string Remark)
        {
            using (WFSContext db = new WFSContext())
            {
                //防止空指针
                ID = ID ?? "";

                //根据ID查找表单
                var form = db.Forms.FirstOrDefault(x => x.ID.Trim() == ID.Trim());
                if (form == null)//如果表单为空，提示没有数据
                {
                    return Json(new JsonResultModel()
                    {
                        success = false,
                        message = "找不到数据"
                    });
                }

                //检查表单状态，如果状态已通过申请，返回提示
                if (form.Status > FormStatus.Passed)
                {
                    return Json(new JsonResultModel()
                    {
                        success = false,
                        message = "此表单不能再做此操作。"
                    });
                }

                form.Status = FormStatus.Return;
                form.PassDate = DateTime.Now; //通过时间
                form.PasswordBy = "jason";//通过人
                form.ReturnRemark = Remark;
                db.Entry<FormEntity>(form).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                return Json(new JsonResultModel()
                {
                    success = true,
                    message = "审批驳回成功。"
                });
            }
        }
*/
        #endregion


    }
}