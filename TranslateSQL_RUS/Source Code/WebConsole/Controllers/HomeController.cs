﻿using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Typeahead.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Azure.Search;
using System.Collections.Generic;
using System.Linq;
using QueryResult;

namespace WebConsole.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Index(SearchData model)
        {
            try
            {
                // Ensure the search string is valid.
                if (model.searchText != null)
                {
                    // Make the search call for the first page.
                    await RunQueryAsync(model, 0, 0);
                }
            }

            catch
            {
                return View("Error", new ErrorViewModel { RequestId = "1" });
            }

            return View(model);
        }

        public async Task<ActionResult> Page(SearchData model)
        {
            try
            {
                int page;

                switch (model.paging)
                {
                    case "prev":
                        page = (int)TempData["page"] - 1;
                        break;

                    case "next":
                        page = (int)TempData["page"] + 1;
                        break;

                    default:
                        page = int.Parse(model.paging);
                        break;
                }

                // Recover the leftMostPage.
                int leftMostPage = (int)TempData["leftMostPage"];

                // Recover the search text and search for the data for the new page.
                model.searchText = TempData["searchfor"].ToString();

                await RunQueryAsync(model, page, leftMostPage);

                // Ensure Temp data is stored for next call, as TempData only stored for one call.
                TempData["page"] = (object)page;
                TempData["searchfor"] = model.searchText;
                TempData["leftMostPage"] = model.leftMostPage;
            }

            catch
            {
                return View("Error", new ErrorViewModel { RequestId = "2" });
            }
            return View("Index", model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private static SearchServiceClient _serviceClient;
        private static ISearchIndexClient _indexClient;
        private static IConfigurationBuilder _builder;
        private static IConfigurationRoot _configuration;

        private void InitSearch()
        {
            // Create a configuration using the appsettings file.
            _builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
            _configuration = _builder.Build();

            // Pull the values from the appsettings.json file.
            string searchServiceName = _configuration["SearchServiceName"];
            string queryApiKey = _configuration["SearchServiceQueryApiKey"];

            // Create a service and index client.
            _serviceClient = new SearchServiceClient(searchServiceName, new SearchCredentials(queryApiKey));
            _indexClient = _serviceClient.Indexes.GetClient("hotels");
        }

        public async Task<ActionResult> Suggest(string term)
        {
            InitSearch();
            return new JsonResult(await GetProposes(term));
        }

        public async Task<ActionResult> AutoComplete(string term)
        {
            InitSearch();
            return new JsonResult(await GetProposes(term));
        }
        public async Task<ActionResult> AutocompleteAndSuggest(string term)
        {
            InitSearch();
            return new JsonResult(await GetProposes(term));
        }
        private async Task<ActionResult> RunQueryAsync(SearchData model, int page, int leftMostPage)
        {
            InitSearch();

            string result = "";

            if (Dataset.Set.Select(x => x.Question).Contains(model.searchText))
            {
                var query = Dataset.Set.FirstOrDefault(x => x.Question == model.searchText).SQLQuery;
                result = await QueryResultCreator.CreateQueryResultFromSQL(query);
            }
            else
                result = await QueryResultCreator.CreateQueryResult(model.searchText);

            // For efficiency, the search call should be asynchronous, so use SearchAsync rather than Search.
            model.resultList = result.Split('\n').ToList();

            // This variable communicates the total number of pages to the view.
            model.pageCount = (model.resultList.Count + GlobalVariables.ResultsPerPage - 1) / GlobalVariables.ResultsPerPage;

            // This variable communicates the page number being displayed to the view.
            model.currentPage = page;

            // Calculate the range of page numbers to display.
            if (page == 0)
            {
                leftMostPage = 0;
            }
            else
               if (page <= leftMostPage)
            {
                // Trigger a switch to a lower page range.
                leftMostPage = Math.Max(page - GlobalVariables.PageRangeDelta, 0);
            }
            else
            if (page >= leftMostPage + GlobalVariables.MaxPageRange - 1)
            {
                // Trigger a switch to a higher page range.
                leftMostPage = Math.Min(page - GlobalVariables.PageRangeDelta, model.pageCount - GlobalVariables.MaxPageRange);
            }
            model.leftMostPage = leftMostPage;

            // Calculate the number of page numbers to display.
            model.pageRange = Math.Min(model.pageCount - leftMostPage, GlobalVariables.MaxPageRange);

            return View("Index", model);
        }

        private async Task<List<string>> GetProposes(string question)
        {
            return await Task.Run(() =>
            {
                return Dataset.Set
                                .Where(x => x.Question.ToLower().Contains(question.ToLower()))
                                .Select(x => x.Question)
                                .ToList();
            });
        }
    }
}

