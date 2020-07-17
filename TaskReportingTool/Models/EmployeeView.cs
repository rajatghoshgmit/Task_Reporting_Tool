using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace TaskReportingTool.Models
{
    public class EmployeeView
    {
        public int EmployeeId { get; set; }
        public string Username { get; set; }
        public string Fname { get; set; }
        public string Lname { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public string Roles { get; set; }
        public string CreDate { get; set; }
        public string Email { get; set; }
        public string Passwords { get; set; }
        public string Status { get; set; }

    }
}