using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WFS.Models;

namespace WFS.Helpers
{
    /// <summary>
    /// 这是系统表数表的助手类
    /// 这个类封闭一些常用的方法，提高代码重用性
    /// </summary>
    public class MetaValueHelper
    {
        private const string GeneralLedger = "GeneralLedger";

        #region 总帐

        /// <summary>
        /// 直接设定余额
        /// </summary>
        /// <param name="income"></param>
        /// <returns></returns>
        public static bool SetGeneralLedger(decimal income)
        {
            using (var db = new WFSContext())
            {
                var value = db.MetaValues.FirstOrDefault(x => x.MetaID == GeneralLedger);
                if (value == null)//如果未存在，就创建
                {
                    value = InitMeta("GeneralLedger", income.ToString(), "总帐余额");
                    db.MetaValues.Add(value);
                }
                else
                {//如果存在就直接修改数据
                    value.Value = income.ToString();
                }
                db.SaveChanges();
                return true;
            }
        }
        
        #endregion

        #region 初级版本，未用到以下三个方法
        /// <summary>
        /// 总帐收入
        /// </summary>
        /// <param name="income"></param>
        /// <returns></returns>
        public bool Earning(decimal income)
        {
            using (var db = new WFSContext())
            {
                var value = db.MetaValues.FirstOrDefault(x => x.MetaID == GeneralLedger);
                if (value == null)
                {
                    value = InitMeta("GeneralLedger", income.ToString(), "总帐余额");
                    db.MetaValues.Add(value);
                }
                else
                {
                    var _total = decimal.Parse(value.Value);
                    _total += income;
                    value.Value = _total.ToString();
                }
                db.SaveChanges();
                return true;
            }
        }

        /// <summary>
        /// 总帐支出
        /// </summary>
        /// <param name="income"></param>
        /// <returns></returns>
        public static bool Payments(decimal total)
        {
            using (var db = new WFSContext())
            {
                var value = db.MetaValues.FirstOrDefault(x => x.MetaID == GeneralLedger);
                if (value == null)
                {
                    value = InitMeta("GeneralLedger", "0", "总帐余额");
                    db.MetaValues.Add(value);
                    db.SaveChanges();
                    return false;//总帐余额刚创建，余额为0
                }
                else
                {
                    var _total = decimal.Parse(value.Value);
                    _total -= total;
                    //如果总帐余额不够扣，返回扣帐失败
                    if(_total < 0)
                    {
                        return false;
                    }
                    value.Value = _total.ToString();
                }
                
                return true;
            }
        }
        
        /// <summary>
        /// 读取余额
        /// </summary>
        /// <returns></returns>
        public static decimal GetGeneralLedger()
        {
            using (var db = new WFSContext())
            {
                var value = db.MetaValues.FirstOrDefault(x => x.MetaID == GeneralLedger);
                if (value == null)
                {
                    value = InitMeta("GeneralLedger", "0", "总帐余额");
                    db.SaveChanges();
                    return 0;
                }
                else
                {
                    var _total = decimal.Parse(value.Value);
                    return _total;

                }
            }
        }
        #endregion
        /// <summary>
        /// 创建参数
        /// </summary>
        /// <param name="_MetaID"></param>
        /// <param name="_Value"></param>
        /// <param name="_Description"></param>
        /// <returns></returns>
        private static MetaValues InitMeta(string _MetaID, string _Value, string _Description)
        {
            var value = new MetaValues()
            {
                MetaID = _MetaID,
                Value = _Value,
                Desription = _Description
            };
            return value;
        }
    }
}