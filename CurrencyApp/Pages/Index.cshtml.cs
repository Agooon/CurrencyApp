using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CurrencyApp.Backend;
using CurrencyApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CurrencyApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private int counter = 0;
        public TableAModel TableA { get; set; }
        public string ErrorString { get; set; }
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
                ErrorString = $"Error has accured: {ex.Message}";
                return null;
            }

        }

        public async Task OnGetAsync(string date = "")
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
                ErrorString += "\nNie odnaleziono strony z tabelami lub przekroczono ilość zapytań. Sprawdź działanie strony NBP.";
                return;
            }
            TableA = call.First();
            ErrorString = null;
        }
    }
}
