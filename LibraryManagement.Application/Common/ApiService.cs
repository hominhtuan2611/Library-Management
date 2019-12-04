using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace LibraryManagement.Application.Common
{
    public class ApiService
    {
        public static HttpClient GetAPI(string address)
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            // Pass the handler to httpclient(from you are calling api)
            var client = new HttpClient(clientHandler);

            Uri apiAddress = new Uri(address);

            client.BaseAddress = apiAddress;

            client.DefaultRequestHeaders.Accept.Clear();

            client.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }
    }
}
