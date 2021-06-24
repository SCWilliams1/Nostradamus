using System;
using System.Collections.Generic;
using System.Text;
using Nostra3.Module;

namespace Nostra3.Data
{
    class LibraryContext
    {
        public List<Coin> Coins { get; set; }
        public List<CoinCollection> Coincollections { get; set; }
        public CoinCollection CurrentCollection { get; set; }

        private static readonly LibraryContext instance = new LibraryContext();

        private  LibraryContext() 
        {
            Coins = new List<Coin>();
            Coincollections = new List<CoinCollection>();
        }
        public static LibraryContext GetLibrary() {
            return instance;
        }
    }
}

