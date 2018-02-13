using System;
using System.Collections.Generic;
using Common.Models;
using Microsoft.AspNet.SignalR.Client;

namespace MarketServerTest.SignalR
{
    public class TickClientBase
    {
        public string Ticker { get; set; }
        public string HubName { get; set; } = "OrderBooksHub";
        public Action<List<OrderBook>> OnQuoteAction { get; set; }
        private HubConnection _connection;
        private IHubProxy _orderBookHubProxy;
        private string _connectUrl = "http://localhost:8080/signalr";

        public async void Connect()
        {
            _connection = new HubConnection(_connectUrl, false)
            {
                TraceLevel = TraceLevels.All
            };
            _orderBookHubProxy = _connection.CreateHubProxy(HubName);
            _orderBookHubProxy.On("OnQuote", OnQuoteAction);
            await _connection.Start();
            if (!string.IsNullOrEmpty(Ticker))
            {
                await _orderBookHubProxy.Invoke("Subscride", Ticker);
            }
            else
            {
                throw new NullReferenceException("Ticker in null or empty");
            }
        }

        public async void Disconnect()
        {
            await _orderBookHubProxy.Invoke("UnSubscribe", Ticker);
            _connection.Stop();
        }
    }
}
