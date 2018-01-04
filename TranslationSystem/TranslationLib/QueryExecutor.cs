using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranslationLib
{
    class QueryExecutor
    {
       public struct Query
        {
            public string select;
            public string from;
            public string where;
            public string group;
            public string having;
        }

        public QueryExecutor(string SqlQuery)
        {

        }

        public void ExecuteQuery(Query sql)
        {

        }
    }
}
