using GooglePlayMusicAPI.Models.RequestModels;
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

namespace GooglePlayMusicAPI.Client
{
    public abstract class BaseRequestClient : IRequestClient
    {
        public abstract HttpClient GetHttpClient();
        public abstract bool IsLoggedIn();
        public abstract Task<bool> LoginAsync();
        public abstract Task<bool> LoginWithEmailPasswordAsync(string email, string password);

        public async Task<T> PerformGetAsync<T>(string url, NameValueCollection additionalHeaders = null)
        {
            HttpClient client = GetHttpClient();
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
            HttpClient client = GetHttpClient();

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
            HttpClient client = GetHttpClient();

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

                // sometimes the previous request will include a non null next page token, so we fetch and get no data
                // which results in null data and null nextpagetoken. In this case, we've fetched all we can, so return
                // what we've fetched.
                if (response.Data == null)
                {
                    if (response.NextPageToken == null)
                    {
                        return results;
                    }
                    else
                    {
                        // TODO: error?
                        return results;
                    }
                }

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
            HttpClient client = GetHttpClient();

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
