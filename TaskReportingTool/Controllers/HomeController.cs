 using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaskReportingTool.Models;

namespace TaskReportingTool.Controllers
{
    public class HomeController : Controller
    {
        public SqlConnection con;
        public SqlCommand com;
        public SqlDataReader dr;
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Dashboard()
        {
            if (Session["EmployeeId"] == null)
            {
               return RedirectToAction("Index", "Login");
            }
            return View("Index");
        }
        public ActionResult AddReport()
        {
            if (Session["EmployeeId"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            return View(PendingTaskList(Session["Department"].ToString(), Session["EmployeeId"].ToString()));
        }
        public ActionResult PendingTask()
        {
            if (Session["EmployeeId"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            return View(PendingTaskList(Session["Department"].ToString(), Session["EmployeeId"].ToString()));
        }
        public ActionResult TotalTask()
        {
            if (Session["EmployeeId"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            return View(TotalTaskList(Session["Department"].ToString(), Session["EmployeeId"].ToString()));
        }
        public ActionResult ModifyReport()
        {
            if (Session["EmployeeId"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            ViewBag.firstdate = GetFirstWeekday(1, int.Parse(DateTime.Now.ToString("MM")), int.Parse(DateTime.Now.ToString("yyyy"))).ToString("dd-MM-yyyy"); 
            ViewBag.task = PendingTaskList(Session["Department"].ToString(), Session["EmployeeId"].ToString());
            return View(GetReports(int.Parse(Session["EmployeeId"].ToString())));
        }

        public ActionResult Reports(ReportView RV)
        {
            string[] s = RV.TaskName.Split('-');
            if (RV.TaskName == "None")
            {

            }
            else
            {
                RV.TaskId = Int32.Parse(s[1]);
                RV.TaskName = s[0];
                RV.EmployeeId = Int32.Parse(Session["EmployeeId"].ToString());
                RV.EmployeeName = Session["Name"].ToString();
                dbmanage db = new dbmanage();
                using (con = db.newconection())
                {
                    using (com = new SqlCommand())
                    {

                        con.Open();
                        com.Connection = con;
                        if (RV.bto == "Delete")
                        {
                            com.CommandText = "DELETE FROM Report WHERE Id='" + RV.Id + "'";
                            dr = com.ExecuteReader();
                        }
                        else
                        {
                            if (RV.Id == 0)
                            {
                                com.CommandText = "INSERT INTO Report (EmployeeName, Dates, TaskName, EmployeeId, TaskId) VALUES('" + RV.EmployeeName + "', '" + RV.Dates.ToShortDateString() + "', '" + RV.TaskName + "', '" + RV.EmployeeId + "', '" + RV.TaskId + "' ) ";
                                dr = com.ExecuteReader();
                            }
                            else
                            {
                                com.CommandText = "UPDATE Report SET TaskName='" + RV.TaskName + "', TaskId='" + RV.TaskId + "' WHERE Id='" + RV.Id + "'";
                                dr = com.ExecuteReader();
                            }
                        }

                        con.Close();
                        
                    }
                }
            }
            return RedirectToAction("ModifyReport", "Home");
        }




        public List<TaskView> PendingTaskList( string dept, string id)
        {
            List<TaskView> Tasklist = new List<TaskView>();
            
            dbmanage db = new dbmanage();
            using (con = db.newconection())
            {
                using (com = new SqlCommand())
                {
                    con.Open();
                    com.Connection = con;
                    com.CommandText = "SELECT * FROM Task WHERE Department= '"+dept+ "'AND NOT Status='100' AND Resource LIKE '%E-" + id+"%'";
                    using (var reader = com.ExecuteReader())
                    {
                       
                        while (reader.Read())
                        {
                            List<string> name = new List<string>();
                            String[] strlist = reader["Resource"].ToString().Split(',');
                            foreach (string s in strlist) 
                            {
                                string s1= s.Replace(" ", "");
                                s1 = s1.Replace("E-", "");
                                name.Add(EmployeName(int.Parse(s1)));
                            }
                            Tasklist.Add(new TaskView {
                                TaskId = int.Parse(reader["TaskId"].ToString()),
                                TaskName = reader["TaskName"].ToString(),
                                SDate = reader["SDate"].ToString(),
                                EDate = reader["EDate"].ToString(),
                                Resource = reader["Resource"].ToString(),
                                RPerson = int.Parse(reader["RPerson"].ToString()),
                                ProjectManager = int.Parse(reader["ProjectManager"].ToString()),
                                Status = reader["Status"].ToString(),
                                Department = reader["Department"].ToString(),
                                RNPerson = EmployeName(int.Parse(reader["RPerson"].ToString())),
                                ProjectManagerName = EmployeName(int.Parse(reader["ProjectManager"].ToString())),
                                ResourceName = name
                        });
                        }
                        reader.Close();
                    }
                }
            }
            return Tasklist;
        }
        public List<TaskView> TotalTaskList(string dept, string id)
        {
            List<TaskView> Tasklist = new List<TaskView>();
            dbmanage db = new dbmanage();
            using (con = db.newconection())
            {
                using (com = new SqlCommand())
                {
                    con.Open();
                    com.Connection = con;
                    com.CommandText = "SELECT * FROM Task WHERE Department= '" + dept + "'AND Resource LIKE '%E-" + id + "%'";
                    using (var reader = com.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            List<string> name = new List<string>();
                            String[] strlist = reader["Resource"].ToString().Split(',');
                            foreach (string s in strlist)
                            {
                                string s1 = s.Replace(" ", "");
                                s1 = s1.Replace("E-", "");
                                name.Add(EmployeName(int.Parse(s1)));
                            }
                            Tasklist.Add(new TaskView
                            {
                                TaskId = int.Parse(reader["TaskId"].ToString()),
                                TaskName = reader["TaskName"].ToString(),
                                SDate = reader["SDate"].ToString(),
                                EDate = reader["EDate"].ToString(),
                                Resource = reader["Resource"].ToString(),
                                RPerson = int.Parse(reader["RPerson"].ToString()),
                                ProjectManager = int.Parse(reader["ProjectManager"].ToString()),
                                Status = reader["Status"].ToString(),
                                Department = reader["Department"].ToString(),
                                RNPerson = EmployeName(int.Parse(reader["RPerson"].ToString())),
                                ProjectManagerName = EmployeName(int.Parse(reader["ProjectManager"].ToString())),
                                ResourceName = name
                            });
                        }
                        reader.Close();
                    }
                }
            }
            return Tasklist;
        }
        public string EmployeName (int id)
        {
            dbmanage db = new dbmanage();
            using (con = db.newconection())
            {
                using (com = new SqlCommand())
                {
                    con.Open();
                    com.Connection = con;
                    com.CommandText = "SELECT * FROM employee WHERE EmployeeId= '" + id + "' ";
                    using (var dr = com.ExecuteReader())
                    {
                        string s = " ";
                        if (dr.Read()) {
                            s = dr["Fname"].ToString() + " " + dr["Lname"].ToString();
                            dr.Close();
                        }
                        return s;
                    }
                }


                
            }
        }
        public List<ReportView> GetReports(int eid)
        {
            List<ReportView> Report = new List<ReportView>();
            string MN = DateTime.Now.ToString("MM");
            dbmanage db = new dbmanage();
            using (con = db.newconection())
            {
                using (com = new SqlCommand())
                {
                    con.Open();
                    com.Connection = con;
                    com.CommandText = "SELECT * FROM Report WHERE EmployeeId= '" + eid + "'AND Dates LIKE '__-" + MN + "%' ORDER BY Dates ASC";
                    using (var reader = com.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Report.Add(new ReportView
                            {
                                Id = int.Parse(reader["Id"].ToString()),
                                EmployeeName = reader["EmployeeName"].ToString(),
                                EmployeeId = int.Parse(reader["EmployeeId"].ToString()),
                                TaskId = int.Parse(reader["TaskId"].ToString()),
                                TaskName = reader["TaskName"].ToString(),
                                Dates = Convert.ToDateTime(reader["Dates"].ToString())
                            });
                        }
                    }
                }
            }
            return Report;
        }
        public DateTime GetFirstWeekday (int day, int month, int year)
        {
            DateTime dateValue = new DateTime(year, month, day);
            int daycount = 0;
            switch (dateValue.ToString("dddd"))
            {
                case "Monday":
                    daycount = -1;
                    break;
                case "Tuesday":
                    daycount = -2;
                    break;
                case "Wednesday":
                    daycount = -3;
                    break;
                case "Thursday":
                    daycount = -4;
                    break;
                case "Friday":
                    daycount = -5;
                    break;
                case "Saturday":
                    daycount = -6;
                    break;
                default:
                    break;
            }

            return dateValue.AddDays(daycount); 
        }

    }
}