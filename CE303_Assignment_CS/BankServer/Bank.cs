using System;
using System.Collections.Generic;

namespace BankServer {
    class Bank {
        private readonly Dictionary<int, Trader> traders = new Dictionary<int, Trader> ();

        public void CreateTrader (int traderId, bool hasStock) {
            Trader trader = new Trader (traderId, hasStock);
            AddToTraders (trader);
        }

        public bool HasTraderStock (int traderId) {
            return traders[traderId].Stock;
        }

        public void Transfer (int fromTrader, int toTrader) {
            lock (traders) {
                if (!traders[fromTrader].Stock)
                    throw new Exception ($"Trader {fromTrader} + does not have a stock. Unable to transfer.");
                traders[fromTrader].Stock = false;
                traders[toTrader].Stock = true;

            }
        }

        public Dictionary<int, Trader> GetCurrentTraders () {
            return traders;
        }

        public void AddToTraders (Trader t) {
            traders.Add (t.TraderId, t);
        }

        public void RemoveFromTraders (int traderId) {
            traders.Remove (traderId);
        }
    }
}