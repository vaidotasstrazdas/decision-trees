using Bridge.IBLL.Data;
using Implementation.BLL;
using Shared.DecisionTrees.DataStructure;

namespace Tests.BLLTest.Helpers
{
    public static class FakeTradingAgent
    {

        public static ForexTradingService Service { get; set; }

        public static void SimpleBuy()
        {
            Service.PlaceBid(new ForexTreeData
            {
                Bid = 1.1111,
                Ask = 1.1114,
                Action = MarketAction.Buy
            }, MarketAction.Buy);
        }

        public static void SimpleSell()
        {
            Service.PlaceBid(new ForexTreeData
            {
                Bid = 1.1111,
                Ask = 1.1114,
                Action = MarketAction.Sell
            }, MarketAction.Sell);
        }

        public static void SimpleBuySell()
        {
            Service.PlaceBid(new ForexTreeData
            {
                Bid = 1.1111,
                Ask = 1.1114,
                Action = MarketAction.Buy
            }, MarketAction.Buy);

            Service.PlaceBid(new ForexTreeData
            {
                Bid = 1.1105,
                Ask = 1.1108,
                Action = MarketAction.Sell
            }, MarketAction.Sell);
        }

        public static void TradeSequence()
        {
            Service.PlaceBid(new ForexTreeData
            {
                Bid = 1.1111,
                Ask = 1.1114,
                Action = MarketAction.Buy
            }, MarketAction.Buy);

            Service.PlaceBid(new ForexTreeData
            {
                Bid = 1.1105,
                Ask = 1.1108,
                Action = MarketAction.Sell
            }, MarketAction.Sell);

            Service.PlaceBid(new ForexTreeData
            {
                Bid = 1.1113,
                Ask = 1.1117,
                Action = MarketAction.Buy
            }, MarketAction.Buy);

            Service.PlaceBid(new ForexTreeData
            {
                Bid = 1.1111,
                Ask = 1.1114,
                Action = MarketAction.Buy
            }, MarketAction.Buy);

            Service.PlaceBid(new ForexTreeData
            {
                Bid = 1.1111,
                Ask = 1.1114,
                Action = MarketAction.Buy
            }, MarketAction.Buy);

            Service.PlaceBid(new ForexTreeData
            {
                Bid = 1.11,
                Ask = 1.1104,
                Action = MarketAction.Sell
            }, MarketAction.Sell);

            Service.PlaceBid(new ForexTreeData
            {
                Bid = 1.1105,
                Ask = 1.1108,
                Action = MarketAction.Sell
            }, MarketAction.Sell);

            Service.PlaceBid(new ForexTreeData
            {
                Bid = 1.1105,
                Ask = 1.1108,
                Action = MarketAction.Sell
            }, MarketAction.Sell);

            Service.PlaceBid(new ForexTreeData
            {
                Bid = 1.1111,
                Ask = 1.1114,
                Action = MarketAction.Hold
            }, MarketAction.Buy);

            Service.PlaceBid(new ForexTreeData
            {
                Bid = 1.1113,
                Ask = 1.1117,
                Action = MarketAction.Hold
            }, MarketAction.Sell);
        }

    }
}
