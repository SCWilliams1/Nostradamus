using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using Nostra3.Module;
using Nostra3.Data;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Nostra3.Module
{
    class SystemMngr
    {
        public int duration { get; set; }
        public Coin CurrentCoin { get; set; }

        public LibraryContext Library { get; set; }

        public void Run() {
            //Generate App Structure
            Library = LibraryContext.GetLibrary();
           
            APIHandler.InitClient();//API Caller
            //Ping();
            LoadCoinList(Library);//Load Data from API
            Console.WriteLine("Initialized.");
            Console.WriteLine(
                $"--------------------------------------------------\n"+
                $"There are {Library.Coins.Count()} searchable coins.\n"+
                $"--------------------------------------------------");

            //Parameter Set
            ParameterSet();

            //Data Process
            CoinLookup();

            //Display Output
            //Console.WriteLine($"{CurrentCoin.name}, {duration} Finished.\n");


            Console.WriteLine($"Would you like to continue? Y/N");
            string cont = Console.ReadLine();
            if (cont.ToUpper().Equals("Y")) { Run(); }
            return;
        }

        public void CoinLookup() 
        {
            Coin coin = Search(CurrentCoin,duration).Result;
        }

        public static async Task<Coin> Search(Coin coin, int day)
        {

            string url = $"https://api.coingecko.com/api/v3/coins/{coin.id}/market_chart?vs_currency=usd&days={day}";
            using (HttpResponseMessage response = await APIHandler.ApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    //Do things
                    string json = await response.Content.ReadAsStringAsync();
                    Coin tempCoin = JsonConvert.DeserializeObject<Coin>(json);
                    coin.prices = tempCoin.prices;
                    coin.market_caps = tempCoin.market_caps;
                    coin.total_volumes = tempCoin.total_volumes;
                    coin.CalculateData();
                    return coin;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }





        static void LoadCoinList(LibraryContext context)
        {
            CoinCollection cc = new CoinCollection();
            cc.CollectionName = "CoinBank 1";
            cc.CoinManager = "Anon";
            context.Coincollections.Add(cc);

            context.Coins = List().Result;
            foreach (Coin coin in context.Coins)
            {
                coin.coinbank = cc;
                //Console.WriteLine($"{coin.name} added");
            }
            context.CurrentCollection = cc;
        }
        public static async Task<List<Coin>> List()
        {
            string url = $"https://api.coingecko.com/api/v3/coins/list?include_platform=false";
            using (HttpResponseMessage response = await APIHandler.ApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    //Do things
                    string json = await response.Content.ReadAsStringAsync();
                    List<Coin> CoinList = JsonConvert.DeserializeObject<List<Coin>>(json);
                    return CoinList;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
        public static async Task<DataModel> Ping()
        {
            string url = $"https://api.coingecko.com/api/v3/ping";
            using (HttpResponseMessage response = await APIHandler.ApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode){
                    //Do things
                    DataModel data = await response.Content.ReadAsAsync<DataModel>();
                    Console.WriteLine($"Ping! {data.Gecko_says}");
                    return data;
                }else{
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
        private void ParameterSet()
        {
            //Name
            Console.WriteLine("Coin Name: ");
            string name = Console.ReadLine();
            
            //Name Verification
            while (!CoinNameCheck(name)) 
            {
                Console.WriteLine("Sorry we couldn't find that name. Please try again.\n");
                Console.WriteLine("Coin Name: ");

                name = Console.ReadLine();
            }

            //Duration
            Console.WriteLine("Days to Lookup: ");
            int days = 0;
            do
            {
                try
                {
                    days = Convert.ToInt32(Console.ReadLine());
                    break;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Invalid input.\n");
                    Console.WriteLine("Days: ");
                }
            } while (true);
            duration = days;
        }
        private bool CoinNameCheck(string id) 
        {
            foreach (Coin coin in Library.Coins) 
            {
                if (coin.id.Equals(id)) 
                {
                    CurrentCoin = coin;
                    return true; 
                }
            }
            return false;
        }

        /*
        static void PracticeData(LibraryContext Library)
        {
            CoinCollection coll1 = new CoinCollection();
            coll1.CollectionName = "CoinBank 1";
            coll1.CoinManager = "Anon";
            Library.Coincollections.Add(coll1);

            var coin1 = new Coin()
            {
                name = "bitcoin",
                symbol = "btc",
                price = 1.00M,
                volume = 1000,
                supply = 1000,
                coinbank = coll1
            };
            Library.Coins.Add(coin1);

            var coin2 = new Coin()
            {
                name = "ethereum",
                symbol = "eth",
                price = 2.00M,
                volume = 2000,
                supply = 2000,
                coinbank = coll1
            };
            Library.Coins.Add(coin2);
        }*/
    }
}
