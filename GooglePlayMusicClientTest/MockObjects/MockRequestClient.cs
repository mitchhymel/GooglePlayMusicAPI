using GooglePlayMusicAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;
using Newtonsoft.Json.Linq;

namespace GooglePlayMusicClientTest
{
    public class MockRequestClient : IRequestClient
    {
        private bool loggedIn = false;
        private string authToken = "012345";

        public MockRequestClient()
        {

        }

        public bool IsLoggedIn()
        {
            return loggedIn;
        }

        public async Task<bool> LoginAsync(string email, string password)
        {
            loggedIn = true;
            return loggedIn;
        }

        public async Task<T> PerformGetAsync<T>(string url, NameValueCollection additionalParams = null)
        {
            throw new NotImplementedException();
        }

        public async Task<List<T>> PerformIncrementalPostAsync<T>(string url, int itemsToGet = 0)
        {
            throw new NotImplementedException();
        }

        public async Task<T> PerformPostAsync<T>(string url, JObject data, NameValueCollection additionalParams = null)
        {
            throw new NotImplementedException();
        }
    }
}
