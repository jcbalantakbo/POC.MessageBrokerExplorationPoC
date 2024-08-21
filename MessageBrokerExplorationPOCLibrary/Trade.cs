namespace MessageBrokerExplorationPOCLibrary
{
        public class Trade
        {
            public int TradeId { get; set; }
            public int MarketType { get; set; }
            public string Pair { get; set; }
            public double EntryPrice { get; set; }
            public double StopLoss { get; set; }
            public double TakeProfit { get; set; }

            // Constructor to initialize a Trade object
            public Trade(int tradeId, int marketType, string pair, double entryPrice, double stopLoss, double takeProfit)
            {
                TradeId = tradeId;
                MarketType = marketType;
                Pair = pair;
                EntryPrice = entryPrice;
                StopLoss = stopLoss;
                TakeProfit = takeProfit;
            }
        }
}
