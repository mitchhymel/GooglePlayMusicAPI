using Mono.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace GooglePlayMusicAPI
{
    public class RequestClient : IRequestClient
    {
        private string ClientId { get; set; }
        private OAuthClient gpsoauth { get; set; }

        public RequestClient(string clientId)
        {
            ClientId = clientId;
            gpsoauth = new OAuthClient();
        }

        public bool IsLoggedIn()
        {
            return !String.IsNullOrEmpty(gpsoauth.OAuthToken);
        }

        public async Task<bool> LoginAsync(string email, string password)
        {
            Dictionary<String, String> response = await gpsoauth.PerformMasterLogin(email, password);
            if (!response.ContainsKey("Token"))
            {
                Console.WriteLine("Master auth failed");
                return false;
            }

            gpsoauth.MasterToken = response["Token"];

            Dictionary<String, String> oauthResponse = await gpsoauth.PerformOAuth(email, gpsoauth.MasterToken, "sj",
                    "com.google.android.music", ClientId);
            if (!oauthResponse.ContainsKey("Auth"))
            {
                Console.WriteLine("Oauth login failed");
                return false;
            }

            gpsoauth.OAuthToken = oauthResponse["Auth"];

            return true;
        }

        public async Task<List<T>> PerformIncrementalPostAsync<T>(string url, int itemsToGet = 0)
        {
            List<T> results = new List<T>();
            int totalTracks = 0;
            string nextPageToken = null;
            do
            {
                JObject requestData = new JObject()
                {
                    { "max-results", "20000" },
                    { "start_token", nextPageToken }
                };

                IncrementalResponse<T> response = await PerformPostAsync<IncrementalResponse<T>>(url, requestData);

                results.AddRange(response.Data.Items);
                totalTracks += response.Data.Items.Count;

                if (itemsToGet != 0)
                {
                    if (totalTracks < itemsToGet)
                    {
                        nextPageToken = response.NextPageToken;
                    }
                    else
                    {
                        return results.Take(itemsToGet).ToList();
                    }
                }
                else
                {
                    nextPageToken = response.NextPageToken;
                }
            }
            while (nextPageToken != null);

            return results;
        }

        public async Task<T> PerformPostAsync<T>(string url, JObject data, NameValueCollection additionalParams = null)
        {
            var client = new HttpClient();

            client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue
                .Parse(String.Format("GoogleLogin auth={0}", gpsoauth.OAuthToken));
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            string combinedUrl = BuildRequestUrl(url, additionalParams);
            string requestBody = data.ToString();
            HttpContent content = new StringContent(requestBody, System.Text.Encoding.UTF8, "application/json");
            var response = await client.PostAsync(combinedUrl, content);
            var responseString = await response.Content.ReadAsStringAsync();

            T result = JsonConvert.DeserializeObject<T>(responseString);
            return result;
        }

        public async Task<T> PerformGetAsync<T>(string url, NameValueCollection additionalParams = null)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue
                .Parse(String.Format("GoogleLogin auth={0}", gpsoauth.OAuthToken));

            string combinedUrl = BuildRequestUrl(url, additionalParams);
            var response = await client.GetAsync(combinedUrl);
            var responseString = await response.Content.ReadAsStringAsync();
            T result = JsonConvert.DeserializeObject<T>(responseString);
            return result;
        }

        private string BuildRequestUrl(string urlBase, NameValueCollection additionalParams)
        {
            var builder = new UriBuilder(urlBase);
            builder.Port = -1;
            var query = HttpUtility.ParseQueryString(builder.Query);
            query["alt"] = "json";
            query["dv"] = "0";
            query["hl"] = "en_US";
            query["tier"] = "aa";

            if (additionalParams != null)
            {
                query.Add(additionalParams);
            }

            builder.Query = query.ToString();

            string combinedUrl = builder.ToString();

            return combinedUrl;
        }
    }
}
