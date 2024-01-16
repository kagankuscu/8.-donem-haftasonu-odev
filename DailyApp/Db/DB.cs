using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyApp.Db
{
    public static class DB
    {
        public static SqlConnection Conn() => new SqlConnection("Server=localhost;Database=DailyDB;Integrated Security=true;TrustServerCertificate=true;");
    }
}
