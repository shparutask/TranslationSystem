using TranslateSQL;
using QueryExecution;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QueryResult
{
    public static class QueryResultCreator
    {
        public async static Task<string> CreateQueryResult(string question)
        {
            return await Task.Run(() =>
           {
               var query = SQLQueryBuilder.Build(question);
               var result = new QueryExecutor(ConnectionHelper.ConnectionString).ExecuteQuery(query);

               return result;
           });
        }
    }
}
