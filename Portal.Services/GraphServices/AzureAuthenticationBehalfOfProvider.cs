using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Graph;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace Portal.Services.GraphServices
{
    public class AzureAuthenticationBehalfOfProvider : IAuthenticationProvider
    {
        private readonly string _accessToken;
        private readonly string _username;

        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _tenant;

        public AzureAuthenticationBehalfOfProvider(string accessToken, string username, string clientId, string clientSecret, string tenant)
        {
            this._accessToken = accessToken;
            this._username = username;
            this._clientId = clientId;
            this._clientSecret = clientSecret;
            this._tenant = tenant; 
        }

        public async Task AuthenticateRequestAsync(HttpRequestMessage request)
        {
            var userAssertion = new UserAssertion(_accessToken, "urn:ietf:params:oauth:grant-type:jwt-bearer", _username);

            AuthenticationContext authContext = new AuthenticationContext($"https://login.microsoftonline.com/{_tenant}");

            ClientCredential creds = new ClientCredential(_clientId, _clientSecret);

            AuthenticationResult authResult = await authContext.AcquireTokenAsync("https://graph.microsoft.com/", creds, userAssertion);

            request.Headers.Add("Authorization", "Bearer " + authResult.AccessToken);
        }
    }
}
