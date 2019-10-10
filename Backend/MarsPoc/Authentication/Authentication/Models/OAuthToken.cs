using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Models
{
    public class OAuthToken : IOAuthToken
    {
        private IConfiguration configuration;
        public OAuthToken(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public async Task<ResponseToken> GetToken(string userName, string password)
        {
            try
            {
                var authorityBaseUrl = configuration.GetSection("AzureAd").GetSection("AuthorityBaseUrl").Value;
                var resourceBaseUrl = configuration.GetSection("AzureAd").GetSection("ResourceBaseUrl").Value;
                var clientId = configuration.GetSection("AzureAd").GetSection("ClientId").Value;
                var tenantId = configuration.GetSection("AzureAd").GetSection("TenantId").Value;
                var clientSecret = configuration.GetSection("AzureAd").GetSection("ClientSecret").Value;

                var client = new HttpClient();
                var url = new Uri($"{authorityBaseUrl}/{tenantId}/oauth2/token");

                var body = CreateBody(clientId, clientSecret, resourceBaseUrl, userName, password);

                var response = await client.PostAsync(url, body);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    ResponseToken res = await response.Content.ReadAsAsync<ResponseToken>();
                    return res;
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }

            return null;
        }

        private HttpContent CreateBody(string clientId,
                                       string clientSecret,
                                       string resource,
                                       string userName,
                                       string password)
        {
            var body = new Dictionary<string, string>
            {
                {"grant_type", "password"},
                {"client_id", clientId},
                {"client_secret", clientSecret},
                {"resource", resource},
                {"username", userName},
                {"password", password}
            };

            return new FormUrlEncodedContent(body);
        }

    }
}
