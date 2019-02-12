using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Http;
using Google.Apis.Services;
using GooglePlayMusicAPI.Models.RequestModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GooglePlayMusicAPI.Client
{
    public class OAuth2Client : IRequestClient
    {
        private static string ClientId = "228293309116.apps.googleusercontent.com";
        private static string ClientSecret = "GL1YV0XMp0RlL7ylCV3ilFz-";
        private static string Scope = "https://www.googleapis.com/auth/skyjam";

        UserCredential cred { get; set; }
        ConfigurableHttpClient client { get; set; }

        public OAuth2Client()
        {

        }

        public bool IsLoggedIn()
        {
            return cred != null && cred.Token != null && client != null;
        }

        public async Task<bool> LoginAsync()
        {
            this.cred = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                new ClientSecrets
                {
                    ClientId = OAuth2Client.ClientId,
                    ClientSecret = OAuth2Client.ClientSecret
                },
                new[] { OAuth2Client.Scope },
                "user",
                CancellationToken.None);


            var service = new BaseClientService.Initializer()
            {
                HttpClientInitializer = cred,
                HttpClientFactory = new HttpClientFactory()
            };


            this.client = service.HttpClientFactory.CreateHttpClient(new CreateHttpClientArgs()
            {
                ApplicationName = "GPM .NET example"
            });

            service.HttpClientInitializer.Initialize(this.client);

            return true;
        }

        public async Task<T> PerformGetAsync<T>(string url, NameValueCollection additionalHeaders = null)
        {
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
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            string requestBody = data.ToString();
            HttpContent content = new StringContent(requestBody, System.Text.Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, content);
            var responseString = await response.Content.ReadAsStringAsync();

            T result = JsonConvert.DeserializeObject<T>(responseString);
            return result;
        }
    }
}
