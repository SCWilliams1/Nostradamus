using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Nostra3.Module
{
    class Coin
    {
        public string id { get; set; }
        public string symbol { get; set; }
        public string name { get; set; }

        public List<double[]> prices { get; set; }
        public List<double[]> market_caps { get; set; }
        public List<double[]> total_volumes { get; set; }

        public decimal price { get; set; }//Current value USD
        public double volume { get; set; }//Value of Change
        public double cap { get; set; }//Total value of all coins
        public double supply { get; set; }//how many exist

        public CoinCollection coinbank { get; set; }

        enum CoinProperty {Date,Price,Cap,Volume}

        public void CalculateData() 
        {
            List<List<double>> MList = ConvertToUsable();

            int EoL = MList[0].Count-1;
            price = (decimal)MList[(int)CoinProperty.Price][EoL];
            volume = (double)MList[(int)CoinProperty.Volume][EoL];
            cap = (double)MList[(int)CoinProperty.Cap][EoL];
            supply = cap/(double)price;

            Console.WriteLine(
                $"---------------------------------------------\n" +
                $"Name: {name} ({coinbank.CollectionName}) \n" +
                $"---------------------------------------------\n" +
                $"Symbol: {symbol}\n" +
                $"Price:    {price.ToString("C2")} \n" +
                $"Volume:   {volume} \n" +
                $"Mkt Cap:  {cap} \n" +
                $"Supply:   {supply} \n" +
                $"---------------------------------------------\n" +
                $"Analysis:");

            List<int> PInterest = new List<int>();
            List<double> PriceChange = new List<double>();
            List<double> VolumeChange = new List<double>();

            int entries = EoL + 1;
            double AvgPrice = MList[(int)CoinProperty.Price].Average();
            double AvgVolume = MList[(int)CoinProperty.Volume].Average();

            //Get initial Data set
            for (int i = 1; i < MList[0].Count; i++) 
            {
                //PriceChange.Add(MList[(int)CoinProperty.Price][i] / MList[(int)CoinProperty.Price][i - 1]);
                //VolumeChange.Add(MList[(int)CoinProperty.Volume][i] / MList[(int)CoinProperty.Volume][i - 1]);
                PriceChange.Add((MList[(int)CoinProperty.Price][i]-MList[(int)CoinProperty.Price][i-1])/MList[(int)CoinProperty.Price][i - 1]);              
                VolumeChange.Add((MList[(int)CoinProperty.Volume][i]-MList[(int)CoinProperty.Volume][i - 1])/MList[(int)CoinProperty.Volume][i - 1]);
            }
            double AvgPriceChange = PriceChange.Average();
            double AvgVolumeChange = VolumeChange.Average();
            double priceChangeMax = 0;
            double volumeChangeMax = 0;
            foreach (double change in PriceChange) { priceChangeMax = Math.Max(priceChangeMax, change); }
            foreach (double change in VolumeChange) { volumeChangeMax = Math.Max(volumeChangeMax, change); }

            Console.WriteLine(
                $"Average Price:  {AvgPrice}\n"+
                $"Average Volume: {AvgVolume}\n" +
                $"Average Price Change: {AvgPriceChange * 100}%\n" +
                $"Average Volume Change: {AvgVolumeChange * 100}%\n\n" +
                $"Greatest Price:  {AvgPrice}\n" +
                $"Greatest Volume: {AvgVolume}\n" +
                $"Greatest Price Change: {priceChangeMax * 100}%\n" +
                $"Greatest Volume Change: {volumeChangeMax * 100}%\n\n");

            double Plen = 0.5;
            double Vlen = 10;
            Console.WriteLine($"Points of Interest (> {Plen*100}%P, {Vlen*100}%V):");
            for (int i = 0; i < PriceChange.Count; i++)
            {
                if (Math.Abs(PriceChange[i]) > Plen)
                {
                    PInterest.Add(i);
                }else{
                    if (Math.Abs(VolumeChange[i]) > Vlen) { PInterest.Add(i); }
                }
            }
            if (PInterest.Count <= 0)
            {
                Console.WriteLine("There were no points of interest.");
            }
            else
            {
                foreach (int point in PInterest)
                {
                    Console.WriteLine(
                        $"----------------------------------------------\n" +
                        $"Date: {UnixTimeToDateTime((long)MList[(int)CoinProperty.Date][point]).ToString("s",CultureInfo.CreateSpecificCulture("en-US"))}\n" +
                        $"Price: {MList[(int)CoinProperty.Price][point]}\n" +
                        $"Volume: {MList[(int)CoinProperty.Volume][point]}\n" +
                        $"Price Change: {PriceChange[point] * 100}%\n" +
                        $"Volume Change: {VolumeChange[point] * 100}%\n\n");
                }
            }
        }

        public List<List<double>> ConvertToUsable()
        {
            List<List<double>> MasterList = new List<List<double>>();
            List<double> Timelist = new List<double>();
            List<double> Pricelist = new List<double>();
            List<double> Caplist = new List<double>();
            List<double> Volumelist = new List<double>();

            for (int i = 0; i < prices.Count; i++)
            {
                double[] _pc = prices[i];
                string _dT = UnixTimeToDateTime((long)_pc[0]).ToString("d", CultureInfo.CreateSpecificCulture("en-US"));
                double[] _mc = market_caps[i];
                double[] _tv = total_volumes[i];

                Timelist.Add(_pc[0]);
                Pricelist.Add(_pc[1]);
                Caplist.Add(_mc[1]);
                Volumelist.Add(_tv[1]);
                //Console.WriteLine($"{_dT} , {_pc[1]}, {_mc[1]}, {_tv[1]}");
            }
            MasterList.Add(Timelist);
            MasterList.Add(Pricelist);
            MasterList.Add(Caplist);
            MasterList.Add(Volumelist);

            return MasterList;
        }

        public DateTime UnixTimeToDateTime(long unixtime)
        {
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddMilliseconds(unixtime).ToLocalTime();
            return dtDateTime;
        }
        public void DisplayStatistics() 
        {
            Console.WriteLine("----------------------------------------");
            Console.WriteLine($"Name: {name} ({coinbank.CollectionName}) \n" +
                              "----------------------------------------\n" +
                              $"Symbol: {symbol}\n" +
                              $"Current Price: {price.ToString("C2")} \n" +
                              $"Current Volume: {volume} \n" +
                              $"Current Supply: {supply} \nS" +
                              "-----\n");
            //Console.WriteLine("----------------------------------------");
        }
    }
}
