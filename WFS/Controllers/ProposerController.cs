﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WFS.Filter;
using WFS.Helpers;
using WFS.Models;

namespace WFS.Controllers
{
    [Authorize]//登录验证
    [WFSAuth(Roles = "User")]//权限管理筛选器
    public class ProposerController : BaseController
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
                //var rows = db.Forms.Include("ProcessLog")
                //    .Where(x=>x.CreateBy == User.Identity.Name)
                //    .OrderByDescending(x=>x.CreateTime)
                //    .ToList();
                List<FormCreateModel> rows = new List<FormCreateModel>();
                var forms = //db.Forms.OrderByDescending(x => x.CreateTime).ToList();
                    (from f in db.Forms
                     join u in db.Users on f.CreateBy equals u.ID
                     join d in db.Deptments on u.Dept equals d
                     where f.CreateBy == User.Identity.Name
                     select f
                     ).ToList();
                rows = AutoMapper.Mapper.Map<List<FormCreateModel>>(forms);
                rows.ForEach(x =>
                {
                    x.Enable = (x.Status == FormStatus.Appling && x.ProcessCode == ProcessCode.L0);
                    x.CurentStatusDesc = FormStrategy.FormStatusDesc(x.ID);
                });
                return Json(rows);
            }
        }

        #endregion

        #region 修改

        public ActionResult Edit(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return View();
            }
            else
            {
                using (var db = new WFSContext())
                {
                    var form = db.Forms.FirstOrDefault(x => x.ID.Trim() == id.Trim());
                    if(form != null)
                    {
                        //检查表单状态，如果状态已通过申请，返回提示
                        if (form.Status != FormStatus.Appling)
                        {
                            return Json(new JsonResultModel()
                            {
                                success = false,
                                message = "此表单不能再做此操作。"
                            });
                        }

                        var model = Mapper.Map<FormCreateModel>(form);
                        return View(model);
                    }else
                    {
                        return View();
                    }
                }
            }
        }

        [HttpPost]
        public ActionResult Edit(FormCreateModel model)
        {
            if (ModelState.IsValid)
            {
                FormEntity form;
                using (WFSContext db = new WFSContext())
                {
                    //附件名称
                    var FileName = string.Empty;
                    var FileID = string.Empty;

                    //判断是否有上传文件
                    if(Request.Files.Count > 0 && Request.Files[0].ContentLength > 0)
                    {
                        //获取文件名
                        FileName = Path.GetFileName(Request.Files[0].FileName);

                        //保存文件，并获得保存ID（包含文件后缀名）
                        FileID = FileHelper.SaveFile(Request.Files[0], Server);
                    }

                    form = db.Forms.FirstOrDefault(x => x.ID.Trim() == model.ID.Trim());
                    if (form == null) // 如果不存在则创建
                    {
                        //使用AutoMapper转换模型
                        form = Mapper.Map<FormEntity>(model);

                        //设置创建时间
                        form.CreateTime = DateTime.Now;

                        //设置当前登录人为创建人
                        form.CreateBy = User.Identity.Name;

                        //获取一个新的单号
                        form.ID = GetFormName();

                        form.FileName = FileName;
                        form.FileId = FileID;
                        form.Status = FormStatus.Appling;
                        form.ProcessCode = ProcessCode.L0;
                        form.ProcessTime = DateTime.Now;

                        form.ProcessLog = new List<ProcessLog>()
                        {
                            new ProcessLog()
                            {
                                Id = Guid.NewGuid(),
                                CreateBy = User.Identity.Name,
                                CreateDate = DateTime.Now,
                                FormId = form.ID,
                                ProcessCode = ProcessCode.L0,
                                status = FormStatus.Appling
                            }
                        };

                        //插入数据库
                        db.Forms.Add(form);
                    }
                    else //如果存在，则修改
                    {
                        //检查表单状态，如果状态已通过申请，返回提示
                        if (form.Status != FormStatus.Appling)
                        {
                            return Json(new JsonResultModel()
                            {
                                success = false,
                                message = "此表单不能再做此操作。"
                            });
                        }

                        //如果上传了附件，则删除旧附件，并更新新的文件名称及ID
                        if (!string.IsNullOrWhiteSpace(FileName))
                        {
                            form.FileName = FileName;
                            form.FileId = FileID;
                        }

                        form.Type = model.Type;
                        form.Title = model.Title;
                        form.Content = model.Content;
                        form.Cost = model.Cost;

                        db.Entry<FormEntity>(form).State = System.Data.Entity.EntityState.Modified;
                    }

                    //保存数据
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }else
            {
                StringBuilder errinfo = new StringBuilder();
                foreach (var s in ModelState.Values)
                {
                    foreach (var p in s.Errors)
                    {
                        errinfo.AppendFormat("{0}\\n", p.ErrorMessage);
                    }
                }

                Response.Write("<script type='text/javascript'>alert('" + errinfo.ToString() +"');window.history.go(-1);</script>");//(可以用来实现在iframe中跳转到指定页面.)
                Response.End();
                return Content("");
                //return JavaScript("alert('" + errinfo.ToString() + "')");
            }            
        }
        #endregion

        #region 删除
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(string id)
        {
            using (var db = new WFSContext())
            {
                id = id ?? "";//防止传空指针
                var u = db.Forms.Include("ProcessLog").FirstOrDefault(x => x.ID.Trim() == id.Trim());
                if(u.ProcessLog != null)
                {
                    db.ProcessLogs.RemoveRange(u.ProcessLog);
                }

                db.Forms.Remove(u);
                db.SaveChanges();
                return Json(new JsonResultModel()
                {
                    success = true,
                    message = "删除成功!"
                });
            }
        }
        #endregion

        #region 下载附件
        public ActionResult DownLoad(string fid)
        {
            var filename = Path.Combine(Server.MapPath("~/Uploads/"), fid.ToString());
            var ex = Path.GetExtension(filename);
            return File(filename, "applicatioin/" + ex, fid);
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 获取一个表单编号
        /// </summary>
        /// <returns></returns>
        private string GetFormName()
        {
            using (var db = new WFSContext())
            {
                var preFix = "F" + DateTime.Now.ToString("yyyyMMdd");
                var _maxForm = db.Forms.Where(x => x.ID.Contains(preFix))
                    .OrderBy(x => x.ID)
                    .Max(x => x.ID);
                if (_maxForm == null)
                {
                    return preFix + "0001";
                }
                else
                {
                    //此处应该考虑转换报错
                    int _end = int.Parse(_maxForm.Substring(_maxForm.Length - 3, 3));
                    _end++;
                    return preFix + (_end.ToString("0000"));
                }
            }
        }

        
        #endregion
    }
}