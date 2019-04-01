using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WFS.Models;
using WFS.Helpers;
using System.IO;

namespace WFS.Controllers
{
    //[Authorize(Roles = "Finance;2")]
    [Authorize]
    public class TreasuryController : Controller
    {
        #region 显示（查询）用户信息
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
                var rows = db.Users.ToList();
                return Json(rows);
            }
        }

        #endregion

        #region 修改用户信息

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
                    user = db.Users.FirstOrDefault(x => x.ID.Trim() == model.ID.Trim());
                    if (user == null) // 如果不存在则创建新用户
                    {
                        user = Mapper.Map<UserEntity>(model);
                        user.CreateDate = DateTime.Now;
                        db.Users.Add(user);
                    }
                    else //如果ID存在，则修改用户
                    {
                        user.Name = model.Name;
                        user.Password = model.Password;
                        user.Disabled = model.Disabled;
                        user.EMail = model.EMail;
                        user.Role = model.Role;
                        user.BankName = model.BankName;
                        user.BankProvice = model.BankProvice;
                        user.BankCity = model.BankCity;
                        user.BankCity2 = model.BankCity2;
                        user.BankSubName = model.BankSubName;
                        user.BankAccount = model.BankAccount;

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
            decimal GeneralLedger = MetaValueHelper.GetGeneralLedger();
            var setting = MetaValueHelper.GetMailSetting();
            if (setting == null)
            {
                setting = new MailSettingModel()
                {
                    Port = 25
                };
            }
            SettingViewModel model = AutoMapper.Mapper.Map<SettingViewModel>(setting);
            model.GeneralLedger = GeneralLedger;
            return View(model);
        }

        /// <summary>
        /// 保存系统参数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Setting(SettingViewModel model)
        {
            if (ModelState.IsValid)
            {
                //MetaValueHelper.SetGeneralLedger(model.GeneralLedger);
                var MailSetting = AutoMapper.Mapper.Map<MailSettingModel>(model);
                MetaValueHelper.SetMailMeta(MailSetting);
            }
            return View(model);
        }
        #endregion

        #region 转帐
        public ActionResult Appling()
        {
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
            using (WFSContext db = new WFSContext())
            {
                //只显示审批通过和已转帐的表单
                var rows = db.Forms
                    .Where(x => x.Status >= FormStatus.Passed)
                    .OrderByDescending(x => x.CreateTime)
                    .ToList();
                return Json(rows);
            }
        }

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

        [HttpPost]
        public ActionResult Remittance(string id, HttpPostedFileBase file)
        {
            using (WFSContext db = new WFSContext())
            {
                //此处不考虑参数错误以及表单不存在的情况
                var form = db.Forms.FirstOrDefault(x => x.ID == id);

                if (form.Status != FormStatus.Passed)
                {
                    return Content("此申请尚未审核通过或已转帐，操作已取消。");
                }

                var _GeneralLedger = MetaValueHelper.GetGeneralLedger();
                if (_GeneralLedger < form.Cost)
                {
                    return Content("当前总帐余额不足，操作已取消。");
                }

                //修改状态
                form.Status = FormStatus.TransactionComplete;

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

                //处理时间
                form.FinDate = DateTime.Now;

                db.Entry<FormEntity>(form).State = System.Data.Entity.EntityState.Modified;

                //保存到数据库
                db.SaveChanges();

                //扣除总帐
                MetaValueHelper.SetGeneralLedger(_GeneralLedger - form.Cost);

                //发送邮件
                var user = db.Users.FirstOrDefault(x => x.ID == form.CreateBy);
                if (user != null && !string.IsNullOrWhiteSpace(user.EMail))
                {
                    string Subject = "经费申请已转帐--" + form.Title.Trim(),
                        Content = @"{0} 您好：
                                            你的经费申请单已经转帐。请您留意到帐情况。
                                            单号：{1}
                                            标题:{2}";
                    Content = string.Format(Content, user.Name.Trim(), form.ID, form.Title);
                    MailHelpers.SendMail(user.EMail, Subject, Content);
                }

                return RedirectToAction("Appling");
            }
        }
        #endregion
    }
}