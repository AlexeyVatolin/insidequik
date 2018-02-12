using Common.Interfaces;
using MarketServerTest;
using Microsoft.AspNet.SignalR;
using QuikSharp.DataStructures.Transaction;
using Server.Quik;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Hubs
{
    public class OrdersHub: Hub<IOrders>
    {
        public void SubscribeToOrdersRefresh(long userId)
        {
            Groups.Add(userId.ToString(), "Orders");
            QuikData.SubscribeToOrdersRefresh(ordersRefresh);
        }
        public void CancelOrder(Order order) //TODO:Создать свою модель
        {
           QuikConnector.Quik.Orders.KillOrder(order);
        }
        public void InitializeOrders(long userId)
        {
            List<Order> orders=QuikConnector.Quik.Orders.GetOrders().Result;
            //TODO:выбрать ордеры по id для юзера
            List<Order> userOrders = orders.Where(x => x.TransID == userId).ToList();
            Clients.Caller.OnInitialize(userOrders);
        }
        private void ordersRefresh(Order order)
        {//TODO: распределение заявок по id 
            //id будет записан в TransId
            Clients.Group("Orders",order.TransID.ToString()).OnOrders(order);
        }
        //private Order order
    }
}
