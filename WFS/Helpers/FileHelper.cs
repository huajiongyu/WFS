using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace WFS.Helpers
{
    public class FileHelper
    {
        /// <summary>
        /// 保存上传的附件
        /// </summary>
        /// <param name="file">文件流</param>
        /// <returns>文件id</returns>
        public static string SaveFile(HttpPostedFileBase file, HttpServerUtilityBase Server)
        {
            var fid = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var fullname = Path.Combine(Server.MapPath("~/Uploads/"), fid);
            file.SaveAs(fullname);
            return fid;
        }

        public static bool DelFile(string FileID, HttpServerUtilityBase Server)
        {
            try
            {
                var fullname = Path.Combine(Server.MapPath("~/Uploads/"), FileID);
                System.IO.File.Delete(fullname);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}