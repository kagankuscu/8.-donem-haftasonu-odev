using DailyApp.Db;
using DailyApp.Model;
using System.Data.SqlClient;

namespace DailyApp.Controller
{
    public static class DiaryController
    {
        public static bool Add(Diary daily)
        {
            SqlConnection conn = DB.Conn();
            SqlCommand cmd = new SqlCommand("INSERT INTO Diary (Name, DateCreated, DateModified, IsActive, IsDeleted) VALUES (@name, @dateCreated, @dateModified, @isActive, @isDeleted)", conn);
            cmd.Parameters.AddWithValue("name", daily.Name);
            cmd.Parameters.AddWithValue("dateCreated", daily.DateCreated);
            cmd.Parameters.AddWithValue("dateModified", daily.DateModified);
            cmd.Parameters.AddWithValue("isActive", daily.IsActive);
            cmd.Parameters.AddWithValue("isDeleted", daily.IsDelete);

            conn.Open();
            int affectedRows = cmd.ExecuteNonQuery();
            conn.Close();

            return affectedRows > 0;
        }
        public static List<Diary> GetAll()
        {
            SqlConnection conn = DB.Conn();
            SqlCommand cmd = new SqlCommand("SELECT Id, Name, DateCreated, DateModified FROM Diary WHERE IsActive = 1 AND IsDeleted = 0", conn);

            conn.Open();

            SqlDataReader dr = cmd.ExecuteReader();
            List<Diary> list = new List<Diary>();

            while(dr.Read())
            {
                list.Add(new Diary 
                { 
                    Id = (int)dr["Id"],
                    Name = (string)dr["Name"],
                    DateCreated = (DateTime)dr["DateCreated"]
                });
            }
            dr.Close();
            conn.Close();

            return list;
        }
        public static bool RemoveAll()
        {
            SqlConnection conn = DB.Conn();
            SqlCommand cmd = new SqlCommand("UPDATE Diary SET IsActive=0, IsDeleted=1", conn);

            conn.Open();
            int affectedRows = cmd.ExecuteNonQuery();
            conn.Close();

            return affectedRows > 0;
        }
        public static bool CheckCurrentDateHasDiary()
        {
            SqlConnection conn = DB.Conn();
            SqlCommand cmd = new SqlCommand("SELECT TOP(1) Id FROM Diary WHERE YEAR(DateCreated)=YEAR(GETDATE()) AND MONTH(DateCreated)=MONTH(GETDATE()) AND DAY(DateCreated) = DAY(GETDATE()) AND IsActive=1 AND IsDeleted = 0", conn);
            conn.Open();
            int affectedRows = cmd.ExecuteScalar() == null ? 0 : (int)cmd.ExecuteScalar();
            conn.Close();

            return affectedRows > 0;
        }
    }
}
