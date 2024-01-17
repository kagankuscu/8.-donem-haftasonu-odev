using DailyApp.Db;
using DailyApp.Model;
using DailyApp.Utils;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyApp.Controller
{
    public static class UserController
    {
        public static User Login(User user)
        {
            SqlConnection conn = DB.Conn();
            SqlCommand cmd = new SqlCommand("SELECT Id, Username, Password FROM Users WHERE Username=@username AND Password=@password AND IsActive=1 AND IsDeleted=0", conn);
            cmd.Parameters.AddWithValue("username", user.Username);
            cmd.Parameters.AddWithValue("password", Helper.Base64Encode(user.Password));

            conn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            User logUser = null;
            if(dr.Read())
            {
                logUser = new User()
                {
                    Id= (int)dr["Id"],
                    Username= (string)dr["Username"],
                    Password = Helper.Base64Decode((string)dr["Password"]),
                };
            }
            dr.Close();
            conn.Close();
            return logUser;
        }
        public static bool Register(User user)
        {
            SqlConnection conn = DB.Conn();
            SqlCommand cmd = new SqlCommand("INSERT INTO Users (Fullname, Username, Password, SecurityQuestion, SecurityAnswer, IsActive, IsDeleted, DateCreated, DateModified) VALUES (@fullname, @username, @password, @securityQuestion, @securityAnswer, @isActive, @isDeleted, @dateCreated, @dateModified)", conn);
            cmd.Parameters.AddWithValue("fullname", user.Fullname);
            cmd.Parameters.AddWithValue("username", user.Username);
            cmd.Parameters.AddWithValue("password", Helper.Base64Encode(user.Password));
            cmd.Parameters.AddWithValue("securityQuestion", Helper.Base64Encode(user.SecurityQuestion));
            cmd.Parameters.AddWithValue("securityAnswer", Helper.Base64Encode(user.SecurityAnswer));
            cmd.Parameters.AddWithValue("dateCreated", user.DateCreated);
            cmd.Parameters.AddWithValue("dateModified", user.DateModified);
            cmd.Parameters.AddWithValue("isActive", user.IsActive);
            cmd.Parameters.AddWithValue("isDeleted", user.IsDelete);

            conn.Open();
            int affectedRows = cmd.ExecuteNonQuery();
            conn.Close();

            return affectedRows > 0;
        }
        public static User GetByUsername(string username)
        {
            SqlConnection conn = DB.Conn();
            SqlCommand cmd = new SqlCommand("SELECT Id, Username, SecurityQuestion, SecurityAnswer FROM Users WHERE Username=@username AND IsActive=1 AND IsDeleted=0", conn);
            cmd.Parameters.AddWithValue("username", username);
            conn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            User user = null;
            if (dr.Read())
            {
                user = new User
                {
                    Id = (int)dr["Id"],
                    Username = (string)dr["Username"],
                    SecurityQuestion = Helper.Base64Decode((string)dr["SecurityQuestion"]),
                    SecurityAnswer = Helper.Base64Decode((string)dr["SecurityAnswer"]),
                };
            }
            dr.Close();
            conn.Close();

            return user;
        }
        public static bool ForgetPassword(User user)
        {
            SqlConnection conn = DB.Conn();
            SqlCommand cmd = new SqlCommand("UPDATE Users SET Password = @password WHERE IsActive=1 AND IsDeleted=0 AND Id = @id", conn);
            cmd.Parameters.AddWithValue("password", Helper.Base64Encode(user.Password));
            cmd.Parameters.AddWithValue("id", user.Id);
            conn.Open();
            int affectedRows = cmd.ExecuteNonQuery();
            conn.Close();
            return affectedRows > 0;
        }

    }
}
