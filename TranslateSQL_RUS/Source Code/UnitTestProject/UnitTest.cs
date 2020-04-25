using TranslateSQL;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            string testQuestion = "Адрес Нижнего парка";
            string expectedResult = "SELECT ADDRESSES.STREET, ADDRESSES.HOUSENUMBER, AREAS.NAME FROM AREAS join ADDRESSES on AREAS.ID = ADDRESSES.ID_AREA join PARKS on ADDRESSES.ID = PARKS.ID_ADDRESS WHERE PARKS.NAME like '%Нижний%'";

            string actualResult = SQLQueryBuilder.Build(testQuestion);

            Assert.AreEqual(expectedResult, actualResult);

            string sqlResult = new QueryExecution.QueryExecutor(ConnectionHelper.ConnectionString).ExecuteQuery(actualResult);
            Assert.IsFalse(sqlResult.Contains("error"));
        }

        [TestMethod]
        public void TestMethod2()
        {
            string testQuestion = "В каком районе находится памятник Ленину";
            string expectedResult = "SELECT AREAS.NAME FROM AREAS join ADDRESSES on AREAS.ID = ADDRESSES.ID_AREA join MONUMENTS on ADDRESSES.ID = MONUMENTS.ID_ADDRESS WHERE MONUMENTS.NAME like '%Ленину%'";
            string actualResult = SQLQueryBuilder.Build(testQuestion);

            Assert.AreEqual(expectedResult, actualResult);

            string sqlResult = new QueryExecution.QueryExecutor(ConnectionHelper.ConnectionString).ExecuteQuery(actualResult);
            Assert.IsFalse(sqlResult.Contains("error"));
        }

        [TestMethod]
        public void TestMethod3()
        {
            string testQuestion = "Где находится Вагон-музей";
            string expectedResult = "SELECT ADDRESSES.STREET, ADDRESSES.HOUSENUMBER, AREAS.NAME FROM AREAS join ADDRESSES on AREAS.ID = ADDRESSES.ID_AREA join MUSEUMS on ADDRESSES.ID = MUSEUMS.ID_ADDRESS WHERE MUSEUMS.DESCRIPTION like '%Вагон-музей%'";
            string actualResult = SQLQueryBuilder.Build(testQuestion);

            Assert.AreEqual(expectedResult, actualResult);

            string sqlResult = new QueryExecution.QueryExecutor(ConnectionHelper.ConnectionString).ExecuteQuery(actualResult);
            Assert.IsFalse(sqlResult.Contains("error"));
        }

        [TestMethod]
        public void TestMethod4()
        {
            string testQuestion = "Где находится Английский парк";
            string expectedResult = "SELECT ADDRESSES.STREET, ADDRESSES.HOUSENUMBER, AREAS.NAME FROM AREAS join ADDRESSES on AREAS.ID = ADDRESSES.ID_AREA join PARKS on ADDRESSES.ID = PARKS.ID_ADDRESS WHERE PARKS.NAME like '%Английский%'";
            string actualResult = SQLQueryBuilder.Build(testQuestion);

            Assert.AreEqual(expectedResult, actualResult);

            string sqlResult = new QueryExecution.QueryExecutor(ConnectionHelper.ConnectionString).ExecuteQuery(actualResult);
            Assert.IsFalse(sqlResult.Contains("error"));
        }

        [TestMethod]
        public void TestMethod5()
        {
            string testQuestion = "Какие музеи находятся на ул Садовая";
            string expectedResult = "SELECT MUSEUMS.* FROM ADDRESSES join MUSEUMS on ADDRESSES.ID = MUSEUMS.ID_ADDRESS WHERE ADDRESSES.STREET like '%Садовая%'";
            string actualResult = SQLQueryBuilder.Build(testQuestion);

            Assert.AreEqual(expectedResult, actualResult);

            string sqlResult = new QueryExecution.QueryExecutor(ConnectionHelper.ConnectionString).ExecuteQuery(actualResult);
            Assert.IsFalse(sqlResult.Contains("error"));
        }

        [TestMethod]
        public void TestMethod6()
        {
            string testQuestion = "Какие парки находятся в г Петергоф";
            string expectedResult = "SELECT PARKS.* FROM ADDRESSES join PARKS on ADDRESSES.ID = PARKS.ID_ADDRESS WHERE ADDRESSES.STREET like '%Петергоф%'";
            string actualResult = SQLQueryBuilder.Build(testQuestion);

            Assert.AreEqual(expectedResult, actualResult);

            string sqlResult = new QueryExecution.QueryExecutor(ConnectionHelper.ConnectionString).ExecuteQuery(actualResult);
            Assert.IsFalse(sqlResult.Contains("error"));
        }
    }
}
