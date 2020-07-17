using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaskReportingTool.Models;

namespace TaskReportingTool.Controllers
{
    public class AdminController : Controller
    {
        public ActionResult Index()
        {
            if (Session["EmployeeId"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            return View();
        }
        public SqlConnection con;
        public SqlCommand com;
        public SqlDataReader dr;
        
        //GET EMPLOYEE
        public ActionResult AddUser()
        {
            if (Session["EmployeeId"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            return View();
        }
        [HttpPost]
        public ActionResult SaveEmployee(EmployeeView EV)
        {
            EV.Passwords = PasswordGenerator(10).ToString();
            EV.Status = "ADD";
            dbmanage db = new dbmanage();
            using (con = db.newconection())
            {
                using (com = new SqlCommand())
                {

                    con.Open();
                    com.Connection = con;
                    com.CommandText = "INSERT INTO employee (Username, Fname, Lname, Department, Roles, CreDate, Email, Passwords, Status) VALUES('" + EV.Username + "', '" + EV.Fname + "', '" + EV.Lname + "', '" + EV.Department + "','" + EV.Roles + "','" + EV.CreDate + "','" + EV.Email + "','" + EV.Passwords + "','" + EV.Status + "' ) ";
                    dr = com.ExecuteReader();
                    con.Close();
                }
            }
            return View("Index");

        }

        [HttpGet]
        public ActionResult EditEmployee(int id)
        {
            dbmanage db = new dbmanage();
            using (con = db.newconection())
            {
                using (com = new SqlCommand())
                {
                    con.Open();
                    com.Connection = con;
                    com.CommandText = "SELECT * FROM employee WHERE EmployeeId='" + id + "'";
                    using (var reader = com.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            ViewBag.EmployeeId = int.Parse(reader["EmployeeId"].ToString());
                            ViewBag.Username = reader["Username"].ToString();
                            ViewBag.Fname = reader["Fname"].ToString();
                            ViewBag.Lname = reader["Lname"].ToString();
                            ViewBag.Department = reader["Department"].ToString();
                            ViewBag.Roles = reader["Roles"].ToString();
                            ViewBag.CreDate = reader["CreDate"].ToString();
                            ViewBag.Email = reader["Email"].ToString();
                        }
                    }
                    con.Close();
                }
            }

            return View("AddUser");
        }
        public ActionResult ModifyEmployee(EmployeeView EV)
        {
            dbmanage db = new dbmanage();
            using (con = db.newconection())
            {
                using (com = new SqlCommand())
                {

                    con.Open();
                    com.Connection = con;
                    com.CommandText = "UPDATE employee SET Roles= '" +EV.Roles+ "', Email= '"+EV.Email+ "' WHERE EmployeeId='" +EV.EmployeeId+ "'";
                    dr = com.ExecuteReader();
                    con.Close();
                }
            }
            return View("ModifyUser");
        }
        [HttpGet]
        public ActionResult DeleteUser(int id)
        {
            dbmanage db = new dbmanage();
            using (con = db.newconection())
            {
                using (com = new SqlCommand())
                {

                    con.Open();
                    com.Connection = con;
                    com.CommandText = "DELETE FROM employee WHERE EmployeeId='" +id+ "'";
                    dr = com.ExecuteReader();
                    con.Close();
                }
            }
            return View("ModifyUser");
        }
        [HttpGet]
        public ActionResult ModifyUser()
        {
            if (Session["EmployeeId"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            return View();
        }
        public PartialViewResult GetUserDetails()
        {
            return PartialView(EmployeeDetails());
        }

        public string PasswordGenerator(int length)
        {
            string totallist = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz~!@#$%^&*_-+=.,?*0123456789";
            Random random = new Random();
            char[] Password = new char[length];
            for (int i = 0; i < length; i++)
            {
                Password[i] = totallist[random.Next(0, totallist.Length)];
            }
            return new string(Password);
        }
        public List<EmployeeView> EmployeeDetails()
        {
            List<EmployeeView> Employeelist = new List<EmployeeView>();
            dbmanage db = new dbmanage();
            using (con = db.newconection())
            {
                using (com = new SqlCommand())
                {
                    con.Open();
                    com.Connection = con;
                    com.CommandText = "SELECT * FROM employee WHERE NOT Fname='Admin' ORDER BY EmployeeId DESC";
                    using (var reader = com.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Employeelist.Add(new EmployeeView
                            {
                                EmployeeId = int.Parse(reader["EmployeeId"].ToString()),
                                Username = reader["Username"].ToString(),
                                Fname = reader["Fname"].ToString(),
                                Lname = reader["Lname"].ToString(),
                                Name = reader["Fname"].ToString() + ' ' + reader["Lname"].ToString(),
                                Department = reader["Department"].ToString(),
                                Roles = reader["Roles"].ToString(),
                                CreDate =reader["CreDate"].ToString(),
                                Email = reader["Email"].ToString()
                            });
                        }
                    }
                }
            }
            return Employeelist;
        }
    }
    

}