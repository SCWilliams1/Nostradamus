using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace Nostra3.Module
{
    public static class APIHandler
    {
        public static HttpClient ApiClient { get; set; }

        public static void InitClient()
        {
            ApiClient = new HttpClient();
            //ApiClient.BaseAddress = new Uri("");

            ApiClient.DefaultRequestHeaders.Accept.Clear();
            ApiClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}

