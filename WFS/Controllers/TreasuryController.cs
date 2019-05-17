using AutoMapper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WFS.Filter;
using WFS.Helpers;
using WFS.Models;

namespace WFS.Controllers
{
    //[Authorize(Roles = "Finance;2")]
    [Authorize]
    [WFSAuth(Roles = "Finance")]
    public class TreasuryController : BaseController
    {
        #region 显示（查询）用户信息
        /// <summary>
        /// 部门及用户管理页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            using (WFSContext db = new WFSContext())
            {
                var Depts = db.Deptments.ToList();
                var user = db.Users.Include("Dept").ToList();
                return View(Depts);
            }

        }

        /// <summary>
        /// 表格异步获取数据
        /// 由于在前端分页及搜索，所以不需要接参数
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult TableData(Guid? DeptId)
        {
            using (WFSContext db = new WFSContext())
            {
                var rows = db.Users.Include("Dept")
                    .Where(x => DeptId == null || x.Dept.Id == DeptId)
                    .Select(x => new
                    {
                        x.ID,
                        x.Name,
                        x.Role,
                        x.CreateDate,
                        x.Disabled,
                        DeptName = x.Dept.Name
                    })
                    .ToList();
                return Json(rows);
            }
        }

        #endregion

        #region 修改用户信息

        public ActionResult Edit(string id)
        {
            using (var db = new WFSContext())
            {
                SelectList depts = new SelectList(db.Deptments.Select(x => new
                {
                    x.Id,
                    x.Name
                }).ToList(), "id", "name");
                ViewBag.Depts = depts;
                if (string.IsNullOrWhiteSpace(id))
                {
                    return View(new UserViewModel()
                    {
                        NewUser = true
                    });
                }
                else
                {

                    var user = db.Users.FirstOrDefault(x => x.ID.Trim() == id.Trim());
                    var model = Mapper.Map<UserViewModel>(user);
                    model.PasswordConfirm = model.Password;

                    return View(model);
                }
            }
        }

        [HttpPost]
        public ActionResult Edit(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                UserEntity user;
                using (WFSContext db = new WFSContext())
                {
                    if(model.NewUser && db.Users.Any(x=>x.ID.Trim() == model.ID.Trim()))
                    {
                        Response.Write("<script type='text/javascript'>alert('此ID已被使用');window.history.go(-1);</script>");//(可以用来实现在iframe中跳转到指定页面.)
                        Response.End();
                        return Content("alert('此ID已被使用');window.history.go(-1);");
                    }
                    user = db.Users.Include("Dept").FirstOrDefault(x => x.ID.Trim() == model.ID.Trim());
                    var dept = db.Deptments.Include("Users").FirstOrDefault(m => m.Id == model.DeptId);
                    if (dept == null)
                    {
                        return JavaScript("alert('部门参数错误');window.history.go(-1);");
                    }
                    if (user == null) // 如果不存在则创建新用户
                    {
                        user = Mapper.Map<UserEntity>(model);
                        user.CreateDate = DateTime.Now;

                        //加入部门
                        if (dept.Users == null)
                        {
                            dept.Users = new List<UserEntity>()
                            {
                                user
                            };
                        }
                        else
                        {
                            dept.Users.Add(user);
                        }

                        //是否是部门主管
                        if (model.Role == RoleType.Supervisor)
                        {
                            dept.Supervisor = user;
                            var oldsupervisor = dept.Users.Where(x => x.Role == RoleType.Supervisor && x.ID.Trim() != user.ID.Trim()).ToList();
                            oldsupervisor.ForEach(x =>
                            {
                                x.Role = RoleType.User;
                            });
                        }

                        db.Users.Add(user);
                    }
                    else //如果ID存在，则修改用户
                    {
                        user.Name = model.Name;
                        user.Password = model.Password;
                        user.Disabled = model.Disabled;
                        user.EMail = model.EMail;
                        
                        user.BankName = model.BankName;
                        user.BankProvice = model.BankProvice;
                        user.BankCity = model.BankCity;
                        user.BankCity2 = model.BankCity2;
                        user.BankSubName = model.BankSubName;
                        user.BankAccount = model.BankAccount;

                        if (!model.ID.Trim().Equals(LoginUser.ID, StringComparison.OrdinalIgnoreCase))
                        {
                            user.Role = model.Role;
                        }

                        //是否是部门主管
                        if (model.Role == RoleType.Supervisor)
                        {
                            dept.Supervisor = user;
                            var oldsupervisor = dept.Users.Where(x => x.Role == RoleType.Supervisor && x.ID.Trim() != user.ID.Trim()).ToList();
                            oldsupervisor.ForEach(x =>
                            {
                                x.Role = RoleType.User;
                            });
                        }

                        //从旧部门移除
                        if (user.Dept != null)
                        {
                            user.Dept.Users.Remove(user);
                        }

                        //加入部门
                        if (dept.Users == null)
                        {
                            dept.Users = new List<UserEntity>()
                            {
                                user
                            };
                        }
                        else
                        {
                            dept.Users.Add(user);
                        }

                        db.Entry<UserEntity>(user).State = System.Data.Entity.EntityState.Modified;
                    }
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }

            return View();
        }
        #endregion

