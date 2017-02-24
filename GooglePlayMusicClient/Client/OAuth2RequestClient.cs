using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GooglePlayMusicAPI.Client
{
    /// <summary>
    /// This class is an attempt to use true OAuth support (that is show a landing page where user can sign in
    /// through google and grant access to your app for the scopes needed for Google Play Music APIs).
    /// This does not work currently. Requesting a token for "skyjam" scope does actually successfully grant a token
    /// But using the token results in the following error:
    /// "errors": [
    //  {
    //  "domain": "usageLimits",
    //  "reason": "accessNotConfigured",
    //  "message": "Access Not Configured. has not been used in project xxxxxxxxxxxxxx before or it is disabled. Enable it by visiting https://console.developers.google.com/apis/api/sj/overview?project=xxxxxxxxxxxxxx then retry. If you enabled this API recently, wait a few minutes for the action to propagate to our systems and retry.",
    //  "extendedHelp": "https://console.developers.google.com/apis/api/sj/overview?project=xxxxxxxxxxxxxx"
    //  }]
    /// 
    /// The issue is tracked here: https://github.com/simon-weber/gmusicapi/issues/426
    /// </summary>
    public class OAuth2RequestClient : IRequestClient
    {
        private readonly string GoogleClientId = "yourclientid";
        private readonly string GoogleClientSecret = "yoursecret";
        private string ClientId { get; set; }
        private string OAuthToken;

        public OAuth2RequestClient(string clientId)
        {
            ClientId = clientId;
        }

        public bool IsLoggedIn()
        {
            return !String.IsNullOrEmpty(OAuthToken);
        }

        public async Task<bool> LoginAsync(string email, string password)
        {
            string[] scopes = new string[] { "https://www.googleapis.com/auth/skyjam" };

            // Once you have code
            string code = "";

            ClientSecrets secrets = new ClientSecrets()
            {
                ClientId = GoogleClientId,
                ClientSecret = GoogleClientSecret
            };

            IAuthorizationCodeFlow flow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = secrets,
                Scopes = scopes
            });

            UserCredential credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(secrets, scopes, "user", CancellationToken.None);

            OAuthToken = credential.Token.AccessToken;

            return true;
        }

        public async Task<List<T>> PerformIncrementalPostAsync<T>(string url, int itemsToGet = 0)
        {
            throw new NotImplementedException();
        }

        public async Task<T> PerformPostAsync<T>(string url, JObject data)
        {
            throw new NotImplementedException();
        }

        public async Task<T> PerformGetAsync<T>(string url, NameValueCollection additionalHeaders = null)
        {
            throw new NotImplementedException();
        }

        public Task<string> PerformGetStringAsync(string url, NameValueCollection additionalHeaders = null)
        {
            throw new NotImplementedException();
        }
    }
}
