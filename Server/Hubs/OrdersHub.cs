using Common.Interfaces;
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
        public void SubscribeToOrdersRefresh()
        {
            Groups.Add(Context.ConnectionId, "Orders");
            QuikData.SubscribeToOrdersRefresh(ordersRefresh);
        }
        public void CancelOrder(Order order) //TODO:Создать свою модель
        {
           QuikConnector.Quik.Orders.KillOrder(order);
        }
        public void InitializeOrders()
        {
            List<Order> orders=QuikConnector.Quik.Orders.GetOrders().Result;
            //TODO:выбрать ордеры по id для юзера
            Clients.Caller.OnInitialize(orders);
        }
        private void ordersRefresh(Order order)
        {//TODO: распределение заявок по id    
            Clients.Group("Orders").OnOrders(order);
        }

    }
}
