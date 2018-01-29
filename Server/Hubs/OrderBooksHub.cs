using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Common.Interfaces;
using Microsoft.AspNet.SignalR;
using QuikSharp.DataStructures;
using Server.Quik;

namespace Server.Hubs
{
    class OrderBooksHub : Hub<IOrderBook>
    {
        //Потокобезопасный словарь
        private static readonly ConcurrentDictionary<string, int> CountOfTickersSubscribers =
            new ConcurrentDictionary<string, int>();

        public void Subscride(string ticker)
        {
            CountOfTickersSubscribers.AddOrUpdate(ticker, 1, (key, oldValue) => ++oldValue);
            Groups.Add(Context.ConnectionId, ticker);
        }

        public void OnQuoteDo(OrderBook quote)
        {
            List<Common.Models.OrderBook> orderBookList = new List<Common.Models.OrderBook>();
            foreach (OrderBook.PriceQuantity offer in quote.offer.Reverse())
            {
                orderBookList.Add(new Common.Models.OrderBook()
                {
                    Price = offer.price,
                    Quantity = offer.quantity,
                    Type = "offer"
                });
            }

            foreach (var bid in quote.bid.Reverse())
            {
                orderBookList.Add(new Common.Models.OrderBook()
                {
                    Price = bid.price,
                    Quantity = bid.quantity,
                    Type = "bid"
                });
            }

            //Отправляем данные всем, кто на них подписался (есть в группе с названием тикера)
            Clients.Group(quote.sec_code).OnQuote(orderBookList);
        }

        public void UnSubscride(string ticker)
        {
            int value;
            CountOfTickersSubscribers.TryGetValue(ticker, out value);
            if (value == 1)
            {
                CountOfTickersSubscribers.TryRemove(ticker, out value);
                QuikData.UnsubscribeFromOrderBook(ticker);
            }

            Groups.Remove(Context.ConnectionId, ticker);

        }
    }
}
