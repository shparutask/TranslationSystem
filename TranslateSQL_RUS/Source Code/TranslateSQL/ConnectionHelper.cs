using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranslateSQL
{
    public static class ConnectionHelper
    {
        public static string ConnectionString
        {
            get
            {
                return $@"Data Source = .\{ServerName}; Initial Catalog = {DatabaseName}; Integrated Security = True";
            }
        }

        public static string ServerName
        {
            get
            {
                return "SQLEXPRESS";
            }
        }

        public static string DatabaseName
        {
            get
            {
                return "SPB_ATTRACTIONS";
            }
        }
    }
}
