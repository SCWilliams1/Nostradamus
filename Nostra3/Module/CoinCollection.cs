using System;
using System.Collections.Generic;
using System.Text;

namespace Nostra3.Module
{
    class CoinCollection
    {
        public int ID { get; set; }

        public string CollectionName { get; set; }
        public string CoinManager { get; set; }
        public List<Coin> Coins { get; set; }


        public CoinCollection() 
        {
            Coins = new List<Coin>();
        }
    }
}
