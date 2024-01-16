using DailyApp.Db;
using DailyApp.View;
using System.Data.SqlClient;

namespace DailyApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Menu.Index();
            }
        }
    }
}
