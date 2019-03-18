﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WFS.Models;

namespace WFS.Controllers
{
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
        public ActionResult Delete(string id)
        {
            return View();
        }
        #endregion
    }
}