        #region 删除用户信息
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
                if (id.Equals(User.Identity.Name))
                {
                    return Json(new JsonResultModel()
                    {
                        success = false,
                        message = "不能删除当前用户，操作失败。"
                    });
                }
                var u = db.Users.FirstOrDefault(x => x.ID.Trim() == id.Trim());
                db.Users.Remove(u);
                db.SaveChanges();
                return Json(new JsonResultModel()
                {
                    success = true,
                    message = "删除成功"
                });
            }
        }
        #endregion

        #region 参数设置
        /// <summary>
        /// 设置系统参数页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Setting()
        {
            using (WFSContext db = new WFSContext())
            {
                var model = db.Settings.FirstOrDefault();
                return View(model);
            }
        }

        /// <summary>
        /// 保存系统参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Setting(Settings model)
        {
            if (ModelState.IsValid)
            {
                using (WFSContext db = new WFSContext())
                {
                    ViewBag.msg = "修改完成";
                    var setting = db.Settings.FirstOrDefault();
                    setting.MaxCost = model.MaxCost;
                    setting.CountOfAll = model.CountOfAll;
                    db.SaveChanges();
                }
            }
            return View(model);
        }
        #endregion

        #region 转帐
        /// <summary>
        /// 转帐列表页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Appling()
        {
            decimal GeneralLedger = SettingHelper.CountOfAll();
            ViewBag.GeneralLedger = GeneralLedger;
            return View();
        }

        /// <summary>
        /// 表格异步获取数据
        /// 由于在前端分页及搜索，所以不需要接参数
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ApplingData()
        {
            var MaxCost = SettingHelper.MaxCost();
            using (WFSContext db = new WFSContext())
            {
                //只显示审批通过和已转帐的表单
                var rows = db.Forms
                    .Where(x => (x.ProcessCode >= ProcessCode.L20 && x.Cost < MaxCost)
                                    || (x.ProcessCode >= ProcessCode.L30 && x.Cost >= MaxCost)
                                    )
                    .OrderByDescending(x => x.CreateTime)
                    .ToList();
                return Json(rows);
            }
        }

        /// <summary>
        /// 转帐页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Remittance(string id)
        {
            using (WFSContext db = new WFSContext())
            {
                //此处不考虑参数错误以及表单不存在的情况
                var rows = db.Forms
                    .FirstOrDefault(x => x.ID == id);
                return View(rows);
            }
        }

        /// <summary>
        /// 转帐提交处理
        /// </summary>
        /// <param name="id"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Remittance(string id, HttpPostedFileBase file)
        {
            using (WFSContext db = new WFSContext())
            {
                //此处不考虑参数错误以及表单不存在的情况
                var form = db.Forms.FirstOrDefault(x => x.ID == id);

                //if (form.Status != FormStatus.Passed)
                //{
                //    return Content("此申请尚未审核通过或已转帐，操作已取消。");
                //}

                var _GeneralLedger = SettingHelper.CountOfAll();
                if (_GeneralLedger < form.Cost)
                {
                    Response.Write(string.Format("<script type='text/javascript'>alert('当前总帐余额不足，操作已取消。');window.location.href = '{0}';</script>", Url.Action("Appling")));//(可以用来实现在iframe中跳转到指定页面.)
                    Response.End();
                    return Content("");
                    //return JavaScript(string.Format("alert('当前总帐余额不足，操作已取消。');window.location.href = '{0}'", Url.Action("Appling")));
                }

                //修改状态
                //form.Status = FormStatus.TransactionComplete;

                //保存附件
                //附件名称
                var FileName = string.Empty;
                var FileID = string.Empty;

                //判断是否有上传文件
                if (Request.Files.Count > 0)
                {
                    //获取文件名
                    FileName = Path.GetFileName(Request.Files[0].FileName);

                    //保存文件，并获得保存ID（包含文件后缀名）
                    FileID = FileHelper.SaveFile(Request.Files[0], Server);

                    //保存文件名
                    form.FinFileId = FileID;
                    form.FinFileName = FileName;
                }

                //处理人
                form.FinBy = User.Identity.Name;//TODO

                ////处理时间
                form.FinDate = DateTime.Now;
                form.Status = FormStatus.Done;

                db.Entry<FormEntity>(form).State = System.Data.Entity.EntityState.Modified;

                db.ProcessLogs.Add(new ProcessLog()
                {
                    Id = Guid.NewGuid(),
                    FormId = form.ID,
                    CreateBy = User.Identity.Name,
                    CreateDate = DateTime.Now,
                    ProcessCode = ProcessCode.L40,
                    status = FormStatus.Done
                });

                //保存到数据库
                db.SaveChanges();

                //扣除总帐
                //MetaValueHelper.SetGeneralLedger(_GeneralLedger - form.Cost);
                SettingHelper.SetCountOfAll(_GeneralLedger - form.Cost);

                //发送邮件
                //var user = db.Users.FirstOrDefault(x => x.ID == form.CreateBy);
                //if (user != null && !string.IsNullOrWhiteSpace(user.EMail))
                //{
                //    string Subject = "经费申请已转帐--" + form.Title.Trim(),
                //        Content = @"{0} 您好：
                //                            你的经费申请单已经转帐。请您留意到帐情况。
                //                            单号：{1}
                //                            标题:{2}";
                //    Content = string.Format(Content, user.Name.Trim(), form.ID, form.Title);
                //    //MailHelpers.SendMail(user.EMail, Subject, Content);
                //}

                return RedirectToAction("Appling");
            }
        }
        #endregion

        #region 部门
        /// <summary>
        /// 部门编辑/创建页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult EditDept(Guid? id)
        {

            using (WFSContext db = new WFSContext())
            {
                var Users = db.Users.Select(x => new { x.ID, x.Name }).ToList();
                SelectList UsersSelectList = new SelectList(Users, "ID", "Name");
                ViewBag.UsersSelectList = UsersSelectList;
                Deptment Dept = db.Deptments.FirstOrDefault(x => x.Id == id);
                if (Dept == null)
                {
                    return View("CreateDept");
                }
                else
                {
                    var model = new DeptViewCreateModel()
                    {
                        Id = Dept.Id,
                        Name = Dept.Name.Trim(),
                        Supervisor = Dept.Supervisor == null ? "" : Dept.Supervisor.ID
                    };
                    return View("EditDept", model);
                }
            }

        }

        /// <summary>
        /// 创建部门后台
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateDept(DeptViewCreateModel model)
        {
            using (WFSContext db = new WFSContext())
            {
                //var user = db.Users.FirstOrDefault(x => x.ID == model.Supervisor);
                Deptment dept = new Deptment()
                {
                    Id = Guid.NewGuid(),
                    Name = model.Name
                };

                //把用户的角色改为部门主任
                
                db.Deptments.Add(dept);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// 编辑部门后台
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditDept(DeptViewCreateModel model)
        {
            using (WFSContext db = new WFSContext())
            {
                Deptment dept = db.Deptments.FirstOrDefault(x => x.Id == model.Id);
                if (dept == null)
                {
                    return Content("参数错误");
                }

                dept.Name = model.Name.Trim();

                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// 删除部门
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DeleteDept(Guid? id)
        {
            using (WFSContext db = new WFSContext())
            {
                var dept = db.Deptments.Include("Users").FirstOrDefault(x => x.Id == id);
                if (dept == null)
                {
                    return Json(new { success = false, message = "没有找到部门" }, JsonRequestBehavior.AllowGet);
                }
                else if (dept.Users != null && dept.Users.Count > 0)
                {
                    return Json(new { success = false, message = "不能删除有人员的部门。" }, JsonRequestBehavior.AllowGet);
                }

                db.Deptments.Remove(dept);
                db.SaveChanges();
                return Json(new { success = true, message = "删除成功。" }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
    }
}