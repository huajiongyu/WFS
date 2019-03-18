using AutoMapper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WFS.Models;

namespace WFS.Controllers
{
    public class ProposerController : Controller
    {
        #region 显示（查询）
        // GET: Treasury
        public ActionResult Index()
        {
            var file = Request.Files[0];
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
                    //var user = db.Users.FirstOrDefault(x => x.ID.Trim() == id.Trim());
                    //var model = Mapper.Map<UserViewModel>(user);
                    //model.PasswordConfirm = model.Password;
                    return View();
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
        [HttpPost]
        public ActionResult Delete(string id)
        {
            using (var db = new WFSContext())
            {
                id = id ?? "";//防止传空指针
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
                var _maxForm = db.Form.Where(x => x.ID.Contains(preFix))
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

        /// <summary>
        /// 保存上传的附件
        /// </summary>
        /// <param name="file">文件流</param>
        /// <returns>文件id</returns>
        private string SaveFile(HttpPostedFileBase file)
        {
            var fid = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var fullname = Path.Combine(Server.MapPath("~/Upload/"), fid);
            file.SaveAs(fullname);
            return fid;
        }

        private bool DelFile(string FileID)
        {
            try
            {
                var fullname = Path.Combine(Server.MapPath("~/Upload/"), FileID);
                System.IO.File.Delete(fullname);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion
    }
}