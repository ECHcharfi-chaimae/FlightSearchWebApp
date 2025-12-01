using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace travel_app
{
    public class TravelAPI
    {
        private string apiKey;
        private string apiSecret;
        private string bearerToken;
        private HttpClient http;

        public TravelAPI(IConfiguration config, IHttpClientFactory httpFactory)
        {
            apiKey = config.GetValue<string>("AmadeusAPI:APIKey");
            apiSecret = config.GetValue<string>("AmadeusAPI:APISecret");
            http = httpFactory.CreateClient("TravelAPI");
        }

        public async Task ConnectOAuth()
        {
            var message = new HttpRequestMessage(HttpMethod.Post, "/v1/security/oauth2/token");
            message.Content = new StringContent(
                $"grant_type=client_credentials&client_id={apiKey}&client_secret={apiSecret}",
                Encoding.UTF8, "application/x-www-form-urlencoded"
            );

            var results = await http.SendAsync(message);
            await using var stream = await results.Content.ReadAsStreamAsync();
            var oauthResults = await JsonSerializer.DeserializeAsync<OAuthResults>(stream);

            bearerToken = oauthResults.access_token;
        }

        private class OAuthResults
        {
            public string access_token { get; set; }
        }

        public async Task<BusiestPeriodResults> GetTravel(string v_depart, string v_arrive, string d_depart, string classe, string nbre_adults)
        {
            var message = new HttpRequestMessage(HttpMethod.Get,
                $"/v2/shopping/flight-offers?originLocationCode={v_depart}&destinationLocationCode={v_arrive}&departureDate={d_depart}&travelClass={classe}&adults={nbre_adults}");
            ConfigBearerTokenHeader();
            var response = await http.SendAsync(message);
            using var stream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<BusiestPeriodResults>(stream);
        }

        private void ConfigBearerTokenHeader()
        {
            http.DefaultRequestHeaders.Add("Authorization", $"Bearer {bearerToken}");
        }

        public class BusiestPeriodResults
        {
            public class item
            {
                public string id { get; set; }
                public List<Itiner> itineraries { get; set; }
                public price price { get; set; }
                public string total => price.total;
                public string currency => price.currency;

            }
            public class price
            {
                public string currency { get; set; }
                public string total { get; set; }
            }
            public class Itiner
            {
                public List<Segm> segments { get; set; }

            }

            public class Segm
            {
                public Dep departure { get; set; }
                public string at1 => departure.at;
                public string ville_dep => departure.iataCode;
                public arr arrival { get; set; }
                public string at2 => arrival.at;
                public string ville_arr => arrival.iataCode;

            }
            public class Dep
            {
                public string at { get; set; }
                public string iataCode { get; set; }

            }

            public class arr
            {
                public string at { get; set; }
                public string iataCode { get; set; }

            }



            public List<item> data { get; set; }
        }
    }
}
