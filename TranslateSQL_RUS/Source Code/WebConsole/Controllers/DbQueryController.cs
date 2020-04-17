using System.Web.Mvc;
using QueryResult;

namespace WebConsole.Controllers
{
    public class DbQueryController : Controller
    {
        public ActionResult DbQuery()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DbQuery(string question)
        {
            var result = QueryResultCreator.CreateQueryResult(question);

            if (string.IsNullOrEmpty(result))
                ViewBag.Result = "В базе нет данных";
            else
                ViewBag.Result = result;


            return View("Result");
        }
    }
}
