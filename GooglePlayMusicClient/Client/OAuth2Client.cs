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
    public class OAuth2Client : BaseRequestClient
    {
        private static string ClientId = "228293309116.apps.googleusercontent.com";
        private static string ClientSecret = "GL1YV0XMp0RlL7ylCV3ilFz-";
        private static string Scope = "https://www.googleapis.com/auth/skyjam";
        
        string ApplicationName { get; set; }
        UserCredential Cred { get; set; }
        

        public OAuth2Client(string applicationName)
        {
            ApplicationName = applicationName;
        }

        override public bool IsLoggedIn()
        {
            return Cred != null && Cred.Token != null;
        }

        override public async Task<bool> LoginAsync()
        {
            this.Cred = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                new ClientSecrets
                {
                    ClientId = OAuth2Client.ClientId,
                    ClientSecret = OAuth2Client.ClientSecret
                },
                new[] { OAuth2Client.Scope },
                "user",
                CancellationToken.None);

            return this.Cred != null && this.Cred.Token != null;
        }

        override public Task<bool> LoginWithEmailPasswordAsync(string email, string password)
        {
            // OAuth 2 client doesn't support login with email and password
            throw new NotImplementedException();
        }

        override public HttpClient GetHttpClient()
        {
            var service = new BaseClientService.Initializer()
            {
                HttpClientInitializer = Cred,
                HttpClientFactory = new HttpClientFactory()
            };

            ConfigurableHttpClient client = service.HttpClientFactory.CreateHttpClient(new CreateHttpClientArgs()
            {
                ApplicationName = ApplicationName
            });

            service.HttpClientInitializer.Initialize(client);

            return client;
        }
    }
}
