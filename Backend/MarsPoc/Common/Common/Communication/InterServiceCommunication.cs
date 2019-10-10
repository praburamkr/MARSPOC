using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace Common.Communication
{
    public static class InterServiceCommunication
    {
        public static async Task<string> GetAsync(string Url, string BearerToken)

        {
            try
            {
                HttpClient client = new HttpClient();
                string contentType = "application/json";
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

                if (!string.IsNullOrEmpty(BearerToken))
                    client.DefaultRequestHeaders.Add("Authorization", BearerToken);


                var response = await client.GetAsync(Url);

                if (response != null && response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    return data;
                }
            }
            catch (Exception ms)
            {

            }

            return null;
        }

        public static async Task<string> PostAsync(string Url, string BearerToken, object body)
        {
            try
            {
                HttpClient client = new HttpClient();
                string contentType = "application/json";
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

                if (!string.IsNullOrEmpty(BearerToken))
                    client.DefaultRequestHeaders.Add("Authorization", BearerToken);

                dynamic clientData = Newtonsoft.Json.JsonConvert.SerializeObject(body);
                var httpContent = new StringContent(clientData, Encoding.UTF8, "application/json");
                // E.g. a JSON string.

                var response = await client.PostAsync(Url, httpContent);

                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    return data;
                }
            }
            catch
            {

            }

            return null;
        }

    }
}
