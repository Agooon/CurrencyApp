﻿using CurrencyApp.Backend;
using CurrencyApp.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace CurrencyApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        // To show only the most important ones 
        // Now it's gotten from configuration, but in future version
        // A user will be define his own list with order
        public string[] MainOnes { get; set; }
        private int counter = 0;
        public TableAModel TableA { get; set; }
        public string ErrorString { get; set; }
        public bool MainOnesCK { get; set; }
        public string Date { get; set; } = "";
        public IndexModel(IHttpClientFactory clientFactory, IConfiguration iConfig, ILogger<IndexModel> logger)
        {
            _clientFactory = clientFactory;
            _configuration = iConfig;
            _logger = logger;
        }

        private async Task<List<TableAModel>> GetCorrectCall(HttpClient client, string date)
        {
            try
            {
                return await client.GetFromJsonAsync<List<TableAModel>>($"tables/A/{date}?format=json");
            }
            catch (Exception ex)
            {
                ErrorString = $"Error has accured:</br>{ex.Message}</br>";
                return null;
            }

        }

        public async Task OnGetAsync(bool mainOnesCK, string date = "")
        {
            // date = RRRR-MM-DD
            var maxNumberOfCalls = _configuration.GetValue<int>("MaxNumberOfCalls");
            var client = _clientFactory.CreateClient("nbp");
            Date = date;

            if (date != "" && !StaticFunctions.IsValidDate(date, "yyyy-MM-dd"))
            {
                ErrorString = "Nieprawidłowy format daty, należy podać format: RRRR-MM-DD (np. 2020-11-20)";
                return;
            }
            var call = await GetCorrectCall(client, date);
            while (call == null && counter < maxNumberOfCalls)
            {
                StaticFunctions.PreviousDay(ref date, "yyyy-MM-dd");
                call = await GetCorrectCall(client, date);
                counter++;
            }
            counter = 0;
            if (call == null)
            {
                ErrorString += "</br>Nie odnaleziono strony z tabelami lub przekroczono ilość zapytań. Sprawdź działanie strony NBP.";
                return;
            }
            TableA = call.First();
            MainOnesCK = mainOnesCK;
            MainOnes = _configuration.GetSection("Currencies").Get<string[]>();
            ErrorString = null;
        }
    }
}
