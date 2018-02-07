using Microsoft.AspNet.SignalR;
using Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Quik;
using QuikSharp.DataStructures.Transaction;

namespace Server.Hubs
{
    public class TradesHub: Hub<ITrades>
    {
        public void SubscribeToTradesRefresh()
        {
            Groups.Add(Context.ConnectionId, "Trades");
            QuikData.SubscribeToTradesRefresh(tradesRefresh);
        }
        public void InitializeTrades()
        {
            List<Trade> trades = QuikConnector.Quik.Trading.GetTrades().Result;
            //TODO:выбрать сделки по id юзера
            Clients.Caller.OnInitialize(trades);
        }
        private void tradesRefresh(Trade trade)
        {//TODO: распределение сделок по id    
            Clients.Group("Trades").OnTrades(trade);
        }
    }
}
