using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaskReportingTool.Models
{
    public class ReportView
    {
        public int Id { get; set; }
        public string EmployeeName { get; set; }
        public System.DateTime  Dates { get; set; }
        public string TaskName { get; set; }
        public int EmployeeId { get; set; }
        public int TaskId { get; set; }
        public string bto { get; set; }
    }
}