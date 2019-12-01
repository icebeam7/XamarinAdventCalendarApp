using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Collections.Generic;

using Newtonsoft.Json;

using XamarinAdventCalendarApp.Helpers;

namespace XamarinAdventCalendarApp.Services
{
    public static class DataService<T>
    {
        private static readonly HttpClient client = CreateHttpClient();

        private static HttpClient CreateHttpClient()
        {
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(Constants.ApiBaseURL);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return httpClient;
        }

        public async static Task<List<T>> GetData()
        {
            try
            {
                var response = await client.GetAsync(Constants.JsonBin);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResult = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<List<T>>(jsonResult);
                    return data;
                }
            }
            catch (Exception ex)
            {
            }

            return null;
        }
    }
}
