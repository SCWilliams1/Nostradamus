using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Nostra3.Module
{
    public class DataTester
    {
        //Not particularly necessary but just to show that I can
        //Singleton Example
        public static DataTester Instance {get; } = new DataTester();


        //PingData
        public static async Task<DataModel> Ping()
        {
            string url = $"https://api.coingecko.com/api/v3/ping";

            using (HttpResponseMessage response = await APIHandler.ApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode){
                    //Do things
                    DataModel data = await response.Content.ReadAsAsync<DataModel>();
                    return data;
                }else{
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public static async Task<DataModel> SearchData()
        {
            string url = "";
            url = $"https://api.coingecko.com/api/v3/coins/bitcoin/market_chart?vs_currency=usd&days=1";
            using (HttpResponseMessage response = await APIHandler.ApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode){
                    //Do things
                    DataModel data = await response.Content.ReadAsAsync<DataModel>();
                    return data;
                }else{
                    throw new Exception(response.ReasonPhrase);
                }
            }
            throw new Exception();
            //return null;
        }
    }
}
