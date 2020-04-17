using TranslateSQL;
using QueryExecution;
using System.Collections.Generic;

namespace QueryResult
{
    public static class QueryResultCreator
    {
        public static string CreateQueryResult(string question)
        {
            var query = SQLQueryBuilder.Build(question);
            var result = new QueryExecutor(ConnectionHelper.ConnectionString).ExecuteQuery(query);

            return result;
        }
    }
}
