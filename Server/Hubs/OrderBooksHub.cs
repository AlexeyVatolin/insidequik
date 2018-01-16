using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.SignalR;
using QuikSharp.DataStructures;
using Server.Data;

namespace Server.Hubs
{
    class OrderBooksHub : Hub
    {
        //Потокобезопасный словарь
        private ConcurrentDictionary<string, HashSet<string>> Dictionary =
            new ConcurrentDictionary<string, HashSet<string>>();

        public void Subscride(string ticker)
        {
            if (Dictionary.ContainsKey(ticker))
            {
                if (!Dictionary[ticker].Contains(Context.ConnectionId))
                {
                    Dictionary[ticker].Add(Context.ConnectionId);
                }
            }
            else
            {
                Dictionary.TryAdd(ticker, new HashSet<string>() {Context.ConnectionId});
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

            foreach (var connectionId in Dictionary[quote.class_code])
            {
                Clients.Client(connectionId).OnQuote(orderBookList);
            }
        }

        public void UnSubscride(string ticker)
        {
            Dictionary[ticker].RemoveWhere(item => item == Context.ConnectionId);
        }
    }
}
