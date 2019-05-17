using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WFS.Models
{
    public class FormPrintModel
    {
        public string ID { get; set; }
        public string DeptName { get; set; }
        public string User { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string CostChinese { get; set; }
        public decimal Cost { get; set; }
        public string P1 { get; set; }
        public string P2 { get; set; }
        public string P3 { get; set; }
        public string P4 { get; set; }
    }
}