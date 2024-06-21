using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public static class AppServices
    {
        private static readonly string connectionString = "Host=192.168.100.5;Username=postgres;Password=root;Database=Library";
        private static readonly DataBaseService dbHelper = new DataBaseService(connectionString);

        public static DataBaseService DbHelper => dbHelper;
        public static string AccountEmail;
    }
}
