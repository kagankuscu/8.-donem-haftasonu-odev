using DailyApp.Db;
using DailyApp.Model;
using DailyApp.Utils;
using System.Data.SqlClient;

namespace DailyApp.Controller
{
    public static class DiaryController
    {
        public static bool Add(Diary daily)
        {
            SqlConnection conn = DB.Conn();
            SqlCommand cmd = new SqlCommand("INSERT INTO Diaries (Name, DateCreated, DateModified, IsActive, IsDeleted, UserId) VALUES (@name, @dateCreated, @dateModified, @isActive, @isDeleted, @userId)", conn);
            cmd.Parameters.AddWithValue("name", Helper.Base64Encode(daily.Name));
            cmd.Parameters.AddWithValue("userId", daily.UserId);
            cmd.Parameters.AddWithValue("dateCreated", daily.DateCreated);
            cmd.Parameters.AddWithValue("dateModified", daily.DateModified);
            cmd.Parameters.AddWithValue("isActive", daily.IsActive);
            cmd.Parameters.AddWithValue("isDeleted", daily.IsDelete);
            
            conn.Open();
            int affectedRows = cmd.ExecuteNonQuery();
            conn.Close();

            return affectedRows > 0;
        }
        public static List<Diary> GetAll(int userId)
        {
            SqlConnection conn = DB.Conn();
            SqlCommand cmd = new SqlCommand("SELECT Id, Name, DateCreated, DateModified FROM Diaries WHERE IsActive = 1 AND IsDeleted = 0 AND UserId = @userId", conn);
            cmd.Parameters.AddWithValue("userId", userId);

            conn.Open();

            SqlDataReader dr = cmd.ExecuteReader();
            List<Diary> list = new List<Diary>();

            while(dr.Read())
            {
                list.Add(new Diary 
                { 
                    Id = (int)dr["Id"],
                    Name = Helper.Base64Decode((string)dr["Name"]),
                    DateCreated = (DateTime)dr["DateCreated"]
                });
            }
            dr.Close();
            conn.Close();

            return list;
        }
        public static bool RemoveAll(int userId)
        {
            SqlConnection conn = DB.Conn();
            SqlCommand cmd = new SqlCommand("UPDATE Diaries SET IsActive=0, IsDeleted=1, DateModified=@date WHERE UserId = @userId", conn);
            cmd.Parameters.AddWithValue("date", DateTime.Now);
            cmd.Parameters.AddWithValue("userId", userId);

            conn.Open();
            int affectedRows = cmd.ExecuteNonQuery();
            conn.Close();

            return affectedRows > 0;
        }
        public static bool Update(Diary diary)
        {
            SqlConnection conn = DB.Conn();
            diary.DateModified = DateTime.Now;
            SqlCommand cmd = new SqlCommand("UPDATE Diaries SET DateModified=@date, Name=@name WHERE Id=@id AND UserId = @userId", conn);
            cmd.Parameters.AddWithValue("id", diary.Id);
            cmd.Parameters.AddWithValue("date", diary.DateModified);
            cmd.Parameters.AddWithValue("name", Helper.Base64Encode(diary.Name));
            cmd.Parameters.AddWithValue("userId", diary.UserId);

            conn.Open();
            int affectedRows = cmd.ExecuteNonQuery();
            conn.Close();
            return affectedRows > 0;
        }
        public static bool RemoveById(int id, int userId)
        {
            SqlConnection conn = DB.Conn();
            SqlCommand cmd = new SqlCommand("UPDATE Diaries SET IsActive=0, IsDeleted=1, DateModified=@date WHERE Id = @id AND UserId = @userid", conn);
            cmd.Parameters.AddWithValue("id", id);
            cmd.Parameters.AddWithValue("date", DateTime.Now);
            cmd.Parameters.AddWithValue("userId", userId);
            conn.Open();
            int affectedRows = cmd.ExecuteNonQuery();
            conn.Close();
            return affectedRows > 0;
        }
        public static bool CheckCurrentDateHasDiary(int userId)
        {
            SqlConnection conn = DB.Conn();
            SqlCommand cmd = new SqlCommand("SELECT TOP(1) Id FROM Diaries WHERE YEAR(DateCreated)=YEAR(GETDATE()) AND MONTH(DateCreated)=MONTH(GETDATE()) AND DAY(DateCreated) = DAY(GETDATE()) AND IsActive=1 AND IsDeleted = 0 AND UserId = @userid", conn);
            cmd.Parameters.AddWithValue("userId", userId);
            conn.Open();
            int affectedRows = cmd.ExecuteScalar() == null ? 0 : (int)cmd.ExecuteScalar();
            conn.Close();

            return affectedRows > 0;
        }
        public static List<Diary> GetDiariesByDate(DateTime date, int userId)
        {
            SqlConnection conn = DB.Conn();
            SqlCommand cmd = new SqlCommand("SELECT Id, Name, DateCreated FROM Diaries WHERE YEAR(DateCreated)=@year AND MONTH(DateCreated)=@month AND DAY(DateCreated)=@day AND IsActive=1 AND IsDeleted = 0 AND UserId = @userid", conn);
            cmd.Parameters.AddWithValue("year", date.Year);
            cmd.Parameters.AddWithValue("month", date.Month);
            cmd.Parameters.AddWithValue("day", date.Day);
            cmd.Parameters.AddWithValue("userId", userId);

            List<Diary> diaries = new List<Diary>();

            conn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                diaries.Add(new Diary
                {
                    Id = (int)dr["Id"],
                    Name= Helper.Base64Decode((string)dr["Name"]),
                    DateCreated= (DateTime)dr["DateCreated"],
                });
            }
            dr.Close();
            conn.Close();
            return diaries;
        }
    }
}
