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
    internal class RequestClient : IRequestClient
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
            string prevPageToken = null;
            do
            {
                JObject requestData = new JObject()
                {
                    { "max-results", "20000" },
                    { "start-token", nextPageToken }
                };

                IncrementalResponse<T> response = await PerformPostAsync<IncrementalResponse<T>>(url, requestData);

                results.AddRange(response.Data.Items);
                totalTracks += response.Data.Items.Count;

                if (itemsToGet != 0)
                {
                    if (totalTracks < itemsToGet)
                    {
                        prevPageToken = nextPageToken;
                        nextPageToken = response.NextPageToken;
                    }
                    else
                    {
                        return results.Take(itemsToGet).ToList();
                    }
                }
                else
                {
                    prevPageToken = nextPageToken;
                    nextPageToken = response.NextPageToken;
                }
            }
            while (nextPageToken != null);

            return results;
        }

        public async Task<T> PerformPostAsync<T>(string url, JObject data)
        {
            var client = new HttpClient();

            client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue
                .Parse(String.Format("GoogleLogin auth={0}", gpsoauth.OAuthToken));
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            
            string requestBody = data.ToString();
            HttpContent content = new StringContent(requestBody, System.Text.Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, content);
            var responseString = await response.Content.ReadAsStringAsync();

            T result = JsonConvert.DeserializeObject<T>(responseString);
            return result;
        }

        public async Task<T> PerformGetAsync<T>(string url, NameValueCollection additionalHeaders = null)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue
                .Parse(String.Format("GoogleLogin auth={0}", gpsoauth.OAuthToken));

            if (additionalHeaders != null)
            {
                foreach (string key in additionalHeaders.Keys)
                {
                    client.DefaultRequestHeaders.Add(key, additionalHeaders.Get(key));
                }
            }

            var response = await client.GetAsync(url);
            var responseString = await response.Content.ReadAsStringAsync();
            T result = JsonConvert.DeserializeObject<T>(responseString);
            return result;
        }

        public async Task<string> PerformGetStringAsync(string url, NameValueCollection additionalHeaders = null)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue
                .Parse(String.Format("GoogleLogin auth={0}", gpsoauth.OAuthToken));

            if (additionalHeaders != null)
            {
                foreach (string key in additionalHeaders.Keys)
                {
                    client.DefaultRequestHeaders.Add(key, additionalHeaders.Get(key));
                }
            }

            var response = await client.GetAsync(url);
            return response.RequestMessage.RequestUri.ToString();
        }
    }
}
