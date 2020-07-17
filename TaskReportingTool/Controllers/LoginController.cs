using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaskReportingTool.Models;

namespace TaskReportingTool.Controllers
{
    public class LoginController : Controller
    {
        public SqlConnection con;
        public SqlCommand com;
        public SqlDataReader dr;
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]

        public ActionResult Verify(Login login)
        {

            dbmanage db = new dbmanage();
            using (con = db.newconection())
            {
                using (com = new SqlCommand())
                {
                    con.Open();
                    com.Connection = con;
                    com.CommandText = "SELECT * FROM employee WHERE Username = '" + login.username + "' AND Passwords = '" + login.password + "'";

                    dr = com.ExecuteReader();
                    if (dr.Read())
                    {


                        Session["Name"] = dr["Fname"].ToString() + " " + dr["Lname"].ToString();
                        Session["Username"] = dr["Username"].ToString();
                        Session["Department"] = dr["Department"].ToString();
                        Session["Email"] = dr["Email"].ToString();
                        Session["EmployeeId"] = dr["EmployeeId"];
                        Session["Password"] = dr["Passwords"].ToString();
                        Session["Roles"] = dr["Roles"].ToString();
                        Session["CreDate"] = dr["CreDate"].ToString();
                        con.Close();
                        return RedirectToAction("Dashboard", "Home");
                    }
                    else
                    {
                        con.Close();
                        ViewBag.error = "UPW";
                        return View("Index");
                    }
                }
            }
        }
        [HttpGet]
        public ActionResult Logout()
        {
            Session.RemoveAll();
            Session.Abandon();
            ViewBag.error = "Logout";
            return View("Index");
        }

    }
}