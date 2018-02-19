using Common.Interfaces;
using Microsoft.AspNet.SignalR;
using QuikSharp.DataStructures.Transaction;
using Server.Quik;
using ServerCore.Hubs;
using ServerCore.Hubs.Interfaces;
using ServerCore.Hubs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Hubs
{
    public class OrdersHub: BaseHub<ClientBase, IHubClientCallbacks>//<IOrders>
    {
        public void SubscribeToOrdersRefresh()
        {
            long userID = long.Parse(CurrentUser.Id);
            Groups.Add(userID.ToString(), "Orders");
            QuikData.SubscribeToOrdersRefresh(ordersRefresh);
        }
        public void CancelOrder(Order order) //TODO:Создать свою модель
        {
           QuikConnector.Quik.Orders.KillOrder(order);
        }
        public List<Order> InitializeOrders()
        {
            //List<Order> orders=QuikConnector.Quik.Orders.GetOrders().Result;
            long userID = long.Parse(CurrentUser.Id);
            List<Order> userOrders = QuikConnector.Quik.Orders.GetOrders().Result.Where(x => x.TransID == userID).ToList();
            return userOrders;
        }
        private void ordersRefresh(Order order)
        {//TODO: распределение заявок по id 
            //id будет записан в TransId
            //Clients.Group("Orders",order.TransID.ToString()).OnOrders(order);
        }
    }
}
