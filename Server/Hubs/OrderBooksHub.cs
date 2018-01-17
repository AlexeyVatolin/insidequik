using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.SignalR;
using QuikSharp.DataStructures;
using Server.Data;
using Server.Quik;

namespace Server.Hubs
{
    class OrderBooksHub : Hub
    {
        //Потокобезопасный словарь
        private ConcurrentDictionary<string, HashSet<string>> usersAndTickers =
            new ConcurrentDictionary<string, HashSet<string>>();

        public void Subscride(string ticker)
        {
            if (usersAndTickers.ContainsKey(ticker))
            {
                if (!usersAndTickers[ticker].Contains(Context.ConnectionId))
                {
                    usersAndTickers[ticker].Add(Context.ConnectionId);
                }
            }
            else
            {
                usersAndTickers.TryAdd(ticker, new HashSet<string> {Context.ConnectionId});
                QuikData.SubscribeToOrderBook(ticker, OnQuoteDo);
            }
        }

        public void OnQuoteDo(OrderBook quote)
        {
            List<OrderBookRow> orderBookList = new List<OrderBookRow>();
            foreach (OrderBook.PriceQuantity offer in quote.offer.Reverse())
            {
                orderBookList.Add(new OrderBookRow(offer, "offer"));
            }

            foreach (var bid in quote.bid.Reverse())
            {
                orderBookList.Add(new OrderBookRow(bid, "bid"));
            }

            foreach (var connectionId in usersAndTickers[quote.class_code])
            {
                Clients.Client(connectionId).OnQuote(orderBookList);
            }
        }

        public void UnSubscride(string ticker)
        {
            usersAndTickers[ticker].RemoveWhere(item => item == Context.ConnectionId);
            if (usersAndTickers[ticker].Count == 0)
            {
                QuikData.UnsubsckibeFromOrderBook(ticker);
                var outHashSet = new HashSet<string>();
                usersAndTickers.TryRemove(ticker, out outHashSet);
            }
        }
    }
}
