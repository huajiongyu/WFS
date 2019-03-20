using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WFS.Models;

namespace WFS.Controllers
{
    public class AssessorController : Controller
    {
        #region 显示（查询）
        // GET: Treasury
        public ActionResult Index()
        {
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
                var rows = db.Forms.OrderByDescending(x => x.CreateTime).ToList();
                return Json(rows);
            }
        }

        #endregion

        #region 表单操作
        [HttpPost]
        public ActionResult Pass(string ID)
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

                form.Status = FormStatus.Passed;
                form.PassDate = DateTime.Now; //通过时间
                form.PasswordBy = "jason";//通过人
                db.Entry<FormEntity>(form).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                return Json(new JsonResultModel()
                {
                    success = true,
                    message = "操作成功.已通过申请。"
                });
            }
        }

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
                    message = "操作成功.已通过申请。"
                });
            }
        }
        #endregion
    }
}