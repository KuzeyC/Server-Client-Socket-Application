namespace BankServer {
    class Trader {
        public Trader (int TraderId, bool Stock) {
            this.TraderId = TraderId;
            this.Stock = Stock;
        }
        public int TraderId { get; }
        public bool Stock { get; set; }
    }
}