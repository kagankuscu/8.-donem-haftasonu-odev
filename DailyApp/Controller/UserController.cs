using DailyApp.Db;
using DailyApp.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyApp.Controller
{
    public static class UserController
    {
        public static bool Login(User user)
        {
            SqlConnection conn = DB.Conn();
            SqlCommand cmd = new SqlCommand("SELECT Username, Password FROM Users WHERE Username=@username AND Password=@password AND IsActive=1 AND IsDeleted=0", conn);
            cmd.Parameters.AddWithValue("username", user.Username);
            cmd.Parameters.AddWithValue("password", user.Password);

            conn.Open();
            object loginUser = cmd.ExecuteScalar();
            conn.Close();
            return loginUser != null;
        }

    }
}
