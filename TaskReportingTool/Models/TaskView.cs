using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaskReportingTool.Models
{
    public class TaskView
    {
        public int TaskId { get; set; }
        public string TaskName { get; set; }
        public string SDate { get; set; }
        public string EDate { get; set; }
        public string Resource { get; set; }
        public int RPerson { get; set; }
        public int ProjectManager { get; set; }
        public string Status { get; set; }
        public string Department { get; set; }
        public string RNPerson { get; set; }
        public string ProjectManagerName { get; set; }
        public List<string> ResourceName { get; set; }
    }
}