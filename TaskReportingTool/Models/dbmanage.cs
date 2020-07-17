using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace TaskReportingTool.Models
{
    public class dbmanage
    {
        public SqlConnection con;
        public SqlCommand com;
        public SqlConnection newconection() 
        {
                con = new SqlConnection();
                con.ConnectionString = @"Data Source=DESKTOP-2FR87TB\SQLEXPRESS;User ID=sa;Password=1234;initial catalog=TaskReportingTool";
                return con;
        }

      
    }
}