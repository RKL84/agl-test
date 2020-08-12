using Agl.Core.Configuration;
using Agl.Core.Model;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Agl.Core.Infrastructure.Services
{
    public class PeopleService : IPeopleService
    {
        private AppSettings _appSettings;
        private ILogger<IPeopleService> _logger;
        private readonly HttpClient _httpClient;

        public PeopleService(HttpClient httpClient,
            AppSettings appSettings, ILogger<IPeopleService> logger)
        {
            _appSettings = appSettings;
            _logger = logger;
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(_appSettings.PeopleServiceClientConfig.Url);
        }

        public async Task<IEnumerable<Person>> FetchAllAsync()
        {
            var result = new List<Person>();
            var uri = _appSettings.PeopleServiceClientConfig.Endpoint;
            _logger.LogDebug("sending request to fetch the data from people service");

            try
            {
                var response = await _httpClient.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogDebug("received a success response");
                    var content = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<List<Person>>(content);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occured when fetching the records from the endpoint '{uri}'");
                throw ex;
            }

            return result;
        }
    }
}
